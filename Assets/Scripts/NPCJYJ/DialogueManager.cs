using System.Collections.Generic;  // List 사용을 위해 필요
using UnityEngine;                 // Unity 엔진 기본 네임스페이스
using TMPro;                       // TextMeshPro 사용
using UnityEngine.UI;              // UI 버튼 사용

public class DialogueManager : MonoBehaviour
{
    public TextAsset csvFile;          // Unity에 연결할 CSV 파일
    public TMP_Text dialogueText;      // 대사 출력할 UI 텍스트
    public TMP_Text nameText;          // NPC 이름 출력할 UI 텍스트 (새로 추가)
    public Transform choiceContainer;  // 선택지 버튼들을 담을 부모 오브젝트
    public Button choiceButtonPrefab;  // 선택지 버튼 프리팹

    private List<DialogueLine> dialogueData = new List<DialogueLine>(); // CSV에서 읽은 전체 대화 데이터 저장
    private int currentBranch = 1;     // 현재 분기 번호
    private int currentIndex = 1;      // 현재 인덱스 번호 (한 대화 묶음 내 순서)

    void Start()
    {
        LoadCSV();                           // 시작 시 CSV 파일 읽기
        ShowDialogue(currentBranch, currentIndex); // 처음 대화 보여주기
    }

    void LoadCSV()
    {
        string[] lines = csvFile.text.Split('\n'); // CSV 파일을 줄 단위로 분리

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더라서 i=1부터 시작
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // 빈 줄은 건너뜀

            string[] cols = lines[i].Split(','); // 한 줄을 콤마로 분리해서 컬럼 배열로 저장

            DialogueLine line = new DialogueLine // CSV 한 줄을 DialogueLine 객체로 변환
            {
                branch = int.Parse(cols[0]),                                   // 분기 번호
                index = int.Parse(cols[1]),                                    // 인덱스 번호
                npcName = cols[2],                                             // NPC 이름 (새로 추가)
                text = cols[3],                                                // 대사
                actions = string.IsNullOrWhiteSpace(cols[4]) ? null : cols[4].Split('/'), // 선택지 (없으면 null)
                nextBranches = string.IsNullOrWhiteSpace(cols[5]) ? null : System.Array.ConvertAll(cols[5].Split('/'), int.Parse), // 선택지별 다음 분기
                shake = cols[6] == "1",                                        // 화면 진동 여부
                animation = cols[7] == "1",                                    // 애니메이션 실행 여부
                zoom = cols[8] == "1",                                         // 카메라 확대 여부
                typingSpeed = float.Parse(cols[9])                             // 타이핑 속도
            };

            dialogueData.Add(line); // 완성된 DialogueLine을 리스트에 추가
        }
    }

    void ShowDialogue(int branch, int index)
    {
        // 현재 분기와 인덱스에 해당하는 모든 대사 라인 찾기
        List<DialogueLine> lines = dialogueData.FindAll(d => d.branch == branch && d.index == index);

        // 기존에 생성된 선택지 버튼들 삭제
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // 찾은 대사들을 하나씩 UI에 출력
        foreach (DialogueLine line in lines)
        {
            nameText.text = line.npcName; // NPC 이름 출력
            dialogueText.text = line.text; // NPC 대사 출력

            // 선택지가 있다면 버튼 생성
            if (line.actions != null && line.actions.Length > 0)
            {
                for (int i = 0; i < line.actions.Length; i++) // 선택지 개수만큼 버튼 생성
                {
                    int nextBranch = line.nextBranches[i]; // 현재 선택지에 해당하는 다음 분기 번호
                    Button btn = Instantiate(choiceButtonPrefab, choiceContainer); // 버튼 프리팹 생성 후 부모 밑에 배치
                    btn.GetComponentInChildren<TMP_Text>().text = line.actions[i]; // 버튼 텍스트에 선택지 내용 표시
                    btn.onClick.AddListener(() => OnChoiceSelected(nextBranch));   // 버튼 클릭 시 해당 분기로 이동하도록 이벤트 연결
                }
            }
        }
    }

    void OnChoiceSelected(int nextBranch)
    {
        currentBranch = nextBranch;  // 다음 분기로 변경
        currentIndex = 1;            // 새로운 분기 시작 시 인덱스 초기화
        ShowDialogue(currentBranch, currentIndex); // 새로운 분기 대사 출력
    }
}
