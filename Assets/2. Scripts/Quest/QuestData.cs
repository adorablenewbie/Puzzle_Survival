using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public enum QuestStatus
{
    NotStarted,
    InProgress,
    CanComplete,
    Completed,
    Failed
}

[Serializable]
public class NpcDialogueChange
{
    public ENPC npc;
    public int newBranch;
    public int newIndex;
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "New Quest")]
public class QuestData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string questID;
    public string questName;
    [TextArea] public string description;

    [Header("��ǥ")]
    public int requiredAmount;
    public string targetID;
    public int currentAmount;

    [Header("��ǥ NPC ��ȭ")]
    public ENPC targetNpc; // ����Ʈ ��ǥ NPC
    public int branch; // ����Ʈ ��ǥ ��ȭ �б�
    public int index; // ����Ʈ ��ǥ ��ȭ �ε���
    public string targetDialogue; // ����Ʈ ��ǥ ��ȭ ����

    [Header("����")]
    public QuestStatus status = QuestStatus.NotStarted;

    [Header("����� ���� NPC ��ȭ ��ȭ")]
    public List<NpcDialogueChange> successChanges;
    public List<NpcDialogueChange> failChanges;
}
