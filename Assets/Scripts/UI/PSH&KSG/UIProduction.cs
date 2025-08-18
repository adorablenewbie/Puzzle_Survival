using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProduction : MonoBehaviour
{
    public ItemSlot[] itemslots;

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
