using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition thirst;
    public GameObject diePanel;
    public Image damageImage;
    public Image diePanelImage;
    private void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
    }
}

