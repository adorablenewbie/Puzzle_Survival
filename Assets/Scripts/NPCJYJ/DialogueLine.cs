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
    public int indextransition;
    public int quest;
    public int[] questNextBranches;
    public int questIndex;

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
