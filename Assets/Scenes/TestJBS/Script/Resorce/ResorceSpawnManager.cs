using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> HarvestablePrefab;
    public float respawnDelay; // 리스폰 딜레이 시간.
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;


    //private string[] tagCheck { get; set; } = { "Ground", "Rock", "Stone" };

    [Header("아이템 넣을께")]
    public int curItemCount;
    public int maxItemCount;
    public int minItemCount = 5;

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
    //

    private void Start()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private void Update()
    {
        // Debuging 용
        if (Input.GetKeyDown(KeyCode.R))
        {
            RequestRespawn(respawnDelay);
        }
    }

    // 디버깅용
    public void RequestRespawn(float delay)
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(respawnDelay);

            if(curItemCount < maxItemCount)
            {
                SpawnRandomPrefab();
                curItemCount++;
            }
        }

    }
    // 랜덤으로 위치를 잡아주는 함수
    private Vector3 GetRandomSpawnPosition()
    {
        float x = UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x,0f, z);
    }

    // 리스트에 담은 프리펩을 랜덤으로 생성하는 함수~
    public void SpawnRandomPrefab()
    {
        if (HarvestablePrefab == null || HarvestablePrefab.Count == 0)
        {
            Debug.Log("아이템 프리펩을 매니저 에다 넣어야지 뭐해");
            return;
        }
        int randomIndex = UnityEngine.Random.Range(0, HarvestablePrefab.Count);
        GameObject prefabToSpawn = Instantiate (HarvestablePrefab[randomIndex], GetRandomSpawnPosition(), Quaternion.identity);


    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(Array.Exists(tagCheck, tag => collision.gameObject.CompareTag(tag)))
    //    {
            
    //    }
    //}

}
