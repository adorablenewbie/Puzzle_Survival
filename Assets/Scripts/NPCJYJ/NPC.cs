using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC ��ȭ CSV")]
    public TextAsset csvDialogueFile; // CSV ������ ������ ����

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
