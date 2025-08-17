using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public UIBuffManager uiBuffManager;
    public DialogueManager dialogueManager;
    public PlayerInput playerInput;

    public ItemData itemData;
    public Action addItem;

    public Equipment equip;

    public Transform dropPosition;

    private void Awake()
    {
        // test
        Debug.Log(Camera.main.targetDisplay);
        //Debug.Log(SubCamera.targetDisplay);

        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
        playerInput = GetComponent<PlayerInput>();
    }
}