using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    private bool isPreviewActive = false;  // ���� - �����䰡 Ȱ��ȭ �Ǿ� �ִ���
    [SerializeField] private Button CampFireButton; 

    [SerializeField] private Build[] builds;   // ���� - ���
    [SerializeField]private GameObject previewPrefab;          // ���� - ���� �� ������ ������
    [SerializeField] private GameObject installPrefab;          // ���� - ������ ������ ������

    [SerializeField] private Image buildPanel;
    [SerializeField] private Transform player;  // �÷��̾��� Ʈ������

    private RaycastHit hitInfo;   // ����ĳ��Ʈ ��Ʈ ������ ���� ����
    [SerializeField] private LayerMask layerMask;  // 
    [SerializeField] private float range;

    public ItemSlot[] itemslots;

    public event Action PreviewStart;
    public event Action PreviewEnd;

    private void Awake()
    {
        PreviewStart += CloseBuildList;
    }

    // Start is called before the first frame update
    void Start()
    {
        buildPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isPreviewActive);

        if (isPreviewActive)
        {
            PreviewPositionUpdate();
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

    public void StartPreview(int slotNumber)
    {
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
            Instantiate(installPrefab, hitInfo.point, Quaternion.identity);
            Destroy(previewPrefab);
            isPreviewActive = false;
            previewPrefab = null;
            installPrefab = null;
            PreviewEnd?.Invoke();
            CloseBuildList();
        }

    }

}
