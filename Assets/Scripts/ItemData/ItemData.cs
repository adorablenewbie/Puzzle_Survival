using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable,
    Harvestable
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
    public BuffData buffData; // ���� ������
    public bool isPermanent; // ������ Ȱ��ȭ�Ǿ� �ִ��� ����
    public float duration; // ���� ���� �ð�
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

    [Header("Resource")]
    public int resourceAmount;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Buff")]
    public ItemBuff[] itemBuff; // ���� ������ (������ �ִٸ�)

}