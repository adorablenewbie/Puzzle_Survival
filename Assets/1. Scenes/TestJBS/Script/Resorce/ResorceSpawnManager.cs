using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> harvestablePrefab;
    public float respawnDelay; // ������ ������ �ð�.
    // ���� ����Ʈ�� ������ �� �ִ� ����Ʈ
    public List<GameObject> spawnPoint;

    [Header("������ ������")]
    public int maxItemCount;
    public int minItemCount;

    private readonly HashSet<Resource> alive = new();


    private void Start()
    {
        StartCoroutine(RespawnCoroutine()); // �ֱ������� ������ üũ
        //SpawnOne(); // ���� �ڿ�
        
    }

    private void Update()
    {
        // Debuging ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RespawnCoroutine());
            Debug.Log(alive.Count);
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(respawnDelay);

            if(alive.Count < maxItemCount)
            {
               SpawnOne();
            }
        }
    }
    // ������Ʈ�� �ϳ� �����ϴ� �Լ�
    private void SpawnOne()
    {
        if (harvestablePrefab.Count == 0) 
            return;

        int randomIndex = Random.Range(0, harvestablePrefab.Count);
        var prefrab = harvestablePrefab[randomIndex];

        Vector3 pos = GetRandomSpawnPosition();
        var go = Instantiate(prefrab, pos, Quaternion.identity);
        
        Resource resource = go.GetComponent<Resource>();
        if(resource != null)
        {
            alive.Add(resource);
            resource.OnDepleted += HandleResourceDepleted;
        }
        // ���� �ߴµ� ���ҽ��� �ֳ�? �̺�Ʈ�� �߰���~
    }

    // �������� ����Ʈ�� �ִ� ������Ʈ�� �������� ��ġ�� ����ִ� �Լ�
    private Vector3 GetRandomSpawnPosition()
    {
        int random = Random.Range(0, spawnPoint.Count);
        Collider spawnPosition = spawnPoint[random].GetComponent<Collider>();

        if (spawnPosition == null)
        {
            Debug.Log("�� ����");
            return Vector3.zero;
        }

        Bounds bounds = spawnPosition.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        Vector3 startRayPos = new Vector3(x, bounds.max.y + 10f, z);
        if(Physics.Raycast(startRayPos, Vector3.down, out RaycastHit hit, 100f))
        {
            return hit.point;
        }
        return new Vector3(x, 1f, z);
    }

    private void HandleResourceDepleted(Resource resource)
    {
        if(resource == null) return;
        alive.Remove(resource);
        resource.OnDepleted -= HandleResourceDepleted;
    }
}
