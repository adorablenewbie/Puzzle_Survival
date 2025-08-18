using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    private bool isPreviewActive = false;  // 건축 - 프리뷰가 활성화 되어 있는지
    [SerializeField] private Button CampFireButton; 

    [SerializeField] private Build[] builds;   // 건축 - 목록
    [SerializeField] private GameObject previewPrefab;          // 건축 - 건축 전 프리뷰 프리펩
    [SerializeField] private GameObject installPrefab;          // 건축 - 실제로 생성될 프리펩

    [SerializeField] private Image buildPanel;
    [SerializeField] private Transform player;  // 플레이어의 트랜스폼

    private RaycastHit hitInfo;                    // 레이캐스트 히트 정보를 담을 변수
    [SerializeField] private LayerMask layerMask;  // 
    [SerializeField] private float range;

    [Header("Snap Settings")]
    [SerializeField] private float snapRadius = 5f;
    [SerializeField] private float snapEnter = 0.5f;
    [SerializeField] private float snapExit = 1f;
    [SerializeField] private LayerMask snapLayer;
    private bool isSnapped = false;
    private int currentSnapIndex = 0;
    private Transform currentSnapPoint;

    // 마우스 휠로 회전 조정
    [SerializeField] private float rotateSpeed = 10f;
    private float currentRotationY = 0;

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
        Vector3 targetPos = Vector3.zero;
        Quaternion targetRot = Quaternion.identity;

        if (Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                //    Vector3 location = hitInfo.point;
                //    previewPrefab.transform.position = location;

                targetPos = hitInfo.point;
                targetRot = Quaternion.Euler(0, currentRotationY, 0);
            }

            Transform nearestSnap = FindNearestSnapPoint(targetPos, out float snapDist);

            if(!isSnapped)
            {
                if(nearestSnap != null && snapDist <= snapEnter)
                {
                    isSnapped = true;
                    currentSnapPoint = nearestSnap;
                }
            }
            else
            {
                if(currentSnapPoint == null)
                {
                    isSnapped = false;
                }
                else
                {
                    float curDist = Vector3.Distance(targetPos, currentSnapPoint.position);
                    if(curDist >= snapExit)
                    {
                        isSnapped = false;
                        currentSnapPoint = null;
                    }
                }
            }

            if(isSnapped && currentSnapPoint != null)
            {
                BuildSnapPoints snaps = previewPrefab.GetComponent<BuildSnapPoints>();
                if(snaps != null && snaps.snapPoints.Length > 0)
                {
                    Transform bestLocalSnap = null;
                    float minDist = Mathf.Infinity;

                    for(int i = 0; i < snaps.snapPoints.Length; i++)
                    {
                        float d = Vector3.Distance(currentSnapPoint.position, snaps.snapPoints[i].position);
                        if(d < minDist)
                        {
                            minDist = d;
                            bestLocalSnap = snaps.snapPoints[i];
                        }
                    }

                    if(bestLocalSnap != null)
                    {
                        Vector3 offset = previewPrefab.transform.position - bestLocalSnap.position;
                        previewPrefab.transform.position = currentSnapPoint.position + offset;
                    }
                }
                PreviewRotationInput();

                //previewPrefab.transform.SetPositionAndRotation(currentSnapPoint.position, currentSnapPoint.rotation);
                previewPrefab.transform.RotateAround(currentSnapPoint.position, Vector3.up, currentRotationY);
            }
            else
            {
                previewPrefab.transform.SetPositionAndRotation(targetPos, targetRot);
                PreviewRotationInput();
            }

        }
        else
        {
            previewPrefab.transform.SetPositionAndRotation(targetPos, targetRot);
            PreviewRotationInput();

            isSnapped = false;
            currentSnapPoint = null;
        }



    }

    private Transform FindNearestSnapPoint(Vector3 pos, out float snapDist)
    {
        snapDist = float.MaxValue;
        Transform nearest = null;

        // 주변 건축물 찾기
        Collider[] cols = Physics.OverlapSphere(pos, snapRadius, snapLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            // 해당 오브젝트 아래 모든 BuildSnapPoint 수집
            BuildSnapPoints[] snaps = cols[i].GetComponentsInChildren<BuildSnapPoints>(true);
            if (snaps == null || snaps.Length == 0) continue;

            foreach (BuildSnapPoints sp in snaps)
            {
                float d = Vector3.Distance(pos, sp.transform.position);
                if (d < snapDist)
                {
                    snapDist = d;
                    nearest = sp.transform;
                }
            }
        }
        return nearest;
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

}
