using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeTeleport : MonoBehaviour
{
    public GameObject mazeObject;

    private NPC npc; // NPC ����
    private ENPC npcKey; // NPC Ű

    private DialogueManager dialogueManager; // DialogueManager ����

    public int branch;
    public int index;

    public Vector3[] posList; // �̷� ��ġ ���

    private bool isReady = false; // �ʱ�ȭ �Ϸ� ����

    private void Start()
    {
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
        int randomIndex = Random.Range(0, posList.Length); // ��ġ ��Ͽ��� ���� �ε��� ����
        gameObject.transform.position = posList[randomIndex]; // ���� ��ġ�� �̵�
        gameObject.transform.rotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
    }

}
