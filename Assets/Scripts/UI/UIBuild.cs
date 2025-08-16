using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    bool isActive;

    [SerializeField] private GameObject baseUI;
    [SerializeField] private Button productionButton;
    [SerializeField] private Button buildButton;
    [SerializeField] private Image listPanel;

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
        listPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UIControl(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ProductionUIOpen()
    {
        listPanel.gameObject?.SetActive(true);
        // 레시피 정보 넣어주기
    }
    private void BuildUIOpen()
    {
        listPanel.gameObject?.SetActive(true);
        // 레시피 정보 넣어주기

    }
}
