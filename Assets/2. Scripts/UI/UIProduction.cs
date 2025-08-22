using System;
using UnityEngine;

public class UIProduction : MonoBehaviour
{
    public ItemSlot[] itemslots;
    public GameObject ProductionSoupButton;
    public GameObject ProductionPotionButton;
    public GameObject ProductionAxeButton;
    public GameObject ProductionSwordButton;

    // �κ��丮 / ���� ������ ����
    public UIInventory uiInventory;

    // ��� ������
    public ItemData soupItem;
    public ItemData potionItem;
    public ItemData axeItem;
    public ItemData swordItem;

    // ��� ������(Inspector�� �Ҵ�)
    public ItemData woodItem;
    public ItemData grassItem;
    public ItemData rockItem;

    public event Action WarnigEvent;

    void Start()
    {
        gameObject.SetActive(false);
        if (uiInventory == null) uiInventory = FindObjectOfType<UIInventory>();
    }

    public void OpenProductionList() { gameObject.SetActive(true); }
    public void CloseProductionList() { gameObject.SetActive(false); }

    // -------- ���� ���� ���� --------
    bool TryCraft(ItemData result, params (ItemData item, int count)[] reqs)
    {
        if (uiInventory == null || result == null)
        {
            Debug.LogWarning("���� ���� ����");
            return false;
        }

        // 1) ��� ���� üũ
        foreach (var r in reqs)
        {
            if (r.item == null || uiInventory.GetItemCount(r.item) < r.count)
            {
                WarnigEvent?.Invoke();
                Debug.LogWarning("��� ���� üũ ���� ���� ����");
                return false;
            }
        }

        // 2) ��� ���� (��� �����ϴٰ� Ȯ���� �� ����)
        foreach (var r in reqs)
        {
            // RemoveItems�� ��û ������ŭ ���ŵǸ� true ��ȯ�ϵ��� ����
            if (!uiInventory.RemoveItems(r.item, r.count))
            {
                Debug.LogWarning("���� ���� ����");
                return false;
            }
        }

        // 3) ��� ������ ���� (���� ��� ����)
        PlayerManager.Instance.Player.itemData = result;
        uiInventory.AddItem();
        Debug.Log($"{result.displayName} ���� �Ϸ�!");
        return true;
    }

    // -------- ��ư �ڵ鷯 --------
    // Soup: ����1 + Ǯ1
    public void OnProductionSoupButton()
    {
        TryCraft(soupItem, (woodItem, 1), (grassItem, 1));
    }

    // Potion: Ǯ1 + ��1
    public void OnProductionPotionButton()
    {
        TryCraft(potionItem, (grassItem, 1), (rockItem, 1));
    }

    // Axe: ����1 + Ǯ1
    public void OnProductionAxeButton()
    {
        TryCraft(axeItem, (woodItem, 1), (rockItem, 1));
    }

    // Sword: ����1 + Ǯ1
    public void OnProductionSwordButton()
    {
        TryCraft(swordItem, (woodItem, 1), (rockItem, 1));
    }
}

