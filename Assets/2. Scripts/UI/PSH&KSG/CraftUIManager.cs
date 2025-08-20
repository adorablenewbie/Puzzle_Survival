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
     처음 빈화면
    craft 를 z키로 연다.
    제작 - 건축 탭 버튼 클릭으로 각 탭들을 연다.
    제작리스트 켜기 -> 건축 끄기
    건축리스트 켜기 -> 제작 끄기

    =============================================
    건축 리스트에서 만들기 버튼
    -> 프리뷰 상태 true 로 진입 -> craft, build모두 끈다.
    -> 취소시에 다시 craft, build모두 킨다.
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

        // 버튼 이벤트 구독하기
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

    public void CraftUIControl(InputAction.CallbackContext context) // 키 입력을 통해 CraftUI 열고 닫기
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isCraftActive) OpenCraftUI();
            else CloseCraftUI();
        }
    }

    private void OpenCraftUI() // CraftUI 열기
    {
        isCraftActive = true;
        craft.OpenUI();
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseCraftUI() // CraftUI 닫기
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
