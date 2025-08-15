using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> resourcePrefab;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;
    public Vector3 RandomSpawnPosition;

    public GameObject prefab;

    [SerializeField]
    private  int maxItemCount = 10;



    
    private void Update()
    {
        // Debuging ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            RequestRespawn(prefab, 5f);
        }
    }


    public void RequestRespawn(GameObject prefab, float delay)
    {
        StartCoroutine(RespawnCoroutine(prefab, delay));
    }


    private IEnumerator RespawnCoroutine(GameObject target, float delay)
    {
        yield return new WaitForSeconds(5f);
        Instantiate(prefab, GetRandomSpawnPosition(), Quaternion.identity);

    }

    private Vector3 GetRandomSpawnPosition()
    {


        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, y, z);
    }




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
