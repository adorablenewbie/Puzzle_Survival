using System.Collections.Generic;  
using UnityEngine;                
using TMPro;                       
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextAsset csvFile;          // Unity�� ������ CSV ����
    public TextAsset currCsv;
    public TMP_Text dialogueText;      // ��� ����� UI �ؽ�Ʈ
    public TMP_Text nameText;          // NPC �̸� ����� UI �ؽ�Ʈ
    public Transform choiceContainer;  // ������ ��ư���� ���� �θ� ������Ʈ
    public Button choiceButtonPrefab;  // ������ ��ư ������

    public Camera npcCameara; // NPC ��ȭ�� ī�޶�

    public bool isDialogue = false;        // ���� ��ȭ ������ ����
    public bool isLastLine = false;         // ���� ��ȭ�� ������ �������� ����
    public bool isChoice = false;          // ���� ������ ��ȭ���� ����

    private List<DialogueLine> dialogueData = new List<DialogueLine>(); // CSV���� ���� ��ü ��ȭ ������ ����
    public int currentBranch = 1;     // ���� �б� ��ȣ
    public int currentIndex = 1;      // ���� �ε��� ��ȣ (�� ��ȭ ���� �� ����)
    private List<DialogueLine> currentLines = new List<DialogueLine>();
    private int currentLineIndex = 0;

    void Start()
    {
        gameObject.SetActive(false); // �ʱ⿡�� ��ȭ UI ��Ȱ��ȭ
    }

    public void LoadCSV()
    {
        if(csvFile == currCsv) return; // �̹� �ε�� CSV �����̸� �ߺ� �ε� ����
        currCsv = csvFile; // ���� CSV ���Ϸ� ����

        string[] lines = csvFile.text.Split('\n'); // CSV ������ �� ������ �и�

        for (int i = 1; i < lines.Length; i++) // ù ���� ����� i=1���� ����
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // �� ���� �ǳʶ�

            string[] cols = lines[i].Split(','); // �� ���� �޸��� �и��ؼ� �÷� �迭�� ����

            // ��Ʈ ũ�� ó�� (������ 50)
            int fontSize = 50;
            if (cols.Length > 15 && !string.IsNullOrWhiteSpace(cols[15]))
                int.TryParse(cols[15], out fontSize);

            // ��Ʈ ���� ó�� (������ ��� 255/255/255/255)
            Color fontColor = new Color32(255, 255, 255, 255);
            if (cols.Length > 16 && !string.IsNullOrWhiteSpace(cols[16]))
            {
                string[] rgba = cols[16].Split('/');
                if (rgba.Length == 4)
                {
                    byte r = byte.Parse(rgba[0]);
                    byte g = byte.Parse(rgba[1]);
                    byte b = byte.Parse(rgba[2]);
                    byte a = byte.Parse(rgba[3]);
                    fontColor = new Color32(r, g, b, a);
                }
            }

            DialogueLine line = new DialogueLine // CSV �� ���� DialogueLine ��ü�� ��ȯ
            {
                branch = SafeParseInt(cols[0]),                                   // �б� ��ȣ
                index = SafeParseInt(cols[1]),                               // �ε��� ��ȣ
                npcName = cols.Length > 2 ? cols[2] : "",                      // NPC �̸�
                text = cols.Length > 3 ? cols[3] : "",                            // ���
                actions = string.IsNullOrWhiteSpace(cols[4]) ? null : cols[4].Split('/'), // ������ (������ null)
                nextBranches = string.IsNullOrWhiteSpace(cols[5]) ? null : System.Array.ConvertAll(cols[5].Split('/'), int.Parse), // �������� ���� �б�
                nextIndex = string.IsNullOrWhiteSpace(cols[6]) ? null : System.Array.ConvertAll(cols[6].Split('/'), int.Parse), // �������� ���� �ε���
                indextransition = SafeParseInt(cols.Length > 7 ? cols[7] : "0"),       // �ε��� ��ȯ (1: ���� ���)
                quest = SafeParseInt(cols.Length > 8 ? cols[8] : "0"),                 // ����Ʈ ID (1: ����)
                questNextBranches = string.IsNullOrWhiteSpace(cols[9]) ? null : System.Array.ConvertAll(cols[9].Split('/'), int.Parse), // ����Ʈ �Ϸ� �� ���� �б�
                questIndex = SafeParseInt(cols.Length > 10 ? cols[10] : "0"),          // ����Ʈ �Ϸ� �� �̵� ���� �ε���
                shake = cols[11] == "1",                                        // ȭ�� ���� ����
                animation = cols[12] == "1",                                    // �ִϸ��̼� ���� ����
                zoom = cols[13] == "1",                                         // ī�޶� Ȯ�� ����
                typingSpeed = float.Parse(cols[14]),                            // Ÿ���� �ӵ�
                fontSize = fontSize,                                           // ��Ʈ ũ��
                fontColor = fontColor                                          // ��Ʈ ����
            };

            dialogueData.Add(line); // �ϼ��� DialogueLine�� ����Ʈ�� �߰�
        }
    }
    int SafeParseInt(string s, int defaultValue = 0)
    {
        int result;
        if (int.TryParse(s.Trim(), out result)) // Trim()���� ����/���� ����
            return result;
        return defaultValue;
    }

    public void ShowDialogue(int branch, int index)
    {
        // ���� �б�� �ε����� �ش��ϴ� ��� ��� ���� ã��
        currentLines = dialogueData.FindAll(d => d.branch == branch && d.index == index);
        currentLineIndex = 0;

        // ������ ������ ������ ��ư�� ����
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (currentLineIndex < currentLines.Count)
        {
            DialogueLine line = currentLines[currentLineIndex];
            nameText.text = line.npcName;
            dialogueText.text = line.text;
            dialogueText.fontSize = line.fontSize;
            dialogueText.color = line.fontColor;

            // �������� �ִٸ� ��ư ����
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
            if (line.actions != null && line.actions.Length > 0)
            {
                isChoice = true; // ������ ��ȭ���� ǥ��
                for (int i = 0; i < line.actions.Length; i++)
                {
                    int nextBranch = line.nextBranches[i];
                    Button btn = Instantiate(choiceButtonPrefab, choiceContainer);
                    btn.GetComponentInChildren<TMP_Text>().text = line.actions[i];
                    btn.onClick.AddListener(() => OnChoiceSelected(nextBranch));
                }
            }
            Debug.Log($"Showing dialogue: Branch {line.branch}, Index {line.index}, Text: {line.text}");
            currentLineIndex++;
        }
        if (currentLineIndex == currentLines.Count)
        {
            isLastLine = true;
            if (currentLines[currentLines.Count-1].indextransition == 1)
            {
                currentIndex++; // �ε��� ��ȯ�� �ʿ��ϸ� ���� �ε����� �̵�
            }
        }
        // ��� ��� ��� �� �߰� ó�� �ʿ�� ���⿡ �ۼ�
    }

    public void OnChoiceSelected(int nextBranch)
    {
        currentBranch = nextBranch;  // ���� �б�� ����
        currentIndex = 1;            // ���ο� �б� ���� �� �ε��� �ʱ�ȭ
        ShowDialogue(currentBranch, currentIndex); // ���ο� �б� ��� ���
    }
}
