using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public int branch;         // 분기
    public int index;          // 인덱스
    public string npcName;     // NPC 이름
    public string text;        // 대사
    public string[] actions;   // 선택지 ("/" 기준 분리)
    public int[] nextBranches; // 선택지에 따른 분기 이동
    public int[] nextIndex;      // 선택지에 따른 인덱스 이동
    public int indextransition; // 인덱스 전환 (1: 다음 대사)
    public int quest;           // 퀘스트 ID (0: 시작 전 1: 진행 중 2: 완료)
    public int[] questNextBranches; // 퀘스트 완료 후 다음 분기 이동
    public int questIndex;      // 퀘스트 완료 후 이동 가능 인덱스
    public bool shake;         // 화면 진동
    public bool animation;     // 애니메이션 실행 여부
    public bool zoom;          // 카메라 확대
    public float typingSpeed;  // 타이핑 속도
    public int fontSize;       // 폰트 크기
    public Color fontColor;    // 폰트 색상 (RGBA)
}
