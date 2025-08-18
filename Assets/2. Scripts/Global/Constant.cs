using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public static class AnimationParameter
    {
        public static readonly int Moving = Animator.StringToHash("Moving");
        public static readonly int Attack = Animator.StringToHash("Attack");
    }
}
