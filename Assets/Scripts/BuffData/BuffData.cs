using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    SpeedUp,
    JumpUp,
    DoubleJump,
    Invincible
}
[System.Serializable]
public class BuffEffect
{
    public BuffType type; // ���� Ÿ��
    public float value; // ���� ȿ�� ��
    public bool isOn; // �������� ���� ���� �����ִ��� ����
}

[CreateAssetMenu(fileName = "Buff", menuName = "New Buff Data")]
public class BuffData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject buffUI;
    public BuffEffect[] effects; // ���� ȿ����
}
