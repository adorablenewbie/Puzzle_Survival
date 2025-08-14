[System.Serializable]
public class DialogueLine
{
    public int branch;         // 분기
    public int index;          // 인덱스
    public string npcName;     // NPC 이름 (새로 추가됨)
    public string text;        // 대사
    public string[] actions;   // 선택지 ("/" 기준 분리)
    public int[] nextBranches; // 선택지에 따른 분기 이동
    public bool shake;         // 화면 진동
    public bool animation;     // 애니메이션 실행 여부
    public bool zoom;          // 카메라 확대
    public float typingSpeed;  // 타이핑 속도
}
