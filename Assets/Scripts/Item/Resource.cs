using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    //�ڿ��� �ؾ� �ϴ� �� 
    // ���� �ڿ��̴� ���� ������ �ڿ��� �ְ� �������� ��.
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity = 3;

    public event Action<Resource> OnDepleted;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log(capacity);

        if (capacity <= 0) return;
        capacity --;
        Debug.Log(itemToGive.dropPrefab);
        Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        

        if (capacity <= 0)
        {
            Debug.Log("�ڿ��� �� ���������ϴ�. ���ҽ��� ������ϴ�.");
            OnDepleted?.Invoke(this);
            Destroy(gameObject);
        }
    }

}