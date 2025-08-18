
// ����Ʈ�� �ִ�, (�迭)
// ����Ʈ�� ���̸� 50�� �س���
// ������ �ϰ� ����Ʈ�� ����
// ����Ʈ�� ���� -> ���� �� �ִ� ������Ʈ�� ���� -> ���� �����Ǿ� �ִ� ������Ʈ�� ����
// 50�� ������ ������ �ϰ� -> Add ���ش�.
// 

// �÷��̾ ������Ʈ�� ä���ϸ� ��Ʈ���� �ϰ� ����Ʈ���� ������
// ���� ���� 49 -> 50���� �Ǿ�� �Ѵ�. ����Ʈ ī��Ʈ�� 50�� �ȵǸ� 
// ��Ʈ���̸� �ϰ� ���̸� Ȯ���ϰ� �ֵ�
// ��������Ʈ�� ��Ʈ���̰� �� �� �Ŵ����� �ִ� ī��Ʈ�� �����ִ� �ż��带
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> harvestablePrefab;
    public float respawnDelay; // ������ ������ �ð�.
    // ���� ����Ʈ�� ������ �� �ִ� ����Ʈ
    public List<GameObject> spawnPoint;


    //private string[] tagCheck { get; set; } = { "Ground", "Rock", "Stone" };

    [Header("������ ������")]
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

    // �������� ����Ʈ�� �ִ� ������Ʈ�� �������� ��ġ�� ����ִ� �Լ�
    private Vector3 GetRandomSpawnPosition()
    {
        int random = UnityEngine.Random.Range(0, spawnPoint.Count);
        Collider spawnPosition = spawnPoint[random].GetComponent<Collider>();

        if (spawnPosition == null)
        {
            Debug.Log("�� ����");
            return Vector3.zero;
        }

        Bounds bounds = spawnPosition.bounds;
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, 1f, z);
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
