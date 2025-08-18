using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICraft : MonoBehaviour
{

    [SerializeField] private GameObject baseUI;  // UIBuild를 담을 변수
    [SerializeField] private Button productionButton; // 제작 버튼
    [SerializeField] private Button buildButton;      // 빌드 버튼
    [SerializeField] private Image productionListPanel;  // 제작 리스트 패널
    [SerializeField] private Image buildListPanel;       // 빌드 리스트 패널

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
