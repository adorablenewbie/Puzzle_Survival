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
    public int questState; // 0: ����, 1: �Ϸ�, 2: ����

    // ���ο� ���� ���� �÷�
    public int rewardRandom;          // ���� ���� ����
    public int[] rewardCategories;    // ���� ī�װ� ��ȣ �迭
    public int[] rewardCounts;        // �� ī�װ��� ���� ����

    public bool shake;
    public int animation;
    public bool zoom;
    public float typingSpeed;
    public int fontSize;
    public Color fontColor;
    public string sceneName;
}
