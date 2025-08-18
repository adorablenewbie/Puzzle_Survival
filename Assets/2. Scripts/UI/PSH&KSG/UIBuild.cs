using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    private bool isPreviewActive = false;  // 건축 - 프리뷰가 활성화 되어 있는지
    [SerializeField] private Button CampFireButton; 

    [SerializeField] private Build[] builds;   // 건축 - 목록
    [SerializeField]private GameObject previewPrefab;          // 건축 - 건축 전 프리뷰 프리펩
    [SerializeField] private GameObject installPrefab;          // 건축 - 실제로 생성될 프리펩

    [SerializeField] private Image buildPanel;
    [SerializeField] private Transform player;  // 플레이어의 트랜스폼

    private RaycastHit hitInfo;   // 레이캐스트 히트 정보를 담을 변수
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
