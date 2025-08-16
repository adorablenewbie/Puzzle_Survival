using System.Collections.Generic;  
using UnityEngine;                
using TMPro;                       
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public TextAsset csvFile;          // Unity�� ������ CSV ����
    private TextAsset currCsv;
    public TMP_Text dialogueText;      // ��� ����� UI �ؽ�Ʈ
    public string fullText;          // ��ü ��� �ؽ�Ʈ (Ÿ���� ȿ����)
    public TMP_Text nameText;          // NPC �̸� ����� UI �ؽ�Ʈ
    public Transform choiceContainer;  // ������ ��ư���� ���� �θ� ������Ʈ
    public Button choiceButtonPrefab;  // ������ ��ư ������

    public EventSystem eventSystem; // �̺�Ʈ �ý��� (������ ��ư Ŭ�� �� ���)

    public Camera npcCameara; // NPC ��ȭ�� ī�޶�

    public bool isDialogue = false;        // ���� ��ȭ ������ ����
    public bool isLastLine = false;         // ���� ��ȭ�� ������ �������� ����
    public bool isTyping = false; // ���� Ÿ���� ������ ����
    public Coroutine typingCoroutine; // Ÿ���� �ڷ�ƾ ���� (�ߴ� �� ���)

    private List<DialogueLine> dialogueData = new List<DialogueLine>(); // CSV���� ���� ��ü ��ȭ ������ ����
    public int currentBranch = 1;     // ���� �б� ��ȣ
    public int currentIndex = 1;      // ���� �ε��� ��ȣ (�� ��ȭ ���� �� ����)
    private bool isEqualBranchIndex = false; // ���� �б�� �ε����� ������ ����
    private List<DialogueLine> currentLines = new List<DialogueLine>();
    private int currentLineIndex = 0;

    public Animator npcAnimator; // NPC �ִϸ����� (�ʿ�� ���)

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
                animation = SafeParseInt(cols[12]),                                    // �ִϸ��̼� ���� ����
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
            fullText = line.text;
            dialogueText.fontSize = line.fontSize;
            dialogueText.color = line.fontColor;
            if(line.shake)
            {
                StartCoroutine(ScreenShake()); // ȭ�� ���� ȿ��
            }
            npcAnimator.SetInteger("actionValue", line.animation); // �ִϸ��̼� ���� ����
            typingCoroutine = StartCoroutine(ShowText(line.typingSpeed)); // Ÿ���� ȿ���� ��� ���

            // �������� �ִٸ� ��ư ����
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
            if (line.actions != null && line.actions.Length > 0)
            {
                for (int i = 0; i < line.actions.Length; i++)
                {
                    int nextBranch = line.nextBranches[i];
                    int nextIndex = line.nextIndex[i];
                    Button btn = Instantiate(choiceButtonPrefab, choiceContainer);
                    if (i == 0) eventSystem.SetSelectedGameObject(btn.gameObject); // ù ��° ��ư ����
                    btn.interactable = true; // ��ư Ȱ��ȭ
                    btn.GetComponentInChildren<TMP_Text>().text = line.actions[i];
                    btn.onClick.AddListener(() => OnChoiceSelected(nextBranch, nextIndex));
                }
            }
            currentLineIndex++;
        }
        if (currentLineIndex == currentLines.Count)
        {
            if(currentLines[currentLineIndex-1].actions == null) isLastLine = true;
            if (currentLines[currentLines.Count-1].indextransition == 1)
            {
                currentIndex++; // �ε��� ��ȯ�� �ʿ��ϸ� ���� �ε����� �̵�
            }
        }
        // ��� ��� ��� �� �߰� ó�� �ʿ�� ���⿡ �ۼ�
    }

    public void OnChoiceSelected(int nextBranch, int nextIndex)
    {
        if (currentBranch == nextBranch && currentIndex == nextIndex) 
        {
            // ���� �б�� �ε����� ������ ��ȭ ���� ��, �� �����Ѵٴ� ������
            gameObject.SetActive(false);
            isLastLine = false;
            isDialogue = false;
            npcCameara.gameObject.SetActive(false); // NPC ��ȭ�� ī�޶� ��Ȱ��ȭ
            npcAnimator.SetInteger("actionValue", 0); // �ִϸ��̼� �ʱ�ȭ
            return;
        }
        currentBranch = nextBranch;  // ���� �б�� ����
        currentIndex = nextIndex;            // ���ο� �б� ���� �� �ε��� �ʱ�ȭ
        ShowDialogue(currentBranch, currentIndex); // ���ο� �б� ��� ���
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPosition = npcCameara.transform.position; // ���� ��ġ ����
        float shakeDuration = 0.5f; // ���� ���� �ð�
        float shakeMagnitude = 0.1f; // ���� ����
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            npcCameara.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        npcCameara.transform.position = originalPosition; // ���� ��ġ�� ����

    }

    IEnumerator ShowText(float typingSpeed)
    {
        isTyping = true; // Ÿ���� ����
        dialogueText.text = ""; // ó���� �� ���ڿ�
        foreach (char c in fullText)
        {
            dialogueText.text += c;  // �� ���ھ� �߰�
            yield return new WaitForSeconds(0.02f/typingSpeed); // ��� �ӵ� ����
        }
        isTyping = false; // Ÿ���� �Ϸ�
    }
}
