using UnityEditor.UIElements;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    public float useStamina;
    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    [Header("Buff")]
    public ItemData itemData;

    private ItemBuff[] buffs;
    private UIBuffManager uiBuffManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
        uiBuffManager = PlayerManager.Instance.Player.uiBuffManager;
        buffs = itemData.itemBuff;
        OnBuff();
    }
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            if (PlayerManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
        }
    }

    public void OnBuff()
    {
        for(int i = 0; i < buffs.Length; i++)
        {
            for (int j = 0; j < buffs[i].buffData.effects.Length; j++)
            {
                if (buffs[i].buffData.effects[j] != null)
                {
                    if (buffs[i].isPermanent)
                    {
                        uiBuffManager.AddPermanentBuff(buffs[i].buffData);
                    }
                    else
                    {
                        uiBuffManager.AddTemporaryBuff(buffs[i].buffData, buffs[i].duration);
                    }
                }
            }
        }
    }
    private void OnDestroy()
    {
        RemoveBuffs();
    }
    public void RemoveBuffs()
    {
        if (uiBuffManager == null || buffs == null) return;

        for (int i = 0; i < buffs.Length; i++)
        {
            for (int j = 0; j < buffs[i].buffData.effects.Length; j++)
            {
                if (buffs[i].buffData.effects[j] != null)
                {
                    if (buffs[i].isPermanent)
                    {
                        uiBuffManager.RemovePermanentBuff(buffs[i].buffData);
                    }
                    else
                    {
                        uiBuffManager.RemoveTemporaryBuff(buffs[i].buffData);
                    }
                }
            }
        }
    }
}