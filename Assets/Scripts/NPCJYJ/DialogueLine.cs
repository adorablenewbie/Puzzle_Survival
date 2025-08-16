using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public int branch;         // �б�
    public int index;          // �ε���
    public string npcName;     // NPC �̸�
    public string text;        // ���
    public string[] actions;   // ������ ("/" ���� �и�)
    public int[] nextBranches; // �������� ���� �б� �̵�
    public int[] nextIndex;      // �������� ���� �ε��� �̵�
    public int indextransition; // �ε��� ��ȯ (1: ���� ���)
    public int quest;           // ����Ʈ ID (0: ���� �� 1: ���� �� 2: �Ϸ�)
    public int[] questNextBranches; // ����Ʈ �Ϸ� �� ���� �б� �̵�
    public int questIndex;      // ����Ʈ �Ϸ� �� �̵� ���� �ε���
    public bool shake;         // ȭ�� ����
    public bool animation;     // �ִϸ��̼� ���� ����
    public bool zoom;          // ī�޶� Ȯ��
    public float typingSpeed;  // Ÿ���� �ӵ�
    public int fontSize;       // ��Ʈ ũ��
    public Color fontColor;    // ��Ʈ ���� (RGBA)
}
