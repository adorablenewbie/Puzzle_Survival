using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    // 자원이 해야 하는 일 
    // 나는 자원이야 알맞은 위치에 있다가 나를 때리면 자원을 주고 없어지면 돼.
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

            Debug.Log("알맞은 위치를 찾는 중이야.");
            OnDepleted?.Invoke(this);
            Destroy(gameObject);
        }
    }

    //방법을 다시 고민 해 봐야 할 듯
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
            Debug.Log("자원이 다 떨어졌어 사라질께");
            OnDepleted?.Invoke(this);
            Destroy(gameObject);
        }
    }

}