using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICraft : MonoBehaviour
{
    private bool isActive = false;  // UIBuild�� Ȱ��ȭ �Ǿ�����

    [SerializeField] private GameObject baseUI;  // UIBuild�� ���� ����
    [SerializeField] private Button productionButton; // ���� ��ư
    [SerializeField] private Button buildButton;      // ���� ��ư
    [SerializeField] private Image productionListPanel;  // ���� ����Ʈ �г�
    [SerializeField] private Image buildListPanel;       // ���� ����Ʈ �г�

    [SerializeField] private UIInventory inventory;
    [SerializeField] private UIBuild build;
    [SerializeField] private UIProduction production;

    private void Awake()
    {
        productionButton.onClick.AddListener(ProductionUIOpen);
        buildButton.onClick.AddListener(BuildUIOpen);
    }

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        baseUI.SetActive(isActive);
        productionListPanel.gameObject.SetActive(false);
        buildListPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            build.itemslots = inventory.slots;
            production.itemslots = inventory.slots;
        }
    }

    public void UIControl(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isActive)
            {
                OpenUI();
            }
            else
            {
                CloseUI();
            }
        }
    }

    private void OpenUI()
    {
        isActive = true;
        baseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

    }

    private void CloseUI()
    {
        isActive = false;
        baseUI.SetActive(false);
        productionListPanel.gameObject.SetActive(false);
        buildListPanel.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ProductionUIOpen()
    {
        buildListPanel.gameObject?.SetActive(false);
        productionListPanel.gameObject?.SetActive(true);
        // ������ ���� �־��ֱ�
    }
    private void BuildUIOpen()
    {
        productionListPanel.gameObject?.SetActive(false);
        buildListPanel.gameObject?.SetActive(true);
        // ������ ���� �־��ֱ�

    }
}
