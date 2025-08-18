using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProduction : MonoBehaviour
{
    public ItemSlot[] itemslots;
    public GameObject ProductionSoupButton;
    public GameObject ProductionPotionButton;
    public GameObject ProductionAxeButton;
    public GameObject ProductionSwordButton;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"UIProduction {itemslots.Length}");
    }

    public void OpenProductionList()
    {
        gameObject.SetActive(true);
    }

    public void CloseProductionList()
    {
        gameObject.SetActive(false);
    }
}
