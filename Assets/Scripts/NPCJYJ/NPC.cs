using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC 대화 CSV")]
    public TextAsset csvDialogueFile; // CSV 파일을 연결할 변수

    public string GetInteractPrompt()
    {
        //string str = $"{data.displayName}\n{data.description}";
        //return str;
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        throw new System.NotImplementedException();
    }

}
