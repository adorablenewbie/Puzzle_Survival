[System.Serializable]
public class DialogueLine
{
    public int branch;         // �б�
    public int index;          // �ε���
    public string npcName;     // NPC �̸� (���� �߰���)
    public string text;        // ���
    public string[] actions;   // ������ ("/" ���� �и�)
    public int[] nextBranches; // �������� ���� �б� �̵�
    public bool shake;         // ȭ�� ����
    public bool animation;     // �ִϸ��̼� ���� ����
    public bool zoom;          // ī�޶� Ȯ��
    public float typingSpeed;  // Ÿ���� �ӵ�
}
