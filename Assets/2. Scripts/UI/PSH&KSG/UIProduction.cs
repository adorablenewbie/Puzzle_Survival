using System;
using UnityEngine;

public class UIProduction : MonoBehaviour
{
    public ItemSlot[] itemslots;
    public GameObject ProductionSoupButton;
    public GameObject ProductionPotionButton;
    public GameObject ProductionAxeButton;
    public GameObject ProductionSwordButton;

    // 인벤토리 / 제작 아이템 연결
    public UIInventory uiInventory;

    // 결과 아이템
    public ItemData soupItem;
    public ItemData potionItem;
    public ItemData axeItem;
    public ItemData swordItem;

    // 재료 아이템(Inspector에 할당)
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

    // -------- 공통 제작 로직 --------
    bool TryCraft(ItemData result, params (ItemData item, int count)[] reqs)
    {
        if (uiInventory == null || result == null)
        {
            Debug.LogWarning("제작 세팅 누락");
            return false;
        }

        // 1) 재료 보유 체크
        foreach (var r in reqs)
        {
            if (r.item == null || uiInventory.GetItemCount(r.item) < r.count)
            {
                WarnigEvent?.Invoke();
                Debug.LogWarning("재료 보유 체크 제작 세팅 누락");
                return false;
            }
        }

        // 2) 재료 차감 (모두 가능하다고 확정된 뒤 차감)
        foreach (var r in reqs)
        {
            // RemoveItems는 요청 수량만큼 제거되면 true 반환하도록 구현
            if (!uiInventory.RemoveItems(r.item, r.count))
            {
                Debug.LogWarning("제작 세팅 누락");
                return false;
            }
        }

        // 3) 결과 아이템 지급 (기존 방식 유지)
        PlayerManager.Instance.Player.itemData = result;
        uiInventory.AddItem();
        Debug.Log($"{result.displayName} 제작 완료!");
        return true;
    }

    // -------- 버튼 핸들러 --------
    // Soup: 나무1 + 풀1
    public void OnProductionSoupButton()
    {
        TryCraft(soupItem, (woodItem, 1), (grassItem, 1));
    }

    // Potion: 풀1 + 돌1
    public void OnProductionPotionButton()
    {
        TryCraft(potionItem, (grassItem, 1), (rockItem, 1));
    }

    // Axe: 나무1 + 풀1
    public void OnProductionAxeButton()
    {
        TryCraft(axeItem, (woodItem, 1), (rockItem, 1));
    }

    // Sword: 나무1 + 풀1
    public void OnProductionSwordButton()
    {
        TryCraft(swordItem, (woodItem, 1), (rockItem, 1));
    }
}

