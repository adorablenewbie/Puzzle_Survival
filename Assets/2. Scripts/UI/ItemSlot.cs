using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText; // 원래 철자 유지
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (icon != null)
        {
            icon.preserveAspect = true;
            icon.enabled = false;            // 초기 비활성
            icon.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (outline != null) outline.enabled = equipped;
        if (button != null)  // 인스펙터 연결 안 했어도 동작
        {
            button.onClick.RemoveListener(OnClickButton);
            button.onClick.AddListener(OnClickButton);
        }
    }

    public void Set()
    {
        if (item == null) { Clear(); return; }

        if (icon != null)
        {
            icon.sprite = item.icon;                 // ItemData.icon (Sprite)
            bool hasIcon = item.icon != null;
            icon.enabled = hasIcon;
            icon.gameObject.SetActive(hasIcon);
            icon.preserveAspect = true;
        }

        if (quatityText != null)
            quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null) outline.enabled = equipped;
    }

    public void Clear()
    {
        item = null;
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
            icon.gameObject.SetActive(false);
        }
        if (quatityText != null) quatityText.text = string.Empty;
        equipped = false;
        if (outline != null) outline.enabled = false;
    }

    public void OnClickButton()
    {
        if (inventory != null) inventory.SelectItem(index);
        else Debug.LogWarning("[ItemSlot] inventory 참조가 없습니다.");
    }
}


/*using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}*/