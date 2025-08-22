using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestData> activeQuests = new List<QuestData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadInitialQuests();
    }

    public void AcceptQuest(QuestData quest)
    {
        if (!activeQuests.Contains(quest))
        {
            quest.status = QuestStatus.InProgress;
            quest.currentAmount = 0;
            activeQuests.Add(quest);
            Debug.Log($"����Ʈ ����: {quest.questName}");
        }
    }

    public void CompleteQuest(QuestData quest)
    {
        if (quest.status == QuestStatus.CanComplete)
        {
            quest.status = QuestStatus.Completed;
            activeQuests.Remove(quest);
            Debug.Log($"����Ʈ �Ϸ�: {quest.questName}");

            ApplyDialogueChanges(quest.successChanges);
        }
    }

    public void FailQuest(QuestData quest)
    {
        if (quest.status != QuestStatus.Completed)
        {
            quest.status = QuestStatus.Failed;
            activeQuests.Remove(quest);
            Debug.Log($"����Ʈ ����: {quest.questName}");

            ApplyDialogueChanges(quest.failChanges);
        }
    }

    public bool IsQuestCompleted(string questID)
    {
        return activeQuests.Exists(q => q.questID == questID && q.status == QuestStatus.Completed);
    }

    /// <summary>
    /// �������� ����Ʈ�� ���¸� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public QuestStatus GetQuestStatus(string questID)
    {
        var quest = activeQuests.Find(q => q.questID == questID);
        return quest != null ? quest.status : QuestStatus.NotStarted;
    }

    public void ApplyDialogueChanges(List<NpcDialogueChange> changes)
    {
        foreach (var change in changes)
        {
            if (NPCDialogueDataManager.Instance.npcDialogueStates.ContainsKey(change.npc))
            {
                NPCDialogueDataManager.Instance.npcDialogueStates[change.npc] = (change.newBranch, change.newIndex);
            }
            else
            {
                NPCDialogueDataManager.Instance.npcDialogueStates.Add(change.npc, (change.newBranch, change.newIndex));
            }

            Debug.Log($"{change.npc} ��ȭ ���� ���� �� Branch:{change.newBranch}, Index:{change.newIndex}");
        }

        NPCDialogueDataManager.Instance.SaveNpcStates(NPCDialogueDataManager.Instance.npcDialogueStates); // JSON ����
    }

    //==================================================����Ʈ ���� üũ. �ٵ� ���� ����Ʈ�� �̱���=======================================================================
    public void UpdateQuestProgressMonster(string targetID, int amount = 1) // Ư�� ��ǥ ���͸� ���� �� ����Ʈ ���� ������Ʈ
    {
        foreach (var quest in activeQuests)
        {
            if (quest.targetID == targetID && quest.status == QuestStatus.InProgress)
            {
                quest.currentAmount += amount;
                if (quest.currentAmount >= quest.requiredAmount)
                {
                    quest.status = QuestStatus.CanComplete;
                    Debug.Log($"����Ʈ �Ϸ� ����: {quest.questName}");
                }
            }
        }
    }
    public void UpdateQuestProgressNPC(ENPC targetNpc, int branch, int index) // Ư�� �ι��� Ư�� ��ȭ�� ���� �� ����Ʈ ���� ������Ʈ
    {
        foreach (var quest in activeQuests)
        {
            if (quest.targetNpc == targetNpc && quest.branch == branch && quest.index == index && quest.status == QuestStatus.InProgress)
            {
                quest.currentAmount++;
                if (quest.currentAmount >= quest.requiredAmount)
                {
                    quest.status = QuestStatus.CanComplete;
                    Debug.Log($"����Ʈ �Ϸ� ����: {quest.questName}");
                }
            }
        }
    }

    private void LoadInitialQuests()
    {
        int i = 0;
        while (true)
        {
            QuestData quest = Resources.Load<QuestData>("QuestData/" + "Quest" + i);
            if (quest == null)
            {
                Debug.Log("�ʱ� ����Ʈ �ε� �Ϸ�");
                break;
            }
            if(quest.status == QuestStatus.InProgress || quest.status == QuestStatus.CanComplete)
            {
                activeQuests.Add(quest);
            }
            i++;
        }
        Debug.Log("�ʱ� ����Ʈ �ε� �Ϸ�");
    }
}
