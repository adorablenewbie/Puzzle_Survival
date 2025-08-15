using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> resourcePrefab;
    public float respawnDelay; // ������ ������ �ð�
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;
    public Vector3 RandomSpawnPosition;
    public uint maxItem;
    public uint minItem;
    public List<GameObject> prefab;


    //private string[] tagCheck { get; set; } = { "Ground", "Rock", "Stone" };

    

    [SerializeField]
    private  int maxItemCount = 10;



    private void Update()
    {
        // Debuging ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            RequestRespawn(respawnDelay);
        }
    }


    public void RequestRespawn(float delay)
    {
        StartCoroutine(RespawnCoroutine());
    }


    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SpawnRandomPrefab();

    }
    // �������� ��ġ�� ����ִ� �Լ�
    private Vector3 GetRandomSpawnPosition()
    {
        float x = UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, y, z);
    }

    // ����Ʈ�� ���� �������� �������� �����ϴ� �Լ�~
    public void SpawnRandomPrefab()
    {
        if (prefab == null || prefab.Count == 0)
        {
            Debug.Log("������ �������� �Ŵ��� ���� �־���� ����");
            return;
        }
        int randomIndex = UnityEngine.Random.Range(0, prefab.Count);
        GameObject prefabToSpawn = Instantiate (prefab[randomIndex], GetRandomSpawnPosition(), Quaternion.identity);


    }




    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(Array.Exists(tagCheck, tag => collision.gameObject.CompareTag(tag)))
    //    {
            
    //    }
    //}





    //{
    //    private Coroutine waveRoutine; // ���� ���� ���� ���̺� �ڷ�ƾ
    //    [SerializeField]
    //    private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ
    //    [SerializeField]
    //    private List<Rect> spawnAreas; // ���� ������ ���� ����Ʈ
    //    [SerializeField]
    //    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // ����� ����
    //    private List<EnemyController> activeEnemies = new List<EnemyController>(); // ���� Ȱ��ȭ�� ����
    //    private bool enemySpawnComplite; // ���� ���̺� ������ �Ϸ�Ǿ����� ����
    //    [SerializeField] private float timeBetweenSpawns = 0.2f; // ���� �� ���� �� ����
    //    [SerializeField] private float timeBetweenWaves = 1f; // ���̺� �� ��� �ð�
    //    GameManager gameManager;

    //    public void Init(GameManager gameManager)
    //    {
    //        this.gameManager = gameManager;
    //    }

    //    // ���̺� ���� (waveCount: ������ �� ��)
    //    public void StartWave(int waveCount)
    //    {
    //        if (waveCount <= 0)
    //        {
    //            gameManager.EndOfWave();
    //            return;
    //        }
    //        // ���� ���̺갡 ���� ���̸� �ߴ�
    //        if (waveRoutine != null)
    //            StopCoroutine(waveRoutine);
    //        // �� ���̺� ����
    //        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    //    }

    //    // ���� ���� ���� ��� ���̺�/������ ����
    //    public void StopWave()
    //    {
    //        StopAllCoroutines();
    //    }

    //    // ������ �� ��ŭ ���� �����ϴ� �ڷ�ƾ
    //    private IEnumerator SpawnWave(int waveCount)
    //    {
    //        enemySpawnComplite = false;
    //        yield return new WaitForSeconds(timeBetweenWaves);
    //        for (int i = 0; i < waveCount; i++)
    //        {
    //            // ���̺� �� ��� �ð�
    //            yield return new WaitForSeconds(timeBetweenSpawns);
    //            SpawnRandomEnemy();
    //        }
    //        enemySpawnComplite = true;
    //    }

    //    // �� �ϳ��� ���� ��ġ�� ����
    //    private void SpawnRandomEnemy()
    //    {
    //        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
    //        {
    //            Debug.LogWarning("Enemy Prefabs �Ǵ� Spawn Areas�� �������� �ʾҽ��ϴ�.");
    //            return;
    //        }
    //        // ������ �� ������ ����
    //        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    //        // ������ ���� ����
    //        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
    //        // Rect ���� ������ ���� ��ġ ���
    //        Vector2 randomPosition = new Vector2(
    //            Random.Range(randomArea.xMin, randomArea.xMax),
    //            Random.Range(randomArea.yMin, randomArea.yMax)
    //        );
    //        // �� ���� �� ����Ʈ�� �߰�
    //        GameObject spawnedEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
    //        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
    //        enemyController.Init(this, gameManager.player.transform);
    //        activeEnemies.Add(enemyController);
    //    }
    //    // ����� �׷� ������ �ð�ȭ (���õ� ��쿡�� ǥ��)
    //    private void OnDrawGizmosSelected()
    //    {
    //        if (spawnAreas == null) return;
    //        Gizmos.color = gizmoColor;
    //        foreach (var area in spawnAreas)
    //        {
    //            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
    //            Vector3 size = new Vector3(area.width, area.height);
    //            Gizmos.DrawCube(center, size);
    //        }
    //    }
    //    public void RemoveEnemyOnDeath(EnemyController enemy)
    //    {
    //        activeEnemies.Remove(enemy);
    //        if (enemySpawnComplite && activeEnemies.Count == 0)
    //            gameManager.EndOfWave();
    //    }
    //}
}
