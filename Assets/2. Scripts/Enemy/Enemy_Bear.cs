using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle, 
    Chasing,
    Wandering, // ���ƴٴϴ� ����
    Attacking  // ���ݻ��� 
}

public class Enemy_Bear : MonoBehaviour, IDamagable
{
    [Header("���� ����")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;
    public bool isDead;

    [Header("AI �׺���̼ǿ� �ʿ��� ������")]
    private NavMeshAgent agent;
    public float detectDistance; // ��ǥ �������� �ּ� �Ÿ� 
    private AIState aiState; // ���� ���� ���� 

    [Header("������ �ʿ��� ������")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime; // ���ο� ��ǥ������ ���� �� ��ٸ��� �ð� (�ּ�)
    public float maxWanderWaitTime; // ���ο� ��ǥ������ ���� �� ��ٸ��� �ð� (�ִ�)

    [Header("���� ������")]
    public int damage; 
    public float attackRate; // ���ݰ� ���ִ� �ð� 
    private float lastAttackTime; // ���������� ������ �ð�
    public float attackDistance; // ���� ������ �Ÿ� 
    private float playerDistance; // �÷��̾���� �Ÿ� 
    public float fieldOfView = 120f; // �þ� ���� 

    [Header("���� ������")]
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
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // �ڽĵ� �͵��� �� ��������. => ������ �ٲٴ� �뵵
        col = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        // ó�� �����ϸ� ��ǥ������ ��� �̵��� �� �ֵ��� Wandering(���ƴٴϴ� ����)���� ���� 
        SetState(AIState.Wandering);

        playerManager = PlayerManager.Instance;
        player = playerManager.Player;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ��� ��� üũ => �Ÿ��� ���� ������ ���°� �ٲ�Ƿ� 
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        // Idle ���°� �ƴϸ� �����̴� �ִϸ��̼� ���� 
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

        // walkSpeed�� �������� �ִϸ��̼� �ӵ��� ���� 
        animator.speed = agent.speed / walkSpeed;
    }

    // ���°� Attacking�� �ƴ� �� ��� ȣ��Ǵ� �޼��� 
    private void PassiveUpdate()
    {
        // Wandering(���ƴٴϴ� ����)�̸鼭, ��ǥ�������� �Ÿ��� 0.1���� ������(��ǥ������ ����������)
        if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle); // Wandering�̸鼭, ��ǥ������ ���������� ��� Idle ���°� ��. 

            // minWanerWaitTime ~maxWanderWaitTime�� �ڿ� ���� ��ǥ������ ���� 
            Invoke("WanderToNewLocation", UnityEngine.Random.Range(minWanderDistance, maxWanderDistance));
        }

        // �÷��̾ �����Ÿ� �ȿ� ������ 
        if(playerDistance < detectDistance)
        {
            // ���ݻ��·� ��ȯ
            SetState(AIState.Attacking); 
        }
    }

    private void WanderToNewLocation()
    {
        // �翬�� Idle �����̰����� Ȥ�� �𸣴� ����ó�� 
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);

        // ���� ��ǥ���� ���� 
        agent.SetDestination(GetWanderLocation());
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        // SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
        // sourcePosition���� ���� ����� NavMesh ���� �� ��ǥ�� ã�Ƽ� hit�� ��ȯ 
        // ã�� �� sourcePosition���� maxDistance �Ÿ����� ã�´�. 
        // ��� ������ ã�´� (NavMesh.AllAreas)

        // transform.position = ���� ��ġ(��) 
        // Random.onUnitSphere = ����(0,0,0)���� �������� 1�� ������ ������ ������ �� ���� ��ȯ 
        // transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance))
        // = ���� ��ġ���� �������� 1�� ������ ���� ������ �� ���� ���� ���� ���� ���� ���� ��ġ 
        // ���� ��ġ���� �������� 1�� ������ ���� ������ �� ���� ���� ���� ���� ���� ���� ��ġ���� 
        // maxWanderDistance �Ÿ����� ���� ����� NavMesh ���� �� ��ǥ�� ã�Ƽ� hit�� ��ȯ�Ѵ�. 
        // ���� ������ ����. (NavMesh.AllAreas)
        NavMesh.SamplePosition(transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        // hit�� �ִܰŸ� ��ΰ� �����

        int i = 0; 
        // �����Ÿ�(detectDistance)���� �� �������� �ִܰŸ� ��ΰ� �������� 
        while(Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            // �ִܰŸ��� ��Ž�� 
            NavMesh.SamplePosition(transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

            // �����Ÿ�(detectDistance) ������ �ִܰŸ��� ���� ������ 30�� �õ� 
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    private void AttackingUpdate()
    {
        // �÷��̾ ���ݹ��� �ȿ� �ְ�, �þ߰��� �ִٸ�
        if(playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true; // ��� ���߰� => ���߰� ���� �ִϸ��̼��� �����ϱ� ����

            // ���� ��Ÿ���� ������
            if(Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage); // �÷��̾� ������ ó��
                animator.speed = 1; // �ִϸ��̼� ��� 

                animator.SetTrigger(Constant.AnimationParameter.Attack);
            }
        }

        else
        {
            // �÷��̾ �����Ÿ� �ȿ� ������ 
            if(playerDistance < detectDistance)
            {
                // �÷��̾ �����ؾ� ��.
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath(); // ��� ������ ����� NavMeshPath

                // �÷��̾� ��ġ���� ��θ� ����Ѵ�. �� �� �ִ� ���̸� true�� �׷��� ������ false�� ��ȯ 
                if(agent.CalculatePath(player.transform.position, path))
                {
                    // �� �� �ִ� ���̸� ��ǥ������ �÷��̾��� ��ġ�� ���� 
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    // �� �� ���� ���̸� ������ ���߰� ���� ��θ� Ž���ؾ� ��. 
                    agent.SetDestination(transform.position); // �׷��� �ϴ��� ���� ��ġ�� �������� ��� 
                    agent.isStopped = false; // ��� ����ٰ� 
                    SetState(AIState.Wandering); // ���ƴٴϴ� ���·� ��ȯ�ؼ� ���� ��θ� Ž���� �� �ְ� �Ѵ�. 
                }
            }

            // �÷��̾ �����Ÿ� �ۿ� ������ (�÷��̾ �ָ� ������)
            else
            {
                // ������ ����� ��.
                agent.SetDestination(transform.position); // ���� ��ġ�� ��ǥ�������� ���� 
                agent.isStopped = true; // ���͸� ���߰�
                SetState(AIState.Wandering); // �ٽ� ���ƴٴϴ� ���·� ��ȯ 
            }
        }
    }

    // �÷��̾ �þ߰��� �ִ��� Ȯ�� 
    private bool IsPlayerInFieldOfView()
    {
        // ���Ͱ� �÷��̾ �ٶ󺸴� ����
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // ���Ͱ� �������� �ٶ󺸴� ����� ���Ͱ� �÷��̾ �ٶ󺸴� ���� ���� ���� 
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // �þ� ������ ������ true�� �ƴϸ� false�� ��ȯ 
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
            // ������ ȿ��
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
            // ���� ��ġ���� 2���� ���� �������� ��� 
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
