using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        // 처음 시작하면 목표지점을 찍고 이동할 수 있도록 Wandering(돌아다니는 상태)으로 설정 
        SetState(AIState.Wandering);

        playerManager = PlayerManager.Instance;
        player = playerManager.Player;
    }

    protected void SetState(AIState state)
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
}
