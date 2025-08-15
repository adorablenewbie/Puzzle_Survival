using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> resourcePrefab;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    private  int maxItemCount = 10;



    private void Update()
    {
        RequestRespawn(prefab, 5f);
        
    }












    public void RequestRespawn(GameObject prefab, float delay)
    {
        StartCoroutine(RespawnCoroutine(prefab, delay));
    }


    private IEnumerator RespawnCoroutine(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 spawnPos = GetRandomGroundPosition();
    }

    private Vector3 GetRandomGroundPosition()
    {
        // ������ ��ġ�� �����ϰ� �� ��ġ�� �ٴ��� ã�Ƽ� ��ȯ�ϴ� �Լ�
        // spawnAreaMin�� spawnAreaMax ������ ������ x, z ��ǥ�� ����
        if (spawnAreaMin.x >= spawnAreaMax.x || spawnAreaMin.y >= spawnAreaMax.y)
        {
            Debug.LogError("Spawn area(���� ����)�� �ùٸ��� ���ǵ��� �ʾҽ��ϴ�. spawnAreaMin�� spawnAreaMax ���� Ȯ��.");
            return Vector3.zero; // �߸��� ������ �����Ǿ��� �� �⺻�� ��ȯ
        }
        float randomX = UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomZ = UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        Vector3 startPos = new Vector3(randomX, 0f, randomZ);
        if(Physics.Raycast(startPos, Vector3.down, out RaycastHit hit, 50f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }
        return GetRandomGroundPosition(); // ��ã�Ҵٸ� ��������� �ٽ� ã�ƺ��ڱ�
    }
}
