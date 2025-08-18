using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeTeleport : MonoBehaviour
{
    public GameObject mazeObject;
    private MazeGenerator mazeGenerator; // MazeGenerator 참조

    private NPC npc; // NPC 참조
    private ENPC npcKey; // NPC 키

    private DialogueManager dialogueManager; // DialogueManager 참조

    public int branch;
    public int index;

    private bool isReady = false; // 초기화 완료 여부

    private void Start()
    {
        if (mazeGenerator == null)
        {
            mazeGenerator = mazeObject.GetComponent<MazeGenerator>();
        }
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
        if (mazeGenerator != null)
        {
            // MazeGenerator의 exitPosition으로 플레이어를 이동
            gameObject.transform.position = mazeGenerator.exitPosition;
            gameObject.transform.rotation = Quaternion.identity; // 회전 초기화
        }
    }

}
