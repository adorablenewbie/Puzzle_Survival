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
    public ItemData soupItem;
    public ItemData potionItem;
    public ItemData axeItem;
    public ItemData swordItem;

    void Start()
    {
        gameObject.SetActive(false);
        if (uiInventory == null) uiInventory = FindObjectOfType<UIInventory>();
    }

    void Update()
    {
       // Debug.Log($"UIProduction {itemslots.Length}");
    }

    public void OpenProductionList() { gameObject.SetActive(true); }
    public void CloseProductionList() { gameObject.SetActive(false); }

    public void OnProductionSoupButton()
    {
        if (uiInventory == null || soupItem == null) { Debug.LogWarning("Soup ���� ���� ����"); return; }
        PlayerManager.Instance.Player.itemData = soupItem;
        uiInventory.AddItem();
        Debug.Log($"{soupItem.displayName} ���� �Ϸ�!");
    }

    public void OnProductionPotionButton()
    {
        if (uiInventory == null || potionItem == null) { Debug.LogWarning("Potion ���� ���� ����"); return; }
        PlayerManager.Instance.Player.itemData = potionItem;
        uiInventory.AddItem();
        Debug.Log($"{potionItem.displayName} ���� �Ϸ�!");
    }

    public void OnProductionAxeButton()
    {
        if (uiInventory == null || axeItem == null) { Debug.LogWarning("Axe ���� ���� ����"); return; }
        PlayerManager.Instance.Player.itemData = axeItem;
        uiInventory.AddItem();
        Debug.Log($"{axeItem.displayName} ���� �Ϸ�!");
    }

    public void OnProductionSwordButton()
    {
        if (uiInventory == null || swordItem == null) { Debug.LogWarning("Sword ���� ���� ����"); return; }
        PlayerManager.Instance.Player.itemData = swordItem;
        uiInventory.AddItem();
        Debug.Log($"{swordItem.displayName} ���� �Ϸ�!");
    }
}


