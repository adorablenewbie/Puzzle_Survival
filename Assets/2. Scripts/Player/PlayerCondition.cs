using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public interface IPlayerCondition
{
    event Action onTakeDamage;
    event Action onDie;

    void Die();
    void Eat(global::System.Single amount);
    void Heal(global::System.Single amount);
    IEnumerator HealBuff(global::System.Single amount, global::System.Single duration);
    void juapWater(global::System.Single amount);
    void TakePhysicalDamage(global::System.Int32 damageAmount);
    global::System.Boolean UseStamina(global::System.Single amount);
}

public class PlayerCondition : MonoBehaviour, IDamagable, IPlayerCondition
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition thirst { get { return uiCondition.thirst; } }

    public float coldStaminaDecay;
    public float noHungerHealthDecay;
    public event Action onTakeDamage;
    public PlayerController playerController;
    private SceneFlowManager sceneFlowManager;

    private float lastShakeTime = -99f;

    public event Action onDie;

    [Header("DayNightCycle")]
    public DayNightCycle dayNightCycle;
    public float currentTime;


    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        thirst.Subtract(thirst.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            Damage(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0f)
        {
            StartCoroutine(DieSceneChange(3f));
        }

        ApplyColdStaminaDecayAtNight();
    }
  
    private void ApplyColdStaminaDecayAtNight()
    {
        
        currentTime = dayNightCycle.time;
        if (0.75f <= currentTime || currentTime <= 0.25f)
        {
            stamina.Subtract(coldStaminaDecay * Time.deltaTime);
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }
    public void Damage(float amount)
    {
        health.Subtract(amount);
        DamageEffect();
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
        thirst.Add(amount / 2f);
    }
    public void juapWater(float amount)
    {
        thirst.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

    public void DamageEffect()
    {
        if (Time.time - lastShakeTime >= 1f)
        {
            uiCondition.damageImage.gameObject.SetActive(true);
            lastShakeTime = Time.time;
            Camera.main.transform.DOShakePosition(0.3f, 0.15f, 10, 90, false, true);
            uiCondition.damageImage
                .DOFade(0.1f,0.1f)
                .SetLoops(2, LoopType.Incremental)
                .OnComplete(() => uiCondition.damageImage.gameObject.SetActive(false));
        }
    }

    public void Die()
    {
        uiCondition.diePanelImage.gameObject.SetActive(true);
        Color color = uiCondition.diePanelImage.color;
        color.a += Time.deltaTime;
        uiCondition.diePanel.SetActive(true);
        uiCondition.diePanelImage.color = color;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.moveSpeed = 0f;
        playerController.jumpPower = 0f;
        playerController.canLook = false;
        Debug.Log("Player has died.");
    }
    private IEnumerator DieSceneChange(float delay)
    {
        Debug.Log("�׾� ���� �׾ ������ �־�");
        Die();
        yield return new WaitForSeconds(delay);
        SceneFlowManager.Instance.SceneChange(SceneFlowManager.SceneType.MainScene);
    }


    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }


    public IEnumerator HealBuff(float amount, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Heal(amount / duration * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}