using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENPC
{
    Amy,
    Knight
}

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC ��ȭ CSV")]
    public TextAsset csvDialogueFile; // CSV ������ ������ ����
    public string npcName; // NPC �̸�
    public string npcDescription; // NPC ����
    public Camera npcCamera; // NPC ��ȭ�� ī�޶�
    public Vector3 npcCameraOriginPosition; // NPC ��ȭ�� ī�޶� ��ġ
    public Vector3 npcCameraZoomPosition; // NPC ��ȭ�� ī�޶� Ȯ�� ��ġ
    public Animator npcAnimator; // NPC �ִϸ����� (�ʿ�� ���)
    public GameObject[] npcRewards; // NPC ���� ������Ʈ (�ʿ�� ���)
    public ENPC npcKey;



    private DialogueManager dialogueManager; // DialogueManager ����

    private void Start()
    {
        dialogueManager = PlayerManager.Instance.Player.dialogueManager;
        npcCamera.gameObject.SetActive(false); // NPC ��ȭ�� ī�޶� ��Ȱ��ȭ
    }

    public string GetInteractPrompt()
    {
        string str = $"{npcDescription}\n{npcName}";
        return str;
    }

    public void OnInteract()
    {
        dialogueManager.gameObject.SetActive(true); // ��ȭ UI Ȱ��ȭ
        dialogueManager.csvFile = csvDialogueFile; // DialogueManager�� CSV ���� ����Z
        dialogueManager.LoadCSV(); // CSV ���� �ε�
        npcCamera.gameObject.SetActive(true); // NPC ��ȭ�� ī�޶� Ȱ��ȭ
        dialogueManager.npcCamera = npcCamera; // NPC ��ȭ�� ī�޶� ����
        dialogueManager.npcAnimator = npcAnimator; // NPC �ִϸ����� ����
        dialogueManager.npcCameraOriginPosition = npcCameraOriginPosition; // NPC ī�޶� ���� ��ġ ����
        dialogueManager.npcCameraZoomPosition = npcCameraZoomPosition; // NPC ī�޶� Ȯ�� ��ġ ����
        dialogueManager.npcDialogueStateKey = npcKey; // NPC ��ȭ ���� Ű ����
        dialogueManager.npc = this; // NPC ����                                                           �׳� npc.~~~ �� �� �����͸� �ع�����
    }

}
