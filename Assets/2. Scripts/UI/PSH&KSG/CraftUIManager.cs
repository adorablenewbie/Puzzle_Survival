using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftUIManager : MonoBehaviour
{
    /*
     ó�� ��ȭ��
    craft �� zŰ�� ����.
    ���� - ���� �� ��ư Ŭ������ �� �ǵ��� ����.
    ���۸���Ʈ �ѱ� -> ���� ����
    ���ฮ��Ʈ �ѱ� -> ���� ����

    =============================================
    ���� ����Ʈ���� ����� ��ư
    -> ������ ���� true �� ���� -> craft, build��� ����.
    -> ��ҽÿ� �ٽ� craft, build��� Ų��.
    -> 
     
     
     */



    [SerializeField] private UICraft craft;
    [SerializeField] private UIBuild build;
    [SerializeField] private UIProduction production;

    private bool isCraftActive = false;

    private void Awake()
    {
        // ��ư �̺�Ʈ �����ϱ�
        craft.OnBuildClick += build.OpenBuildList;
        craft.OnBuildClick += production.CloseProductionList;
        craft.OnProductionClick += production.OpenProductionList;
        craft.OnProductionClick += build.CloseBuildList;

        build.PreviewStart += PreviewBuildHandle;
        build.PreviewStart += craft.CloseUI;
        build.PreviewEnd += CloseCraftUI;
    }

    public void CraftUIControl(InputAction.CallbackContext context) // Ű �Է��� ���� CraftUI ���� �ݱ�
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isCraftActive) OpenCraftUI();
            else CloseCraftUI();
        }
    }

    private void OpenCraftUI() // CraftUI ����
    {
        isCraftActive = true;
        craft.OpenUI();
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseCraftUI() // CraftUI �ݱ�
    {
        isCraftActive = false;
        craft.CloseUI();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PreviewBuildHandle()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
