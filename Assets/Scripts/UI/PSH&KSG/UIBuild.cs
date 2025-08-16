using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    private bool isPreviewActive = false;  // ���� - �����䰡 Ȱ��ȭ �Ǿ� �ִ���

    [SerializeField] private Build[] builds;   // ���� - ���
    private GameObject previewPrefab;          // ���� - ���� �� ������ ������
    private GameObject installPrefab;          // ���� - ������ ������ ������

    [SerializeField] private Transform player;  // �÷��̾��� Ʈ������

    private RaycastHit hitInfo;   // ����ĳ��Ʈ ��Ʈ ������ ���� ����
    [SerializeField] private LayerMask layerMask;  // 
    [SerializeField] private float range;

    public ItemSlot[] itemslots;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"UIBuild {itemslots.Length}");
        //if (isPreviewActive) PreviewPositionUpdate();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Build();
        //}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cancel();
        //}
    }

    //private void PreviewPositionUpdate()
    //{
    //    if (Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
    //    {
    //        if(hitInfo.transform != null)
    //        {
    //            Vector3 location = hitInfo.point;
    //            previewPrefab.transform.position = location;
    //        }
    //    }
    //}

    //public void SlotClick(int slotNumber)
    //{
    //    previewPrefab = Instantiate(builds[slotNumber].previewPrefab, player.position + player.forward, Quaternion.identity);
    //    installPrefab = builds[slotNumber].installPrefab;
    //    isPreviewActive = true;
    //    //baseUI.SetActive(false);
    //    Cursor.lockState = CursorLockMode.Locked;
    //}




    //private void Cancel()
    //{
    //    if (isPreviewActive)
    //        Destroy(previewPrefab);

    //    isActive = false;
    //    isPreviewActive= false;
    //    previewPrefab = null;
    //    CloseUI();
    //}

    //private void Build()
    //{
    //    if (isPreviewActive && previewPrefab.GetComponent<PreviewObject>().IsBuildable())
    //    {
    //        Instantiate(installPrefab, hitInfo.point, Quaternion.identity);
    //        Destroy(previewPrefab);
    //        isActive = false;
    //        isPreviewActive = false;
    //        previewPrefab = null;
    //        installPrefab = null;
    //    }

    //}

}
