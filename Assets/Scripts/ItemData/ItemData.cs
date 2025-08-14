using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health,
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}
[System.Serializable]
public class ItemBuff
{
    public BuffData buffData; // 버프 데이터
    public bool isPermanent; // 버프가 활성화되어 있는지 여부
    public float duration; // 버프 지속 시간
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Buff")]
    public ItemBuff[] itemBuff; // 버프 데이터 (버프가 있다면)

}