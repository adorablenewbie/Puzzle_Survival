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

    public void AddPermanentBuff(BuffData buffData) // 나중에 리팩터링 할 때 영구구적인가 일시적인가 판별도 여기서 해보자. (equiptool, UIInventory에서 각자 조건식 있음)
    {
        if (buffData == null) return;                       //다중 이펙트가 있는 버프는 첫번째 이펙트만 적용되는 문제가 있기에 고쳐야함
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
        MakeTempBuffUI(buffData.buffUI, inputDuration);
    }
    public void RemovePermanentBuff(BuffData buffData)
    {
        if (buffData == null) return;

        // 버프 효과 해제
        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            controller.SpeedUp(-buffData.effects[0].value); // SpeedUp 해제용 메서드 필요
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            controller.SpeedUp(-buffData.effects[0].value); // JumpUp 해제용 메서드 필요
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
            controller.onDoubleJump = false;
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            condition.uiCondition.health.EndConditionInvincible();
        }

        // 버프 UI 제거
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

        RemoveBuffUI(buffData.buffUI.name);
    }

    // 버프 UI 제거 메서드
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
