using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuff : MonoBehaviour
{
    public float remainDuration;
    public float duration;
    public bool isPermanent;
    public Image uiImage;

    private void Update()
    {
        if (isPermanent) return;
        remainDuration -= Time.deltaTime;
        uiImage.fillAmount = GetPercentage();
        if (remainDuration <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetPercentage()
    {
        return 1- (remainDuration / duration);
    }
}
