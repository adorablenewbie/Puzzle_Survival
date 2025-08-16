using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    private bool isActive = false;
    private bool isPreviewActive = false;

    [SerializeField] private GameObject baseUI;
    [SerializeField] private Button productionButton;
    [SerializeField] private Button buildButton;
    [SerializeField] private Image productionListPanel;
    [SerializeField] private Image buildListPanel;

    [SerializeField] private Build[] builds;
    private GameObject previewPrefab;
    private GameObject installPrefab;

    [SerializeField] private Transform player;

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

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
        if (isPreviewActive) PreviewPositionUpdate();

        if(Input.GetMouseButtonDown(0))
        {
            Build();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                previewPrefab.transform.position = location;
            }
        }
    }

    public void SlotClick(int slotNumber)
    {
        previewPrefab = Instantiate(builds[slotNumber].previewPrefab, player.position + player.forward, Quaternion.identity);
        installPrefab = builds[slotNumber].installPrefab;
        isPreviewActive = true;
        baseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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
        productionListPanel.gameObject.SetActive(false);
        buildListPanel.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Cancel()
    {
        if (isPreviewActive)
            Destroy(previewPrefab);

        isActive = false;
        isPreviewActive= false;
        previewPrefab = null;
        CloseUI();
    }

    private void Build()
    {
        if (isPreviewActive && previewPrefab.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(installPrefab, hitInfo.point, Quaternion.identity);
            Destroy(previewPrefab);
            isActive = false;
            isPreviewActive = false;
            previewPrefab = null;
            installPrefab = null;
        }

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
