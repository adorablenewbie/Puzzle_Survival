using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeTeleport : MonoBehaviour
{
    public GameObject mazeObject;
    private MazeGenerator mazeGenerator; // MazeGenerator ����

    private NPC npc; // NPC ����
    private ENPC npcKey; // NPC Ű

    private DialogueManager dialogueManager; // DialogueManager ����

    public int branch;
    public int index;

    private bool isReady = false; // �ʱ�ȭ �Ϸ� ����

    private void Start()
    {
        if (mazeGenerator == null)
        {
            mazeGenerator = mazeObject.GetComponent<MazeGenerator>();
        }
        if (npc == null)
        {
            npc = GetComponent<NPC>();
            npcKey = npc.npcKey; // NPC Ű ����
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
            return; // ��ȭ ���°� ������ �ƹ� �۾��� ���� ����
        }

        if (dialogueManager.npcDialogueStates[npcKey].branch == branch && dialogueManager.npcDialogueStates[npcKey].index == index)
        {
            isReady = true; // ��ȭ ���°� ��ġ�ϸ� �ʱ�ȭ �Ϸ�
        }
        if (!dialogueManager.gameObject.activeSelf && isReady)
        {
            TeleportToMaze(); // ��ȭ�� Ȱ��ȭ�Ǹ� �̷η� �̵�
            GetComponent<MazeTeleport>().enabled = false; // �̵� �� ��ũ��Ʈ ��Ȱ��ȭ

        }
    }
    public void TeleportToMaze()
    {
        if (mazeGenerator != null)
        {
            // MazeGenerator�� exitPosition���� �÷��̾ �̵�
            gameObject.transform.position = mazeGenerator.exitPosition;
            gameObject.transform.rotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
        }
    }

}
