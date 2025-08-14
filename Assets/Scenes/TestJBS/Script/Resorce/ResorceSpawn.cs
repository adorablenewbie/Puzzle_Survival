using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceSpawn : MonoBehaviour
{
    [Header("풀링 시스템")]
    public List<GameObject> resorcePrefab; // 프리펩 생성
    public int poolSizePerPrefab = 10; // 프리팹 개수
    public float spawnRadius = 50f; // 스폰 범위
    public LayerMask groundLayer;
    public float yOffset = 0.2f; // 땅에 박히지 않도록 y를 살짝쿵



    private List<GameObject> pooledObjects = new List<GameObject>();



 





}
