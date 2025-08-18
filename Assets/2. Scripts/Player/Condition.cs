using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public bool isConditionInvincible;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        if (isConditionInvincible) return;
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
    public void SetConditionInvincible()
    {

        isConditionInvincible = true;
    }
    public void EndConditionInvincible()
    {
        isConditionInvincible = false;
    }
    public IEnumerator SetConditionInvincibleCoroutine(float duration)
    {
        SetConditionInvincible();
        yield return new WaitForSeconds(duration);
        EndConditionInvincible();
    }
}
