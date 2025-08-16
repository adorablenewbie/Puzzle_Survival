using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC 대화 CSV")]
    public TextAsset csvDialogueFile; // CSV 파일을 연결할 변수
    public string npcName; // NPC 이름
    public string npcDescription; // NPC 설명
    public Camera npcCamera; // NPC 대화용 카메라
    public Animator npcAnimator; // NPC 애니메이터 (필요시 사용)

    private DialogueManager dialogueManager; // DialogueManager 참조

    private void Start()
    {
        dialogueManager = PlayerManager.Instance.Player.dialogueManager;
        npcCamera.gameObject.SetActive(false); // NPC 대화용 카메라 비활성화
    }

    public string GetInteractPrompt()
    {
        string str = $"{npcDescription}\n{npcName}";
        return str;
    }

    public void OnInteract()
    {
        dialogueManager.gameObject.SetActive(true); // 대화 UI 활성화
        dialogueManager.csvFile = csvDialogueFile; // DialogueManager에 CSV 파일 설정Z
        dialogueManager.LoadCSV(); // CSV 파일 로드
        npcCamera.gameObject.SetActive(true); // NPC 대화용 카메라 활성화
        dialogueManager.npcCameara = npcCamera; // NPC 대화용 카메라 설정
        dialogueManager.npcAnimator = npcAnimator; // NPC 애니메이터 설정
    }

}
