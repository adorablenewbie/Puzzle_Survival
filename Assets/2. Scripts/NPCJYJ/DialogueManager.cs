using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;                
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public Camera npcCamera; // NPC ��ȭ�� ī�޶�
    public Vector3 npcCameraOriginPosition; // NPC ��ȭ�� ī�޶� ���� ��ġ
    public Vector3 npcCameraZoomPosition; // NPC ��ȭ�� ī�޶� Ȯ�� ��ġ
    private bool isZoom = false; // ī�޶� Ȯ�� ����

    public bool isDialogue = false;        // ���� ��ȭ ������ ����
    public bool isLastLine = false;         // ���� ��ȭ�� ������ �������� ����
    public bool isTyping = false; // ���� Ÿ���� ������ ����
    public Coroutine typingCoroutine; // Ÿ���� �ڷ�ƾ ���� (�ߴ� �� ���)

    public List<DialogueLine> dialogueData = new List<DialogueLine>(); // CSV���� ���� ��ü ��ȭ ������ ����
    public int currentBranch = 1;     // ���� �б� ��ȣ
    public int currentIndex = 1;      // ���� �ε��� ��ȣ (�� ��ȭ ���� �� ����)
    private bool isEqualBranchIndex = false; // ���� �б�� �ε����� ������ ����
    private List<DialogueLine> currentLines = new List<DialogueLine>();
    private int currentLineIndex = 0;

    private Interaction interaction;
    public Animator npcAnimator; // NPC �ִϸ����� (�ʿ�� ���)

    public Dictionary<ENPC, (int branch, int index)> npcDialogueStates = new Dictionary<ENPC, (int, int)>();
    public ENPC npcDialogueStateKey;

    public NPC npc; // NPC ���� (�ʿ�� ���)

    public NPCLoadScene npcLoadScene; // NPC �ε� �� ��ũ��Ʈ ���� (�ʿ�� ���)
    private string sceneName;
    private bool isSceneLoaded = false; // �� �ε� ����

    private PlayerController playerController;

    void Start()
    {
        gameObject.SetActive(false); // �ʱ⿡�� ��ȭ UI ��Ȱ��ȭ
        interaction = PlayerManager.Instance.Player.GetComponent<Interaction>();
        PlayerManager.Instance.Player.dialogueManager = this; // DialogueManager�� PlayerManager�� ����
        playerController = PlayerManager.Instance.Player.controller;
        // ���� ���� �� JSON �ε�
        LoadNpcStates();

    }
    void Update()
    {
        if (isZoom)
        {
            ZoomNpcCamera(); // ī�޶� Ȯ�� ���̸� Ȯ�� �Լ� ȣ��
        }
    }
    public void LoadCSV()
    {
        if (csvFile == currCsv) return; // �̹� �ε�� CSV �����̸� �ߺ� �ε� ����
        currCsv = csvFile; // ���� CSV ���Ϸ� ����

        string[] lines = csvFile.text.Split('\n'); // CSV ������ �� ������ �и�

        dialogueData.Clear();
        for (int i = 1; i < lines.Length; i++) // ù ���� ����� i=1���� ����
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // �� ���� �ǳʶ�

            string[] cols = lines[i].Split(','); // �� ���� �޸��� �и��ؼ� �÷� �迭�� ����

            // ��Ʈ ũ�� ó�� (������ 50)
            int fontSize = 50;
            if (cols.Length > 19 && !string.IsNullOrWhiteSpace(cols[19]))
                int.TryParse(cols[19], out fontSize);

            // ��Ʈ ���� ó�� (������ ��� 255/255/255/255)
            Color fontColor = new Color32(255, 255, 255, 255);
            if (cols.Length > 20 && !string.IsNullOrWhiteSpace(cols[20]))
            {
                string[] rgba = cols[20].Split('/');
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
                branchTransition = SafeParseInt(cols.Length > 7 ? cols[7] : "0"), // �б� ��ȯ
                indextransition = SafeParseInt(cols.Length > 8 ? cols[8] : "0"),       // �ε��� ��ȯ
                quest = SafeParseInt(cols.Length > 9 ? cols[9] : "0"),                 // ����Ʈ ID (1: ����)
                questNextBranches = string.IsNullOrWhiteSpace(cols[10]) ? null : System.Array.ConvertAll(cols[10].Split('/'), int.Parse), // ����Ʈ �Ϸ� �� ���� �б�
                questIndex = SafeParseInt(cols.Length > 11 ? cols[11] : "0"),          // ����Ʈ �Ϸ� �� �̵� ���� �ε���
                rewardRandom = SafeParseInt(cols.Length > 12 ? cols[12] : "0"),
                rewardCategories = (cols.Length > 13 && !string.IsNullOrWhiteSpace(cols[13]))
              ? System.Array.ConvertAll(cols[13].Split('/'), x => SafeParseInt(x)) : null,
                rewardCounts = (cols.Length > 14 && !string.IsNullOrWhiteSpace(cols[14]))
              ? System.Array.ConvertAll(cols[14].Split('/'), x => SafeParseInt(x)) : null,
                shake = cols[15] == "1",                                        // ȭ�� ���� ����
                animation = SafeParseInt(cols[16]),                                    // �ִϸ��̼� ���� ����
                zoom = cols[17] == "1",                                         // ī�޶� Ȯ�� ����
                typingSpeed = float.Parse(cols[18]),                            // Ÿ���� �ӵ�
                fontSize = fontSize,                                           // ��Ʈ ũ��
                fontColor = fontColor,                                          // ��Ʈ ����
                sceneName = cols.Length > 21 ? cols[21] : "" // �� �̸� (�߰��� �÷�)
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

    public void ShowDialogue()
    {
        if(npcDialogueStates.ContainsKey(npcDialogueStateKey))
        {
            // NPC�� ��ȭ ���°� �̹� �����ϸ� �ش� ���·� �ʱ�ȭ
            currentBranch = npcDialogueStates[npcDialogueStateKey].branch;
            currentIndex = npcDialogueStates[npcDialogueStateKey].index;
        }
        else
        {
            // NPC�� ��ȭ ���°� ������ �⺻������ �ʱ�ȭ
            npcDialogueStates[npcDialogueStateKey] = (1, 1);
            currentBranch = 1;
            currentIndex = 1;
        }
        int branch = npcDialogueStates[npcDialogueStateKey].branch; // NPC�� ���� �б�
        int index = npcDialogueStates[npcDialogueStateKey].index; // NPC�� ���� �ε���
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
            if (line.zoom)
            {
                isZoom = true; // ī�޶� Ȯ�� Ȱ��ȭ
            }
            else
            {
                isZoom = false; // ī�޶� Ȯ�� ��Ȱ��ȭ
                npcCamera.transform.localPosition = npcCameraOriginPosition; // ���� ��ġ�� ����
            }
            if (line.shake)
            {
                StartCoroutine(ScreenShake()); // ȭ�� ���� ȿ��
            }
            npcAnimator.SetInteger("actionValue", line.animation); // �ִϸ��̼� ���� ����
            typingCoroutine = StartCoroutine(ShowTypingEffect(line.typingSpeed)); // Ÿ���� ȿ���� ��� ���
            if(line.rewardCategories!=null)
            {
                GiveRewards(line, npc); // ���� ����
            }
            sceneName = line.sceneName.Trim(); // �� �̸� ����
            if (npcLoadScene != null && sceneName != "")
            {
                isSceneLoaded = true; // �� �ε� ���� ����
            }
            

            // �������� �ִٸ� ��ư ����
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
            if (line.actions != null && line.actions.Length > 0)
            {
                playerController.ToggleCursor();
                for (int i = 0; i < line.actions.Length; i++)
                {
                    int nextBranch = line.nextBranches[i];
                    int nextIndex = line.nextIndex[i];
                    Button btn = Instantiate(choiceButtonPrefab, choiceContainer);
                    if (i == 0) eventSystem.SetSelectedGameObject(btn.gameObject); // ù ��° ��ư ����
                    StartCoroutine(BtnInteractableDelay(btn)); // ��ư Ȱ��ȭ ������
                    btn.GetComponentInChildren<TMP_Text>().text = line.actions[i];
                    btn.onClick.AddListener(() => OnChoiceSelected(nextBranch, nextIndex));
                }
            }
            currentLineIndex++;
        }
        if (currentLineIndex == currentLines.Count)
        {
            if (currentLines[currentLineIndex - 1].actions == null)
            {
                isLastLine = true;
            }
            if (currentLines[currentLines.Count - 1].branchTransition != 0)
            {
                currentBranch = currentLines[currentLines.Count - 1].branchTransition; // �б� ��ȯ
            }
            if (currentLines[currentLines.Count - 1].indextransition != 0)
            {
                currentIndex = currentLines[currentLines.Count - 1].indextransition; // �ε��� ��ȯ
            }
        }
        UpdateNpcState(); // NPC ���� ������Ʈ
        // ��� ��� ��� �� �߰� ó�� �ʿ�� ���⿡ �ۼ�
    }

    public void OnChoiceSelected(int nextBranch, int nextIndex)
    {
        playerController.ToggleCursor(); // Ŀ�� ���
        if (currentBranch == nextBranch && currentIndex == nextIndex)
        {
            // ���� �б�� �ε����� ������ ��ȭ ���� ��, �� �����Ѵٴ� ������
            ResetDialogueUI();
            return;
        }
        currentBranch = nextBranch;  // ���� �б�� ����
        currentIndex = nextIndex;            // ���ο� �б� ���� �� �ε��� �ʱ�ȭ
        UpdateNpcState();
        ShowDialogue(); // ���ο� �б� ��� ���
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPosition = npcCamera.transform.position; // ���� ��ġ ����
        float shakeDuration = 0.5f; // ���� ���� �ð�
        float shakeMagnitude = 0.1f; // ���� ����
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            npcCamera.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        npcCamera.transform.position = originalPosition; // ���� ��ġ�� ����

    }

    IEnumerator ShowTypingEffect(float typingSpeed)
    {
        isTyping = true; // Ÿ���� ����
        dialogueText.text = ""; // ó���� �� ���ڿ�
        foreach (char c in fullText)
        {
            if (!isTyping) yield break; // Ÿ���� ���� �� �ڷ�ƾ ����
            dialogueText.text += c;  // �� ���ھ� �߰�
            yield return new WaitForSeconds(0.02f / typingSpeed); // ��� �ӵ� ����
        }
        isTyping = false; // Ÿ���� �Ϸ�
    }
    IEnumerator BtnInteractableDelay(Button btn)
    {
        yield return new WaitForSeconds(0.5f); // ������ �ð� ���
        btn.interactable = true; // ��ư Ȱ��ȭ
    }
    void ZoomNpcCamera()
    {
        Vector3 currentPos = npcCamera.transform.localPosition;
        Vector3 targetPos = npcCameraZoomPosition;
        npcCamera.transform.localPosition = Vector3.Lerp(currentPos, npcCameraZoomPosition, 5 * Time.deltaTime);
    }
    public void ResetDialogueUI()
    {
        gameObject.SetActive(false);
        isLastLine = false;
        isDialogue = false;
        npcCamera.gameObject.SetActive(false);
        npcAnimator.SetInteger("actionValue", 0);
        interaction.EnableActions();
        currentLines.Clear();
        currentLineIndex = 0;
        csvFile = null;
        currentBranch = 1;
        currentIndex = 1;
        dialogueText.text = "";
        nameText.text = "";
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Ÿ���� �ڷ�ƾ ����
            typingCoroutine = null;
        }
        isTyping = false;
    }
    private void UpdateNpcState()
    {
        npcDialogueStates[npcDialogueStateKey] = (currentBranch, currentIndex);
        SaveNpcStates(); // ���� ������Ʈ �� ����
    }
    // ���� ���� �Լ�
    public void GiveRewards(DialogueLine line, NPC npc)
    {
        if (line.rewardCategories == null || line.rewardCounts == null) return;

        List<int> selectedCategories = new List<int>();

        if (line.rewardRandom == 0)
        {
            // ��� ī�װ� ����
            selectedCategories.AddRange(line.rewardCategories);
        }
        else
        {
            // �������� rewardRandom�� ����
            List<int> pool = new List<int>(line.rewardCategories);
            for (int i = 0; i < line.rewardRandom && pool.Count > 0; i++)
            {
                int idx = Random.Range(0, pool.Count);
                selectedCategories.Add(pool[idx]);
                pool.RemoveAt(idx);
            }
        }

        // ���� ���� ����
        for (int i = 0; i < line.rewardCategories.Length; i++)
        {
            int category = line.rewardCategories[i];
            int count = line.rewardCounts[i];

            if (selectedCategories.Contains(category))
            {
                if (category >= 0 && category < npc.npcRewards.Length)
                {
                    GameObject rewardPrefab = npc.npcRewards[category];
                    for (int j = 0; j < count; j++)
                    {
                        GameObject.Instantiate(rewardPrefab, npc.transform.position + Vector3.up, Quaternion.identity);
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        if (isSceneLoaded) 
        {
            isSceneLoaded = false; // ��ȭ UI�� ��Ȱ��ȭ�Ǹ� �� �ε� ���� �ʱ�ȭ
            npcLoadScene.LoadScene(sceneName); // NPC �ε� �� ��ũ��Ʈ�� �� �ε�
        }
    }
    [System.Serializable]
    public class NpcState
    {
        public ENPC npc;
        public int branch;
        public int index;
    }

    [System.Serializable]
    public class NpcStateWrapper
    {
        public List<NpcState> states = new List<NpcState>();
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "npcStates.json");

    public void SaveNpcStates()
    {
        NpcStateWrapper wrapper = new NpcStateWrapper();
        foreach (var kvp in npcDialogueStates)
        {
            wrapper.states.Add(new NpcState
            {
                npc = kvp.Key,
                branch = kvp.Value.branch,
                index = kvp.Value.index
            });
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SavePath, json);
    }

    public void LoadNpcStates()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        NpcStateWrapper wrapper = JsonUtility.FromJson<NpcStateWrapper>(json);

        npcDialogueStates.Clear();
        foreach (var state in wrapper.states)
        {
            npcDialogueStates[state.npc] = (state.branch, state.index);
        }
    }

}
