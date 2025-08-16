using System.Collections.Generic;  
using UnityEngine;                
using TMPro;                       
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextAsset csvFile;          // Unity에 연결할 CSV 파일
    public TextAsset currCsv;
    public TMP_Text dialogueText;      // 대사 출력할 UI 텍스트
    public TMP_Text nameText;          // NPC 이름 출력할 UI 텍스트
    public Transform choiceContainer;  // 선택지 버튼들을 담을 부모 오브젝트
    public Button choiceButtonPrefab;  // 선택지 버튼 프리팹

    public Camera npcCameara; // NPC 대화용 카메라

    public bool isDialogue = false;        // 현재 대화 중인지 여부
    public bool isLastLine = false;         // 현재 대화의 마지막 라인인지 여부
    public bool isChoice = false;          // 현재 선택지 대화인지 여부

    private List<DialogueLine> dialogueData = new List<DialogueLine>(); // CSV에서 읽은 전체 대화 데이터 저장
    public int currentBranch = 1;     // 현재 분기 번호
    public int currentIndex = 1;      // 현재 인덱스 번호 (한 대화 묶음 내 순서)
    private List<DialogueLine> currentLines = new List<DialogueLine>();
    private int currentLineIndex = 0;

    void Start()
    {
        gameObject.SetActive(false); // 초기에는 대화 UI 비활성화
    }

    public void LoadCSV()
    {
        if(csvFile == currCsv) return; // 이미 로드된 CSV 파일이면 중복 로드 방지
        currCsv = csvFile; // 현재 CSV 파일로 설정

        string[] lines = csvFile.text.Split('\n'); // CSV 파일을 줄 단위로 분리

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더라서 i=1부터 시작
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // 빈 줄은 건너뜀

            string[] cols = lines[i].Split(','); // 한 줄을 콤마로 분리해서 컬럼 배열로 저장

            // 폰트 크기 처리 (없으면 50)
            int fontSize = 50;
            if (cols.Length > 15 && !string.IsNullOrWhiteSpace(cols[15]))
                int.TryParse(cols[15], out fontSize);

            // 폰트 색상 처리 (없으면 흰색 255/255/255/255)
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

            DialogueLine line = new DialogueLine // CSV 한 줄을 DialogueLine 객체로 변환
            {
                branch = SafeParseInt(cols[0]),                                   // 분기 번호
                index = SafeParseInt(cols[1]),                               // 인덱스 번호
                npcName = cols.Length > 2 ? cols[2] : "",                      // NPC 이름
                text = cols.Length > 3 ? cols[3] : "",                            // 대사
                actions = string.IsNullOrWhiteSpace(cols[4]) ? null : cols[4].Split('/'), // 선택지 (없으면 null)
                nextBranches = string.IsNullOrWhiteSpace(cols[5]) ? null : System.Array.ConvertAll(cols[5].Split('/'), int.Parse), // 선택지별 다음 분기
                nextIndex = string.IsNullOrWhiteSpace(cols[6]) ? null : System.Array.ConvertAll(cols[6].Split('/'), int.Parse), // 선택지별 다음 인덱스
                indextransition = SafeParseInt(cols.Length > 7 ? cols[7] : "0"),       // 인덱스 전환 (1: 다음 대사)
                quest = SafeParseInt(cols.Length > 8 ? cols[8] : "0"),                 // 퀘스트 ID (1: 있음)
                questNextBranches = string.IsNullOrWhiteSpace(cols[9]) ? null : System.Array.ConvertAll(cols[9].Split('/'), int.Parse), // 퀘스트 완료 후 다음 분기
                questIndex = SafeParseInt(cols.Length > 10 ? cols[10] : "0"),          // 퀘스트 완료 후 이동 가능 인덱스
                shake = cols[11] == "1",                                        // 화면 진동 여부
                animation = cols[12] == "1",                                    // 애니메이션 실행 여부
                zoom = cols[13] == "1",                                         // 카메라 확대 여부
                typingSpeed = float.Parse(cols[14]),                            // 타이핑 속도
                fontSize = fontSize,                                           // 폰트 크기
                fontColor = fontColor                                          // 폰트 색상
            };

            dialogueData.Add(line); // 완성된 DialogueLine을 리스트에 추가
        }
    }
    int SafeParseInt(string s, int defaultValue = 0)
    {
        int result;
        if (int.TryParse(s.Trim(), out result)) // Trim()으로 개행/공백 제거
            return result;
        return defaultValue;
    }

    public void ShowDialogue(int branch, int index)
    {
        // 현재 분기와 인덱스에 해당하는 모든 대사 라인 찾기
        currentLines = dialogueData.FindAll(d => d.branch == branch && d.index == index);
        currentLineIndex = 0;

        // 기존에 생성된 선택지 버튼들 삭제
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

            // 선택지가 있다면 버튼 생성
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
            if (line.actions != null && line.actions.Length > 0)
            {
                isChoice = true; // 선택지 대화임을 표시
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
                currentIndex++; // 인덱스 전환이 필요하면 다음 인덱스로 이동
            }
        }
        // 모든 대사 출력 후 추가 처리 필요시 여기에 작성
    }

    public void OnChoiceSelected(int nextBranch)
    {
        currentBranch = nextBranch;  // 다음 분기로 변경
        currentIndex = 1;            // 새로운 분기 시작 시 인덱스 초기화
        ShowDialogue(currentBranch, currentIndex); // 새로운 분기 대사 출력
    }
}
