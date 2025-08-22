using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle, 
    Chasing,
    Wandering, // 돌아다니는 상태
    Attacking  // 공격상태 
}

public class Enemy_Bear : MonoBehaviour, IDamagable
{
    [Header("몬스터 스탯")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;
    public bool isDead;

    [Header("AI 네비게이션에 필요한 정보들")]
    private NavMeshAgent agent;
    public float detectDistance; // 목표 지점까지 최소 거리 
    private AIState aiState; // 몬스터 상태 정보 

    [Header("추적에 필요한 정보들")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime; // 새로운 목표지점을 찍을 때 기다리는 시간 (최소)
    public float maxWanderWaitTime; // 새로운 목표지점을 찍을 때 기다리는 시간 (최대)

    [Header("공격 정보들")]
    public int damage; 
    public float attackRate; // 공격간 텀주는 시간 
    private float lastAttackTime; // 마지막으로 공격한 시간
    public float attackDistance; // 공격 가능한 거리 
    private float playerDistance; // 플레이어와의 거리 
    public float fieldOfView = 120f; // 시야 각도 

    [Header("참조 변수들")]
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;
    private PlayerManager playerManager;
    private Player player;
    private BoxCollider col;

    public event Action<Enemy_Bear> OnDie;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // 자식들 것들을 다 가져오기. => 색깔을 바꾸는 용도
        col = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        // 처음 시작하면 목표지점을 찍고 이동할 수 있도록 Wandering(돌아다니는 상태)으로 설정 
        SetState(AIState.Wandering);

        playerManager = PlayerManager.Instance;
        player = playerManager.Player;
    }

    private void Update()
    {
        // 플레이어와의 거리를 계속 체크 => 거리에 따라 몬스터의 상태가 바뀌므로 
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        // Idle 상태가 아니면 움직이는 애니메이션 적용 
        animator.SetBool(Constant.AnimationParameter.Moving, aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Attacking:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Wandering:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        // walkSpeed를 기준으로 애니메이션 속도를 설정 
        animator.speed = agent.speed / walkSpeed;
    }

    // 상태가 Attacking이 아닐 때 계속 호출되는 메서드 
    private void PassiveUpdate()
    {
        // Wandering(돌아다니는 상태)이면서, 목표지점과의 거리가 0.1보다 작으면(목표지점에 도착했으면)
        if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle); // Wandering이면서, 목표지점에 도착했으면 계속 Idle 상태가 됨. 

            // minWanerWaitTime ~maxWanderWaitTime초 뒤에 다음 목표지점을 설정 
            Invoke("WanderToNewLocation", UnityEngine.Random.Range(minWanderDistance, maxWanderDistance));
        }

        // 플레이어가 감지거리 안에 있으면 
        if(playerDistance < detectDistance)
        {
            // 공격상태로 전환
            SetState(AIState.Attacking); 
        }
    }

    private void WanderToNewLocation()
    {
        // 당연히 Idle 상태이겠지만 혹시 모르니 예외처리 
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);

        // 다음 목표지점 설정 
        agent.SetDestination(GetWanderLocation());
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        // SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
        // sourcePosition에서 가장 가까운 NavMesh 영역 위 좌표를 찾아서 hit에 반환 
        // 찾을 때 sourcePosition에서 maxDistance 거리까지 찾는다. 
        // 모든 영역을 찾는다 (NavMesh.AllAreas)

        // transform.position = 몬스터 위치(곰) 
        // Random.onUnitSphere = 원점(0,0,0)에서 반지름이 1인 가상의 구에서 무작위 한 점을 반환 
        // transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance))
        // = 현재 위치에서 반지름이 1인 가상의 구의 무작위 한 점에 랜덤 값을 곱한 값을 더한 위치 
        // 현재 위치에서 반지름이 1인 가상의 구의 무작위 한 점에 랜덤 값을 곱한 값을 더한 위치에서 
        // maxWanderDistance 거리까지 가장 가까운 NavMesh 영역 위 좌표를 찾아서 hit에 반환한다. 
        // 영역 제한은 없다. (NavMesh.AllAreas)
        NavMesh.SamplePosition(transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        // hit에 최단거리 경로가 저장됨

        int i = 0; 
        // 감지거리(detectDistance)보다 더 안쪽으로 최단거리 경로가 잡혔으면 
        while(Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            // 최단거리를 재탐색 
            NavMesh.SamplePosition(transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

            // 감지거리(detectDistance) 밖으로 최단거리가 잡힐 때까지 30번 시도 
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    private void AttackingUpdate()
    {
        // 플레이어가 공격범위 안에 있고, 시야각에 있다면
        if(playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true; // 잠깐 멈추고 => 멈추고 공격 애니메이션을 실행하기 위함

            // 공격 쿨타임이 끝나면
            if(Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage); // 플레이어 데미지 처리
                animator.speed = 1; // 애니메이션 재생 

                animator.SetTrigger(Constant.AnimationParameter.Attack);
            }
        }

        else
        {
            // 플레이어가 감지거리 안에 있으면 
            if(playerDistance < detectDistance)
            {
                // 플레이어를 추적해야 함.
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath(); // 경로 정보를 담아줄 NavMeshPath

                // 플레이어 위치까지 경로를 계산한다. 갈 수 있는 곳이면 true를 그렇지 않으면 false를 반환 
                if(agent.CalculatePath(player.transform.position, path))
                {
                    // 갈 수 있는 곳이면 목표지점을 플레이어의 위치로 설정 
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    // 갈 수 없는 곳이면 추적을 멈추고 다음 경로를 탐색해야 함. 
                    agent.SetDestination(transform.position); // 그래서 일단은 현재 위치로 목적지를 잡고 
                    agent.isStopped = false; // 잠깐 멈췄다가 
                    SetState(AIState.Wandering); // 돌아다니는 상태로 전환해서 다음 경로를 탐색할 수 있게 한다. 
                }
            }

            // 플레이어가 감지거리 밖에 있으면 (플레이어가 멀리 있으면)
            else
            {
                // 추적을 멈춰야 함.
                agent.SetDestination(transform.position); // 현재 위치를 목표지점으로 설정 
                agent.isStopped = true; // 몬스터를 멈추고
                SetState(AIState.Wandering); // 다시 돌아다니는 상태로 전환 
            }
        }
    }

    // 플레이어가 시야각에 있는지 확인 
    private bool IsPlayerInFieldOfView()
    {
        // 몬스터가 플레이어를 바라보는 벡터
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // 몬스터가 정면으로 바라보는 방향과 몬스터가 플레이어를 바라보는 방향 사이 각도 
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // 시야 각도에 들어오면 true를 아니면 false를 반환 
        return angle < fieldOfView * 0.5f;
    }

    private void OnDrawGizmosSelected()
    {
        // detectDistance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectDistance);

        // attackDistance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        if (health > 0)
        {
            health -= damage;
            // 데미지 효과
            StartCoroutine(DamageFlash());
        }
        

        if(health <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        if (!isDead)
        {
            animator.SetBool(Constant.AnimationParameter.Death, true);
            agent.isStopped = true;
            isDead = true;
            col.enabled = false;
        }

        for(int i = 0; i < dropOnDeath.Length; i++)
        {
            // 몬스터 위치에서 2정도 위로 아이템을 드랍 
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        OnDie?.Invoke(this);

        Destroy(this.gameObject, 3.0f);
    }

    private IEnumerator DamageFlash()
    {
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for(int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }
}
