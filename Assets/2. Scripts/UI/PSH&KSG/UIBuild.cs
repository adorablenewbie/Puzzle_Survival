using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    private bool isPreviewActive = false;  // ���� - �����䰡 Ȱ��ȭ �Ǿ� �ִ���
    [SerializeField] private Button CampFireButton; 

    [SerializeField] private Build[] builds;   // ���� - ���
    [SerializeField] private GameObject previewPrefab;          // ���� - ���� �� ������ ������
    [SerializeField] private GameObject installPrefab;          // ���� - ������ ������ ������

    [SerializeField] private Image buildPanel;
    [SerializeField] private Transform player;  // �÷��̾��� Ʈ������

    private RaycastHit hitInfo;                    // ����ĳ��Ʈ ��Ʈ ������ ���� ����
    [SerializeField] private LayerMask layerMask;  // 
    [SerializeField] private float range;

    // ���콺 �ٷ� ȸ�� ����
    [SerializeField] private float rotateSpeed = 10f;
    private float currentRotationY = 0;

    public UIInventory inventory;
    [SerializeField] private ItemData woodItem;
    [SerializeField] private ItemData grassItem;
    [SerializeField] private ItemData rockItem;
    public event Action PreviewStart;
    public event Action PreviewEnd;
    public event Action WaringEvent;

    private void Awake()
    {
        PreviewStart += CloseBuildList;
    }

    // Start is called before the first frame update
    void Start()
    {
        buildPanel.gameObject.SetActive(false);
        if(inventory == null) inventory = FindObjectOfType<UIInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isPreviewActive);

        if (isPreviewActive)
        {
            PreviewPositionUpdate();
            PreviewRotationInput();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Build();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Cancel();
        }
    }


    public void OpenBuildList()
    {
        buildPanel.gameObject.SetActive (true);
    }

    public void CloseBuildList()
    {
        buildPanel.gameObject.SetActive(false);
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                previewPrefab.transform.position = location;
            }
        }
    }


    private void PreviewRotationInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (MathF.Abs(scroll) > 0.01f)
        {
            currentRotationY += scroll * rotateSpeed;
            previewPrefab.transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
        }
    }


    public void StartPreview(int slotNumber)
    {
        if (!CheckHasItem(builds[slotNumber], inventory.slots))
        {
            Debug.Log(CheckHasItem(builds[slotNumber], inventory.slots));
            WaringEvent?.Invoke();
            return;
        }
        previewPrefab = Instantiate(builds[slotNumber].previewPrefab, player.position + player.forward, Quaternion.identity);
        installPrefab = builds[slotNumber].installPrefab;
        isPreviewActive = true;

        PreviewStart?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Cancel()
    {
        if (isPreviewActive)
            Destroy(previewPrefab);

        isPreviewActive = false;
        previewPrefab = null;
        PreviewEnd?.Invoke();
    }

    private void Build()
    {
        if (isPreviewActive && previewPrefab.GetComponent<PreviewObject>().IsBuildable())
        {
            //Instantiate(installPrefab, hitInfo.point, previewPrefab.transform.rotation);
            Instantiate(installPrefab, previewPrefab.transform.position, previewPrefab.transform.rotation);
            Destroy(previewPrefab);
            isPreviewActive = false;
            previewPrefab = null;
            installPrefab = null;
            PreviewEnd?.Invoke();
            CloseBuildList();
        }
    }

    private bool CheckHasItem(Build data, ItemSlot[] inventorySlots)
    {
        foreach(BuildRequirement require in data.requirements)
        {
            ItemSlot slot = FindItemSlot(inventorySlots, require.item);
            if (slot == null || slot.quantity < require.amount)
                return false;
        }

        foreach(BuildRequirement require in data.requirements)
        {
            ItemSlot slot = FindItemSlot(inventorySlots,require.item);
            slot.quantity -= require.amount;
        }


        return true;
    }

    private ItemSlot FindItemSlot(ItemSlot[] inventory, ItemData targetItem)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null && inventory[i].item == targetItem)
            {
                return inventory[i];
            }
        }
        return null;
    }


}
