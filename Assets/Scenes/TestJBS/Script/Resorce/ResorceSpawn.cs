using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ResorceSpawn : MonoBehaviour
{
    [Header("Ǯ�� �ý���")]
    public List<GameObject> resorcePrefab; // ������ ����
    public int poolSizePerPrefab = 10; // ������ ����
    public float spawnRadius = 50f; // ���� ����
    public LayerMask groundLayer; // ����, Ȯ�� �غ��߰���
    public float yOffset = 0.2f; // ���� ������ �ʵ��� y�� ��¦��



    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Start()
    {
        // Ǯ�� �ý��� �ʱ�ȭ
        // �����麰�� Ǯ�������ۺ긮�� ���� ��ŭ ������Ʈ ����
        // ���� �� ��Ȱ��ȭ ���·� �ξ� Ǯ�� �����ϰ� ��!
        foreach(var prefab in resorcePrefab)
        {
            for(int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
        //�� ���� ��ġ�ϰ� Ȱ��ȭ.
        foreach(var obj in pooledObjects)
        {
            PlaceObjectAtrandomPosition(obj);
            obj.SetActive(true);
        }
    }

    public void Respawnresorce(GameObject resource, float Delay)
    {
        StartCoroutine(RespawnCoroutine(resource)); // ������ ��ŭ ��ٷ��� ������
    }

    private IEnumerator RespawnCoroutine(GameObject resource)
    {
        yield return new WaitForSeconds(30f); // ������ ��ŭ ��ٷ�
        PlaceObjectAtrandomPosition(resource);   // �����ϰ� ��ġ �ع���
        resource.SetActive(true);                // Ȱ��ȭ ���ѹ���
    }

    private void PlaceObjectAtrandomPosition(GameObject obj)
    {
        Vector3 pos = GetRandomPosition(); // ���� ��ġ�� �޾ƿ�
    }
    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; // �������� X,Z �� ������ ����
        return transform.position + new Vector3(randomCircle.x, yOffset, randomCircle.y);
    }




}
