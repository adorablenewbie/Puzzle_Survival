using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC ��ȭ CSV")]
    public TextAsset csvDialogueFile; // CSV ������ ������ ����
    public string npcName; // NPC �̸�
    public string npcDescription; // NPC ����
    public Camera npcCamera; // NPC ��ȭ�� ī�޶�
    public Animator npcAnimator; // NPC �ִϸ����� (�ʿ�� ���)

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
        dialogueManager.npcCameara = npcCamera; // NPC ��ȭ�� ī�޶� ����
        dialogueManager.npcAnimator = npcAnimator; // NPC �ִϸ����� ����
    }

}
