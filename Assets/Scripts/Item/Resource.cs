using Unity.VisualScripting;
using UnityEngine;

public class Resource : MonoBehaviour
{
    //자원이 해야 하는 일 
    // 나는 자원이다 나를 때리면 자원을 주고 없어지면 돼.
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity = 3;
    public GameObject prefabreference;
    public float respawnDelay = 150f;
    public int initialCapacity;
    public ResorceSpawnManager resourceSpawnManager;

    public EquipTool equipTool;

    


    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log(capacity);

        if (capacity <= 0) return;
        capacity --;
        Debug.Log(itemToGive.dropPrefab);
        Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        

        if (capacity <= 0)
        {
            Debug.Log("자원이 다 떨어졌습니다. 리소스가 사라집니다.");
            Destroy(gameObject);
        }
    }

}