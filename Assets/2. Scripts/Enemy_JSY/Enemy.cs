using UnityEngine;
using UnityEngine.AI;

// Enemy 속성만 
public class Enemy : MonoBehaviour
{
    protected enum AIState
    {
        Idle,
        Wandering, // 돌아다니는 상태
        Attacking,  // 공격 상태 
        Fear, // 두려움 상태 
        Dead
    }

    [Header("초기화 변수들")]
    protected Animator animator;

    [Header("몬스터 스탯")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;
    protected bool isDead = false;

    [Header("AI 네비게이션에 필요한 정보들")]
    protected NavMeshAgent agent;
    public float detectDistance; // 목표 지점까지 최소 거리 
    protected AIState aiState; // 몬스터 상태 정보 

    [Header("추적에 필요한 정보들")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime; // 새로운 목표지점을 찍을 때 기다리는 시간 (최소)
    public float maxWanderWaitTime; // 새로운 목표지점을 찍을 때 기다리는 시간 (최대)

    [Header("공격 정보들")]
    public int damage;
    public float attackRate; // 공격간 텀주는 시간 
    protected float lastAttackTime; // 마지막으로 공격한 시간
    public float attackDistance; // 공격 가능한 거리 
    protected float playerDistance; // 플레이어와의 거리 
    public float fieldOfView = 120f; // 시야 각도 

    [Header("참조 변수들")]
    protected SkinnedMeshRenderer[] meshRenderers;
    protected PlayerManager playerManager;
    protected Player player;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // 자식들 것들을 다 가져오기. => 색깔을 바꾸는 용도
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

    //    // walkSpeed를 기준으로 애니메이션 속도를 설정 
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
