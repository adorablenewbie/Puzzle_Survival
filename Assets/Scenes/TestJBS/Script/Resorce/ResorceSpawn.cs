using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ResorceSpawn : MonoBehaviour
{
    [Header("풀링 시스템")]
    public List<GameObject> resorcePrefab; // 프리펩 생성
    public int poolSizePerPrefab = 10; // 프리팹 개수
    public float spawnRadius = 50f; // 스폰 범위
    public LayerMask groundLayer; // 지형, 확인 해봐야겠지
    public float yOffset = 0.2f; // 땅에 박히지 않도록 y를 살짝쿵



    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Start()
    {
        // 풀링 시스템 초기화
        // 프립펩별로 풀사이즈퍼브리펩 개수 만큼 오브젝트 생성
        // 생성 시 비활성화 상태로 두어 풀링 가능하게 함!
        foreach(var prefab in resorcePrefab)
        {
            for(int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
        //맵 위에 배치하고 활성화.
        foreach(var obj in pooledObjects)
        {
            PlaceObjectAtrandomPosition(obj);
            obj.SetActive(true);
        }
    }

    public void Respawnresorce(GameObject resource, float Delay)
    {
        StartCoroutine(RespawnCoroutine(resource)); // 딜레이 만큼 기다려서 리스폰
    }

    private IEnumerator RespawnCoroutine(GameObject resource)
    {
        yield return new WaitForSeconds(30f); // 딜레이 만큼 기다려
        PlaceObjectAtrandomPosition(resource);   // 랜덤하게 배치 해버려
        resource.SetActive(true);                // 활성화 시켜버려
    }

    private void PlaceObjectAtrandomPosition(GameObject obj)
    {
        Vector3 pos = GetRandomPosition(); // 랜덤 위치를 받아와
    }
    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; // 랜덤으로 X,Z 값 돌려야 겠지
        return transform.position + new Vector3(randomCircle.x, yOffset, randomCircle.y);
    }




}
