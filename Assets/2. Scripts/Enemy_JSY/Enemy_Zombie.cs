using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Zombie : MonoBehaviour, IDamagable
{

    [Header("���� ����")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // �ڽĵ� �͵��� �� ��������. => ������ �ٲٴ� �뵵
    }

    private void Start()
    {
        // ó�� �����ϸ� ��ǥ������ ��� �̵��� �� �ֵ��� Wandering(���ƴٴϴ� ����)���� ���� 
        SetState(AIState.Chasing);
        playerManager = PlayerManager.Instance;
        player = playerManager.Player;
        agent.SetDestination(player.transform.position);
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

        // walkSpeed�� �������� �ִϸ��̼� �ӵ��� ���� 
        animator.speed = agent.speed / walkSpeed;
    }

    // ���°� Attacking�� �ƴ� �� ��� ȣ��Ǵ� �޼��� 
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
            agent.isStopped = true; // ��� ���߰� => ���߰� ���� �ִϸ��̼��� �����ϱ� ����

            // ���� ��Ÿ���� ������
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage); // �÷��̾� ������ ó��
                animator.speed = 1; // �ִϸ��̼� ��� 

                animator.SetTrigger(Constant.AnimationParameter.Attack);
            }
        }
        //else
        //{
        //    // �÷��̾ �����Ÿ� �ȿ� ������ 
        //    if (playerDistance < detectDistance)
        //    {
        //        // �÷��̾ �����ؾ� ��.
        //        agent.isStopped = false;

        //        // �÷��̾� ��ġ���� ��θ� ����Ѵ�. �� �� �ִ� ���̸� true�� �׷��� ������ false�� ��ȯ 

        //        // �� �� ���� ���̸� ������ ���߰� ���� ��θ� Ž���ؾ� ��. 
        //        agent.SetDestination(player.transform.position); // �׷��� �ϴ��� ���� ��ġ�� �������� ��� 
        //        agent.isStopped = false; // ��� ����ٰ� 
        //        SetState(AIState.Chasing); // ���ƴٴϴ� ���·� ��ȯ�ؼ� ���� ��θ� Ž���� �� �ְ� �Ѵ�. 
        //    }

        //    // �÷��̾ �����Ÿ� �ۿ� ������ (�÷��̾ �ָ� ������)
        //    else
        //    {
        //        // ������ ����� ��.
        //        agent.SetDestination(player.transform.position); // ���� ��ġ�� ��ǥ�������� ���� 
        //        agent.isStopped = true; // ���͸� ���߰�
        //        SetState(AIState.Chasing); // �ٽ� ���ƴٴϴ� ���·� ��ȯ 
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

        // ������ ȿ��
        StartCoroutine(DamageFlash());
    }

    private void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            // ���� ��ġ���� 2���� ���� �������� ��� 
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