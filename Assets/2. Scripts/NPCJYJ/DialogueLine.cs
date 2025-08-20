using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public int branch;
    public int index;
    public string npcName;
    public string text;

    public string[] actions;
    public int[] nextBranches;
    public int[] nextIndex;
    public int branchTransition;
    public int indextransition;
    public string questID;
    public int questState; // 0: 시작, 1: 완료, 2: 실패

    // 새로운 보상 관련 컬럼
    public int rewardRandom;          // 랜덤 보상 개수
    public int[] rewardCategories;    // 보상 카테고리 번호 배열
    public int[] rewardCounts;        // 각 카테고리별 보상 개수

    public bool shake;
    public int animation;
    public bool zoom;
    public float typingSpeed;
    public int fontSize;
    public Color fontColor;
    public string sceneName;
}
