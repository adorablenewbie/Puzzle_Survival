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
        // ó�� �����ϸ� ��ǥ������ ��� �̵��� �� �ֵ��� Wandering(���ƴٴϴ� ����)���� ���� 
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

        // walkSpeed�� �������� �ִϸ��̼� �ӵ��� ���� 
        animator.speed = agent.speed / walkSpeed;
    }
}
