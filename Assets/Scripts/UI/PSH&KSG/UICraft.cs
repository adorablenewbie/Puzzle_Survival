using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICraft : MonoBehaviour
{
    private bool isActive = false;  // UIBuild가 활성화 되었는지

    [SerializeField] private GameObject baseUI;  // UIBuild를 담을 변수
    [SerializeField] private Button productionButton; // 제작 버튼
    [SerializeField] private Button buildButton;      // 빌드 버튼
    [SerializeField] private Image productionListPanel;  // 제작 리스트 패널
    [SerializeField] private Image buildListPanel;       // 빌드 리스트 패널

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
        // 레시피 정보 넣어주기
    }
    private void BuildUIOpen()
    {
        productionListPanel.gameObject?.SetActive(false);
        buildListPanel.gameObject?.SetActive(true);
        // 레시피 정보 넣어주기

    }
}
