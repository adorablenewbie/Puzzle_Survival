
// 리스트가 있다, (배열)
// 리스트의 길이를 50을 해놓고
// 생성을 하고 리스트에 에드
// 리스트의 길이 -> 지금 들어가 있는 오브젝트의 개수 -> 지금 생성되어 있는 오브젝트의 개수
// 50개 까지만 생성을 하고 -> Add 해준다.
// 

// 플레이어가 오브젝트를 채집하면 디스트로이 하고 리스트에서 리무브
// 현재 길이 49 -> 50개가 되어야 한다. 리스트 카운트가 50이 안되면 
// 디스트로이를 하고 길이를 확인하고 애드
// 델리게이트러 디스트로이가 될 때 매니저에 있는 카운트를 낮춰주는 매서드를
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> harvestablePrefab;
    public float respawnDelay; // 리스폰 딜레이 시간.
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;


    //private string[] tagCheck { get; set; } = { "Ground", "Rock", "Stone" };

    [Header("아이템 넣을께")]
    public int maxItemCount;
    public int minItemCount = 5;

    private readonly HashSet<Resource> alive = new();


    private void Start()
    {
        StartCoroutine(RespawnCoroutine());
        SpawnOne();
        
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
    private void SpawnOne()
    {
        if (harvestablePrefab.Count == 0) 
            return;

        int randomIndex = UnityEngine.Random.Range(0, harvestablePrefab.Count);
        var prefrab = harvestablePrefab[randomIndex];

        Vector3 pos = GetRandomSpawnPosition();
        var go = Instantiate(prefrab, pos, Quaternion.identity);
        
        Resource resource = go.GetComponent<Resource>();
        if(resource != null)
        {
            alive.Add(resource);
            resource.OnDepleted += HandleResourceDepleted;
        }

    }

    // 랜덤으로 위치를 잡아주는 함수
    private Vector3 GetRandomSpawnPosition()
    {
        float x = UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, y, z);
    }

    
    private void HandleResourceDepleted(Resource resource)
    {
        if(resource == null) return;
        alive.Remove(resource);
        resource.OnDepleted -= HandleResourceDepleted;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        int need =  maxItemCount - alive.Count;
        for (int i = 0; i < need; i++)
        {
            SpawnOne();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(Array.Exists(tagCheck, tag => collision.gameObject.CompareTag(tag)))
    //    {
            
    //    }
    //}

}
