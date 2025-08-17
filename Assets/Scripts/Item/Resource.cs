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
    private Rigidbody rb;

    bool raycastHit = true;

    public event Action<Resource> OnDepleted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //gameObject.SetActive(false);
    }
    //private void Update()
    //{
    //    PositionChecki();
    //    if(transform.position.y <= -5f)
    //    {
    //        Destroy(gameObject);
    //        OnDepleted?.Invoke(this);
    //    }
    //}

    //����� �ٽ� ��� �� ���� �� ��
    //private void PositionChecki()
    //{
    //    Ray ray = new Ray(transform.position, Vector3.down);
    //    RaycastHit hit;
    //    int layerMask = LayerMask.GetMask("Ground");
    //    if (raycastHit)
    //    {
    //        if (Physics.Raycast(ray, out hit, 100f, layerMask))
    //        {
    //            Debug.Log("Raycast hit: " + hit.transform.name);

    //            Vector3 pos = transform.position;
    //            pos.y = hit.point.y; // 0.5f �� �ణ�� ������ �ֱ� ����
    //            transform.position = pos;

    //            rb.MovePosition(pos);
    //            raycastHit = false;
    //        }
            
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