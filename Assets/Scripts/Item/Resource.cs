using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    // �ڿ��� �ؾ� �ϴ� �� 
    // ���� �ڿ��̾� �˸��� ��ġ�� �ִٰ� ���� ������ �ڿ��� �ְ� �������� ��.
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity = 3;
    private float CheckposY = -5f;

    public event Action<Resource> OnDepleted;
    private void Start()
    {
        //gameObject.SetActive(false);
    }
    private void Update()
    {
        PositionCheck();
    }

    private void PositionCheck()
    {

        if (transform.position.y <= CheckposY)
        {

            Debug.Log("�˸��� ��ġ�� ã�� ���̾�.");
            OnDepleted?.Invoke(this);
            Destroy(gameObject);
        }
    }

    //����� �ٽ� ��� �� ���� �� ��
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Debug.Log("Resource collided with ground, destroying.");
    //        gameObject.SetActive(true);
    //    }
        
    //}
    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log(capacity);

        if (capacity <= 0) return;
        capacity--;
        Debug.Log(itemToGive.dropPrefab);
        Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));

        if (capacity <= 0)
        {
            Debug.Log("�ڿ��� �� �������� �������");
            OnDepleted?.Invoke(this);
            Destroy(gameObject);
        }
    }

}