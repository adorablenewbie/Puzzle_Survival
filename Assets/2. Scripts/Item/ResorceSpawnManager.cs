using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> harvestablePrefab;
    public float respawnDelay; // 리스폰 딜레이 시간.
    // 스폰 포인트를 지정할 수 있는 리스트
    public List<GameObject> spawnPoint;

    [Header("아이템 넣을께")]
    public int maxItemCount;
    public int minItemCount;

    private readonly HashSet<Resource> alive = new();


    private void Start()
    {
        StartCoroutine(RespawnCoroutine()); // 주기적으로 리스폰 체크
        //SpawnOne(); // 시작 자원
        
    }

    private void Update()
    {
        // Debuging 용
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
    // 오브젝트를 하나 생성하는 함수
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
        // 스폰 했는데 리소스가 있네? 이벤트에 추가해~
    }

    // 랜덤으로 리스트에 있는 오브젝트의 기준으로 위치를 잡아주는 함수
    private Vector3 GetRandomSpawnPosition()
    {
        int random = Random.Range(0, spawnPoint.Count);
        Collider spawnPosition = spawnPoint[random].GetComponent<Collider>();

        if (spawnPosition == null)
        {
            Debug.Log("야 없어");
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
