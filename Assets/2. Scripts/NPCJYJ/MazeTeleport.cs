using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeTeleport : MonoBehaviour
{
    public GameObject mazeObject;

    private NPC npc; // NPC 참조
    private ENPC npcKey; // NPC 키

    private DialogueManager dialogueManager; // DialogueManager 참조

    public int branch;
    public int index;

    public Vector3[] posList; // 미로 위치 목록

    private bool isReady = false; // 초기화 완료 여부

    private void Start()
    {
        if (npc == null)
        {
            npc = GetComponent<NPC>();
            npcKey = npc.npcKey; // NPC 키 설정
        }
        if (dialogueManager == null)
        {
            dialogueManager = PlayerManager.Instance.Player.dialogueManager;
        }
    }

    private void Update()
    {
        if (dialogueManager.npcDialogueStates == null || !dialogueManager.npcDialogueStates.ContainsKey(npcKey))
        {
            return; // 대화 상태가 없으면 아무 작업도 하지 않음
        }

        if (dialogueManager.npcDialogueStates[npcKey].branch == branch && dialogueManager.npcDialogueStates[npcKey].index == index)
        {
            isReady = true; // 대화 상태가 일치하면 초기화 완료
        }
        if (!dialogueManager.gameObject.activeSelf && isReady)
        {
            TeleportToMaze(); // 대화가 활성화되면 미로로 이동
            GetComponent<MazeTeleport>().enabled = false; // 이동 후 스크립트 비활성화

        }
    }
    public void TeleportToMaze()
    {
        int randomIndex = Random.Range(0, posList.Length); // 위치 목록에서 랜덤 인덱스 선택
        gameObject.transform.position = posList[randomIndex]; // 랜덤 위치로 이동
        gameObject.transform.rotation = Quaternion.identity; // 회전 초기화
    }

}
