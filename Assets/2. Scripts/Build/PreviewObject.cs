using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]public Vector3 cubeSize = Vector3.zero; // ť�� ũ��
    public Color cubeColor = Color.green;  // ť�� ����

    private Renderer meshRenderer;

    [SerializeField] private LayerMask groundLayer;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeMaterial();
    }

    private void ChangeMaterial()
    {
        if(colliderList.Count > 0)
        {
            SetMaterial(red);
        }
        else
        {
            SetMaterial(green);
        }
    }

    private void SetMaterial(Material mat)
    {
        //Material[] materials = new Material[GetComponent<MeshRenderer>().materials.Length];
        //Material[] materials = new Material[meshRenderer.materials.Length];
        //Debug.Log(meshRenderer.materials.Length);
        //for (int i = 0; i < materials.Length; i++)
        //{
        //    materials[i] = mat;
        //}

        //meshRenderer.materials = materials;

        meshRenderer.material = mat;

    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.layer != groundLayer && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        if(((1 << other.gameObject.layer) & groundLayer) == 0 && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);

    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) == 0 && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);

    }


    public bool IsBuildable()
    {
        return colliderList.Count == 0;
    }
}


// List�� �ִ�. (�迭)// 
// List ���̸� 50���� �س���
// ������ �ϰ� -> List�� Add
// List�� ���� -> ���� �� �ִ� ������Ʈ ���� -> ���� �����Ǿ� �ִ� ������Ʈ�� ����
// 50�������� ������ �ϰ� -> Add���ش�.

// Resource -> List

// ���ε��� ������ �������� Ǯ����

// ���ҽ��Ŵ������� ���ش�.
// �÷��̾ ������Ʈ�� �ƾ� �׷��� Destroy() -> List���� Remove
// ���� ���� 49 List.Count�� 50 �� �ȵǸ� 50�� ���������
// -> Instantiate -> Add
// -> 

// ǮŸ��, ����Ÿ��, ��Ÿ��
// 10, 10, 10
// if(itemType == HarvertableType.Grass)
// {
//      for(int i = 0; i < 10; i++) // 10��
//      {
//          1�� �����Ѵ�.
//          List.Add();
//      }
// }
// 

// Destroy -> 
// ��������Ʈ -> Destory -> ResourceManager�� �ִ� ������ �ϳ� �ٿ��ִ� �޼��带 ���� �����Ų��.
// 
// 