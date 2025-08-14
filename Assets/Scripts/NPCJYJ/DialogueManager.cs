using System.Collections.Generic;  // List ����� ���� �ʿ�
using UnityEngine;                 // Unity ���� �⺻ ���ӽ����̽�
using TMPro;                       // TextMeshPro ���
using UnityEngine.UI;              // UI ��ư ���

public class DialogueManager : MonoBehaviour
{
    public TextAsset csvFile;          // Unity�� ������ CSV ����
    public TMP_Text dialogueText;      // ��� ����� UI �ؽ�Ʈ
    public TMP_Text nameText;          // NPC �̸� ����� UI �ؽ�Ʈ (���� �߰�)
    public Transform choiceContainer;  // ������ ��ư���� ���� �θ� ������Ʈ
    public Button choiceButtonPrefab;  // ������ ��ư ������

    private List<DialogueLine> dialogueData = new List<DialogueLine>(); // CSV���� ���� ��ü ��ȭ ������ ����
    private int currentBranch = 1;     // ���� �б� ��ȣ
    private int currentIndex = 1;      // ���� �ε��� ��ȣ (�� ��ȭ ���� �� ����)

    void Start()
    {
        LoadCSV();                           // ���� �� CSV ���� �б�
        ShowDialogue(currentBranch, currentIndex); // ó�� ��ȭ �����ֱ�
    }

    void LoadCSV()
    {
        string[] lines = csvFile.text.Split('\n'); // CSV ������ �� ������ �и�

        for (int i = 1; i < lines.Length; i++) // ù ���� ����� i=1���� ����
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // �� ���� �ǳʶ�

            string[] cols = lines[i].Split(','); // �� ���� �޸��� �и��ؼ� �÷� �迭�� ����

            DialogueLine line = new DialogueLine // CSV �� ���� DialogueLine ��ü�� ��ȯ
            {
                branch = int.Parse(cols[0]),                                   // �б� ��ȣ
                index = int.Parse(cols[1]),                                    // �ε��� ��ȣ
                npcName = cols[2],                                             // NPC �̸� (���� �߰�)
                text = cols[3],                                                // ���
                actions = string.IsNullOrWhiteSpace(cols[4]) ? null : cols[4].Split('/'), // ������ (������ null)
                nextBranches = string.IsNullOrWhiteSpace(cols[5]) ? null : System.Array.ConvertAll(cols[5].Split('/'), int.Parse), // �������� ���� �б�
                shake = cols[6] == "1",                                        // ȭ�� ���� ����
                animation = cols[7] == "1",                                    // �ִϸ��̼� ���� ����
                zoom = cols[8] == "1",                                         // ī�޶� Ȯ�� ����
                typingSpeed = float.Parse(cols[9])                             // Ÿ���� �ӵ�
            };

            dialogueData.Add(line); // �ϼ��� DialogueLine�� ����Ʈ�� �߰�
        }
    }

    void ShowDialogue(int branch, int index)
    {
        // ���� �б�� �ε����� �ش��ϴ� ��� ��� ���� ã��
        List<DialogueLine> lines = dialogueData.FindAll(d => d.branch == branch && d.index == index);

        // ������ ������ ������ ��ư�� ����
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // ã�� ������ �ϳ��� UI�� ���
        foreach (DialogueLine line in lines)
        {
            nameText.text = line.npcName; // NPC �̸� ���
            dialogueText.text = line.text; // NPC ��� ���

            // �������� �ִٸ� ��ư ����
            if (line.actions != null && line.actions.Length > 0)
            {
                for (int i = 0; i < line.actions.Length; i++) // ������ ������ŭ ��ư ����
                {
                    int nextBranch = line.nextBranches[i]; // ���� �������� �ش��ϴ� ���� �б� ��ȣ
                    Button btn = Instantiate(choiceButtonPrefab, choiceContainer); // ��ư ������ ���� �� �θ� �ؿ� ��ġ
                    btn.GetComponentInChildren<TMP_Text>().text = line.actions[i]; // ��ư �ؽ�Ʈ�� ������ ���� ǥ��
                    btn.onClick.AddListener(() => OnChoiceSelected(nextBranch));   // ��ư Ŭ�� �� �ش� �б�� �̵��ϵ��� �̺�Ʈ ����
                }
            }
        }
    }

    void OnChoiceSelected(int nextBranch)
    {
        currentBranch = nextBranch;  // ���� �б�� ����
        currentIndex = 1;            // ���ο� �б� ���� �� �ε��� �ʱ�ȭ
        ShowDialogue(currentBranch, currentIndex); // ���ο� �б� ��� ���
    }
}
