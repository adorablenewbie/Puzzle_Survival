using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMaze : MonoBehaviour
{
    [Header("UI ���")]
    public TextMeshProUGUI timeText; // �ð� ǥ�� �ؽ�Ʈ

    [Header("�ʱ� �ð�")]
    public float initialTime = 60f; // �ʱ� �ð� ���� (�� ����)

    private float currentTime; // ���� �ð�

    [Header("NPC ����")]
    public ENPC npcKey; // NPC Ű

    [Header("���� �� NPC �б� �ε��� ��ȭ")]
    public int branch; // NPC �б�
    public int index; // NPC �ε���

    private Dictionary<ENPC, (int branch, int index)> npcDictionary; // NPC Ű�� NPC ��ü�� �����ϴ� ��ųʸ�
    private bool isGameSet = false; // ���� ���� ����



    void Start()
    {
        // �ʱ� �ð� ����
        currentTime = initialTime;

        npcDictionary = NPCDialogueDataManager.Instance.LoadNpcStates(); // NPC ��ųʸ� ��������

    }

    private void Update()
    {
        currentTime -= Time.deltaTime; // �� �����Ӹ��� �ð� ����
        timeText.text = "Time: " + currentTime.ToString("F2"); // ���� �ð��� �� ������ ǥ��

        //�¸�����
        if (QuestManager.Instance.GetQuestStatus("0") == QuestStatus.CanComplete)
        {
            Debug.Log("����Ʈ �Ϸ�: MazeQuest");
            gameObject.SetActive(false); // UI ��Ȱ��ȭ
            return; // �� �̻� �ð� ���Ҹ� ���� ����
        }

        if (currentTime <= 0 && !isGameSet)
        {
            currentTime = 0; // �ð��� 0 ���Ϸ� �������� 0���� ����
            isGameSet = true; // ���� ���� �Ϸ�
            OnTimeUp(); // �ð� �ʰ� �� ó��
            gameObject.SetActive(false); // UI ��Ȱ��ȭ
        }
    }

    void OnTimeUp()
    {

        Debug.Log("Time is up!");
        // NPC ��ųʸ����� ���� NPC Ű�� �ش��ϴ� �б�� �ε����� ������
        if (npcDictionary.TryGetValue(npcKey, out var npcState))
        {
            npcDictionary[npcKey] = (branch, index); // NPC ���� ������Ʈ)
            // NPC�� �б�� �ε����� ������Ʈ
            npcState.branch = branch;
            npcState.index = index;
            // NPC ���¸� ����
            NPCDialogueDataManager.Instance.SaveNpcStates(npcDictionary);
            Debug.Log($"NPC {npcKey} ���� ������Ʈ �� Branch: {branch}, Index: {index}");
        }
        else
        {
            Debug.LogWarning("NPC key not found in dictionary: " + npcKey);
        }
    }
}
