using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICraft : MonoBehaviour
{

    [SerializeField] private GameObject baseUI;  // UIBuild�� ���� ����
    [SerializeField] private Button productionButton; // ���� ��ư
    [SerializeField] private Button buildButton;      // ���� ��ư
    [SerializeField] private Image productionListPanel;  // ���� ����Ʈ �г�
    [SerializeField] private Image buildListPanel;       // ���� ����Ʈ �г�

    [SerializeField] private UIInventory inventory;

    public event Action OnBuildClick;
    public event Action OnProductionClick;

    private void Awake()
    {
        productionButton.onClick.AddListener(ProductionUIOpen);
        buildButton.onClick.AddListener(BuildUIOpen);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    private void ProductionUIOpen()
    {
        OnProductionClick?.Invoke();
    }

    private void BuildUIOpen()
    {
        OnBuildClick?.Invoke();
    }
}
