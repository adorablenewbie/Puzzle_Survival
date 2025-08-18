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

    // 마우스 휠로 회전 조정
    [SerializeField] private float rotateSpeed = 10f;
    private float currentRotationY = 0;

    // 스냅 기능
    [SerializeField] private float snapRadius = 2f;
    [SerializeField] private LayerMask snapMaske;

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

    private void PreviewRotationInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        if(MathF.Abs(scroll) > 0.01f)
        {
            currentRotationY += scroll * rotateSpeed;
            previewPrefab.transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
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
        Vector3 targetPos = Vector3.zero;
        //Quaternion targetRot = Quaternion.identity;

        if (Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                //Vector3 location = hitInfo.point;
                //previewPrefab.transform.position = location;

                targetPos = hitInfo.point;
                //targetRot = Quaternion.identity;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(previewPrefab.transform.position, snapRadius, snapMaske);

        Transform nearestSnap = null;
        float minDist = float.MaxValue;

        foreach (Collider col in colliders)
        {
            BuildSnapPoints buildSnap = col.GetComponent<BuildSnapPoints>();
            if (buildSnap == null) continue;

            foreach (Transform snapPoint in buildSnap.snapPoints)
            {
                float dist = Vector3.Distance(previewPrefab.transform.position, snapPoint.position);

                if(dist < minDist)
                {
                    minDist = dist;
                    nearestSnap = snapPoint;
                }
            }
        }

        if(nearestSnap != null)
        {
            BuildSnapPoints prevSnapPoint = previewPrefab.GetComponent<BuildSnapPoints>();

            if(prevSnapPoint != null && prevSnapPoint.snapPoints.Length >0)
            {
                Transform previewSnap = prevSnapPoint.snapPoints[0];

                Vector3 posOffset = previewPrefab.transform.position - previewSnap.position;
                previewPrefab.transform.position = nearestSnap.position + posOffset;

                Quaternion rotOffset = Quaternion.Inverse(previewSnap.rotation) * previewPrefab.transform.rotation;
                previewPrefab.transform.rotation = nearestSnap.rotation * rotOffset;

            }

            //previewPrefab.transform.position = nearestSnap.position;
            //previewPrefab.transform.rotation = nearestSnap.rotation;
        }
        else
        {
            previewPrefab.transform.position = targetPos;
            //previewPrefab.transform.rotation = targetRot;
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
            Instantiate(installPrefab, hitInfo.point, previewPrefab.transform.rotation);
            Destroy(previewPrefab);
            isPreviewActive = false;
            previewPrefab = null;
            installPrefab = null;
            PreviewEnd?.Invoke();
            CloseBuildList();
        }

    }

}
