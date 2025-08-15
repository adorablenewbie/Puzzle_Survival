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
        // 랜덤한 위치를 생성하고 그 위치의 바닥을 찾아서 반환하는 함수
        // spawnAreaMin과 spawnAreaMax 사이의 랜덤한 x, z 좌표를 생성
        if (spawnAreaMin.x >= spawnAreaMax.x || spawnAreaMin.y >= spawnAreaMax.y)
        {
            Debug.LogError("Spawn area(스폰 구역)가 올바르게 정의되지 않았습니다. spawnAreaMin과 spawnAreaMax 값을 확인.");
            return Vector3.zero; // 잘못된 영역이 설정되었을 때 기본값 반환
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
        return GetRandomGroundPosition(); // 못찾았다면 재귀적으로 다시 찾아보자구
    }
}
