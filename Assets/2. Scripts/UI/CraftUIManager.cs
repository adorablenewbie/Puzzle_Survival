using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
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
    [SerializeField] private TextMeshProUGUI notEnoughText;

    Coroutine coroutine;
    private float duration = 1.0f;

    private bool isCraftActive = false;

    private void Awake()
    {
        notEnoughText.gameObject.SetActive(false);

        // ��ư �̺�Ʈ �����ϱ�
        craft.OnBuildClick += build.OpenBuildList;
        craft.OnBuildClick += production.CloseProductionList;
        craft.OnProductionClick += production.OpenProductionList;
        craft.OnProductionClick += build.CloseBuildList;

        build.PreviewStart += PreviewBuildHandle;
        build.PreviewStart += craft.CloseUI;
        build.PreviewEnd += CloseCraftUI;

        build.WaringEvent += ShowWarning;
        production.WarnigEvent += ShowWarning;
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
        build.CloseBuildList();
        production.CloseProductionList();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PreviewBuildHandle()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void ShowWarning()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(ShowWarningRoutine(duration));
    }

    private IEnumerator ShowWarningRoutine(float duration)
    {
        notEnoughText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        notEnoughText.gameObject.SetActive(false);
        coroutine = null;
    }
}
