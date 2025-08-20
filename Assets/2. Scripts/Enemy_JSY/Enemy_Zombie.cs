using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Zombie : MonoBehaviour, IDamagable
{

    [Header("몬스터 스탯")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // 자식들 것들을 다 가져오기. => 색깔을 바꾸는 용도
    }

    private void Start()
    {
        // 처음 시작하면 목표지점을 찍고 이동할 수 있도록 Wandering(돌아다니는 상태)으로 설정 
        SetState(AIState.Chasing);
        playerManager = PlayerManager.Instance;
        player = playerManager.Player;
        agent.SetDestination(player.transform.position);
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
            case AIState.Chasing:
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
            case AIState.Chasing:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
        }

        // walkSpeed를 기준으로 애니메이션 속도를 설정 
        animator.speed = agent.speed / walkSpeed;
    }

    // 상태가 Attacking이 아닐 때 계속 호출되는 메서드 
    private void PassiveUpdate()
    {
        if (playerDistance > attackDistance)
        {
            agent.SetDestination(player.transform.position);
            SetState(AIState.Chasing);
        }
        else
        {
            SetState(AIState.Attacking);
        }
    }


    private void AttackingUpdate()
    {
        if (playerDistance < attackDistance)
        {
            agent.isStopped = true; // 잠깐 멈추고 => 멈추고 공격 애니메이션을 실행하기 위함

            // 공격 쿨타임이 끝나면
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage); // 플레이어 데미지 처리
                animator.speed = 1; // 애니메이션 재생 

                animator.SetTrigger(Constant.AnimationParameter.Attack);
            }
        }
        //else
        //{
        //    // 플레이어가 감지거리 안에 있으면 
        //    if (playerDistance < detectDistance)
        //    {
        //        // 플레이어를 추적해야 함.
        //        agent.isStopped = false;

        //        // 플레이어 위치까지 경로를 계산한다. 갈 수 있는 곳이면 true를 그렇지 않으면 false를 반환 

        //        // 갈 수 없는 곳이면 추적을 멈추고 다음 경로를 탐색해야 함. 
        //        agent.SetDestination(player.transform.position); // 그래서 일단은 현재 위치로 목적지를 잡고 
        //        agent.isStopped = false; // 잠깐 멈췄다가 
        //        SetState(AIState.Chasing); // 돌아다니는 상태로 전환해서 다음 경로를 탐색할 수 있게 한다. 
        //    }

        //    // 플레이어가 감지거리 밖에 있으면 (플레이어가 멀리 있으면)
        //    else
        //    {
        //        // 추적을 멈춰야 함.
        //        agent.SetDestination(player.transform.position); // 현재 위치를 목표지점으로 설정 
        //        agent.isStopped = true; // 몬스터를 멈추고
        //        SetState(AIState.Chasing); // 다시 돌아다니는 상태로 전환 
        //    }
        //}
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
        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        // 데미지 효과
        StartCoroutine(DamageFlash());
    }

    private void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            // 몬스터 위치에서 2정도 위로 아이템을 드랍 
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }

    private IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }
}