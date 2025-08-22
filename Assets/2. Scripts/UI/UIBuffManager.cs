using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuffManager : MonoBehaviour
{
    private PlayerController controller;
    private PlayerCondition condition;

    private Coroutine speedUpCoroutine;
    private Coroutine jumpUpCoroutine;
    private Coroutine doubleJumpCoroutine;
    private Coroutine invincibleCoroutine;
    private Coroutine healCoroutine;
    void Start()
    {
        controller = PlayerManager.Instance.Player.controller;
        condition = PlayerManager.Instance.Player.condition;
    }
    public void MakePermanentBuffUI(GameObject inputPrefab)
    {
        GameObject buffUI = Instantiate(inputPrefab, transform);
        buffUI.GetComponent<UIBuff>().isPermanent = true;

    }
    public void MakeTempBuffUI(GameObject inputPrefab, float inputDuration)
    {
        GameObject buffUI = Instantiate(inputPrefab, transform);
        buffUI.GetComponent<UIBuff>().remainDuration = inputDuration;
        buffUI.GetComponent<UIBuff>().duration = inputDuration;
    }

    public void AddPermanentBuff(BuffData buffData) // ���߿� �����͸� �� �� ���������ΰ� �Ͻ����ΰ� �Ǻ��� ���⼭ �غ���. (equiptool, UIInventory���� ���� ���ǽ� ����)
    {
        if (buffData == null) return;                       //���� ����Ʈ�� �ִ� ������ ù��° ����Ʈ�� ����Ǵ� ������ �ֱ⿡ ���ľ���
        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            controller.SpeedUp(buffData.effects[1].value);
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            controller.JumpUp(buffData.effects[1].value);
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
           controller.onDoubleJump = buffData.effects[0].isOn;
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            condition.uiCondition.health.SetConditionInvincible();
        }
        MakePermanentBuffUI(buffData.buffUI);
    }

    public void AddTemporaryBuff(BuffData buffData, float inputDuration)
    {
        if (buffData == null) return;
        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            speedUpCoroutine = StartCoroutine(controller.SpeedUpCoroutine(buffData.effects[0].value, inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            jumpUpCoroutine = StartCoroutine(controller.JumpUpCoroutine(buffData.effects[0].value, inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
            doubleJumpCoroutine = StartCoroutine(controller.DoubleJumpCoroutine(inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            invincibleCoroutine = StartCoroutine(condition.uiCondition.health.SetConditionInvincibleCoroutine(inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.Heal)
        {
            healCoroutine = StartCoroutine(condition.HealBuff(buffData.effects[0].value, inputDuration));
        }
        MakeTempBuffUI(buffData.buffUI, inputDuration);
    }
    public void RemovePermanentBuff(BuffData buffData)
    {
        if (buffData == null) return;

        // ���� ȿ�� ����
        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            controller.SpeedUp(-buffData.effects[0].value); // SpeedUp ������ �޼��� �ʿ�
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            controller.SpeedUp(-buffData.effects[0].value); // JumpUp ������ �޼��� �ʿ�
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
            controller.onDoubleJump = false;
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            condition.uiCondition.health.EndConditionInvincible();
        }

        // ���� UI ����
        RemoveBuffUI(buffData.buffUI.name);
        
    }

    public void RemoveTemporaryBuff(BuffData buffData)
    {
        if (buffData == null) return;

        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            if (speedUpCoroutine != null)
            {
                StopCoroutine(speedUpCoroutine);
                speedUpCoroutine = null;
                controller.SpeedUp(-buffData.effects[0].value);
            }
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            if (jumpUpCoroutine != null)
            {
                StopCoroutine(jumpUpCoroutine);
                jumpUpCoroutine = null;
                controller.JumpUp(-buffData.effects[0].value);
            }
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
            if (doubleJumpCoroutine != null)
            {
                StopCoroutine(doubleJumpCoroutine);
                doubleJumpCoroutine = null;
                controller.onDoubleJump = false;
            }
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            if (invincibleCoroutine != null)
            {
                StopCoroutine(invincibleCoroutine);
                invincibleCoroutine = null;
                condition.uiCondition.health.EndConditionInvincible();
            }
        }
        else if (buffData.effects[0].type ==  BuffType.Heal)
        {
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
                healCoroutine = null;
            }

        }

        RemoveBuffUI(buffData.buffUI.name);
    }

    // ���� UI ���� �޼���
    private void RemoveBuffUI(string buffUIName)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("(Clone)"))
            {
                int cloneIndex = child.gameObject.name.IndexOf("(Clone)");
                string name = child.gameObject.name.Substring(0, cloneIndex);
                Debug.Log("Checking child: " + name);
                Debug.Log("Removing buff UI: " + buffUIName);
            }
            if (child.gameObject.name == buffUIName+"(Clone)")
            {
                Debug.Log("Checking child: " + name);
                Debug.Log("Removing buff UI: " + buffUIName);
                Destroy(child.gameObject);
                break;

               
            }
        }
    }
}
