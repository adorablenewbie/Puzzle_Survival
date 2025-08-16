
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

public class ResorceSpawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public List<GameObject> harvestablePrefab;
    public float respawnDelay; // ������ ������ �ð�.
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;


    //private string[] tagCheck { get; set; } = { "Ground", "Rock", "Stone" };

    [Header("������ ������")]
    public int curItemCount;
    public int maxItemCount;
    public int minItemCount = 5;

    private readonly HashSet<Resource> alive = new();


    private void Start()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private void Update()
    {
        // Debuging ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RespawnCoroutine());
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
        if (harvestablePrefab.Count == 0) return;

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

    // �������� ��ġ�� ����ִ� �Լ�
    private Vector3 GetRandomSpawnPosition()
    {
        float x = UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x,0f, z);
    }

    private void HandleResourceDepleted(Resource resource)
    {
        if(resource == null) return;
        alive.Remove(resource);
        resource.OnDepleted -= HandleResourceDepleted;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(Array.Exists(tagCheck, tag => collision.gameObject.CompareTag(tag)))
    //    {
            
    //    }
    //}

}
