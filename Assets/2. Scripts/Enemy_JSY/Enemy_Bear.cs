using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Bear : EnemyController, IDamagable
{
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

    // ���°� Attacking�� �ƴ� �� ��� ȣ��Ǵ� �޼��� 
    private void PassiveUpdate()
    {
        // Wandering(���ƴٴϴ� ����)�̸鼭, ��ǥ�������� �Ÿ��� 0.1���� ������(��ǥ������ ����������)
        if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle); // Wandering�̸鼭, ��ǥ������ ���������� ��� Idle ���°� ��. 

            // minWanerWaitTime ~maxWanderWaitTime�� �ڿ� ���� ��ǥ������ ���� 
            Invoke("WanderToNewLocation", Random.Range(minWanderDistance, maxWanderDistance));
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
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        // hit�� �ִܰŸ� ��ΰ� �����

        // ������ ������Ʈ�� ���̸� 
        

        int i = 0; 
        // �����Ÿ�(detectDistance)���� �� �������� �ִܰŸ� ��ΰ� �������� 
        while(Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            // �ִܰŸ��� ��Ž�� 
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

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

    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }

        // ������ ȿ��
        StartCoroutine(DamageFlash());
    }

    private void Die()
    {
        SetState(AIState.Dead);

        agent.isStopped = true;
        //agent.enabled = false;
        //agent.SetDestination(transform.position);

        // �״� �ִϸ��̼� 
        if (!isDead)
        {
            Debug.Log("�״� �ִϸ��̼� ����");
            animator.SetBool(Constant.AnimationParameter.Dead, true);
            isDead = true;
        }

        // ������ ��� 
        for(int i = 0; i < dropOnDeath.Length; i++)
        {
            // ���� ��ġ���� 2���� ���� �������� ��� 
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

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
