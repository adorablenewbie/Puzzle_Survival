using UnityEngine;

public class Resource : MonoBehaviour
{
    //�ڿ��� �ؾ� �ϴ� �� 
    // ���� �ڿ��̴� ���� ������ �ڿ��� �ְ� �������� ��.
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;
    public GameObject prefabreference;
    public float respawnDelay = 150f;
    public int initialCapacity;

    public ResorceSpawnManager resourceSpawnManager;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (capacity <= 0) return;
        capacity --;
        Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        

        if (capacity <= 0)
        {
            resourceSpawnManager.RequestRespawn(prefabreference, respawnDelay);
            Destroy(gameObject);
        }
    }
 
}