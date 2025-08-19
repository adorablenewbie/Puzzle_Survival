using UnityEngine;
using UnityEngine.AI;

// Enemy �Ӽ��� 
public class Enemy : MonoBehaviour
{
    protected enum AIState
    {
        Idle,
        Wandering, // ���ƴٴϴ� ����
        Attacking,  // ���� ���� 
        Fear, // �η��� ���� 
        Dead
    }

    [Header("�ʱ�ȭ ������")]
    protected Animator animator;

    [Header("���� ����")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;
    protected bool isDead = false;

    [Header("AI �׺���̼ǿ� �ʿ��� ������")]
    protected NavMeshAgent agent;
    public float detectDistance; // ��ǥ �������� �ּ� �Ÿ� 
    protected AIState aiState; // ���� ���� ���� 

    [Header("������ �ʿ��� ������")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime; // ���ο� ��ǥ������ ���� �� ��ٸ��� �ð� (�ּ�)
    public float maxWanderWaitTime; // ���ο� ��ǥ������ ���� �� ��ٸ��� �ð� (�ִ�)

    [Header("���� ������")]
    public int damage;
    public float attackRate; // ���ݰ� ���ִ� �ð� 
    protected float lastAttackTime; // ���������� ������ �ð�
    public float attackDistance; // ���� ������ �Ÿ� 
    protected float playerDistance; // �÷��̾���� �Ÿ� 
    public float fieldOfView = 120f; // �þ� ���� 

    [Header("���� ������")]
    protected SkinnedMeshRenderer[] meshRenderers;
    protected PlayerManager playerManager;
    protected Player player;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // �ڽĵ� �͵��� �� ��������. => ������ �ٲٴ� �뵵
    }

    

    //protected void SetState(AIState state)
    //{
    //    aiState = state;

    //    switch (aiState)
    //    {
    //        case AIState.Idle:
    //            agent.speed = walkSpeed;
    //            agent.isStopped = true;
    //            break;
    //        case AIState.Attacking:
    //            agent.speed = walkSpeed;
    //            agent.isStopped = false;
    //            break;
    //        case AIState.Wandering:
    //            agent.speed = runSpeed;
    //            agent.isStopped = false;
    //            break;
    //    }

    //    // walkSpeed�� �������� �ִϸ��̼� �ӵ��� ���� 
    //    animator.speed = agent.speed / walkSpeed;
    //}

    private void OnDrawGizmosSelected()
    {
        // detectDistance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectDistance);

        // attackDistance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
