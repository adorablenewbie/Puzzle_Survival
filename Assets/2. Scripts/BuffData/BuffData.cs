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
public enum DebuffType
{
    SlowDown,
    cold,
    Poison
}
[System.Serializable]
public class BuffEffect
{
    public BuffType type; // 버프 타입
    public DebuffType debuffType; // 디버프 타입 (추가적인 디버프 효과를 위해)
    public float value; // 버프 효과 값
    public bool isOn; // 더블점프 같은 경우는 켜져있는지 여부
}

[CreateAssetMenu(fileName = "Buff", menuName = "New Buff Data")]
public class BuffData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject buffUI;
    public BuffEffect[] effects; // 버프 효과들
}