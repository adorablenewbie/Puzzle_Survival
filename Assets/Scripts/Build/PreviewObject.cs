using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]public Vector3 cubeSize = Vector3.zero; // 큐브 크기
    public Color cubeColor = Color.green;  // 큐브 색상

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


// List가 있다. (배열)// 
// List 길이를 50으로 해놓고
// 생성을 하고 -> List에 Add
// List의 길이 -> 지금 들어가 있는 오브젝트 개수 -> 지금 생성되어 있는 오브젝트의 개수
// 50개까지만 생성을 하고 -> Add해준다.

// Resource -> List

// 따로따로 돌따로 나무따로 풀따로

// 리소스매니저에서 해준다.
// 플레이어가 오브젝트를 쳤어 그러면 Destroy() -> List에서 Remove
// 현재 길이 49 List.Count가 50 이 안되면 50로 맞춰줘야함
// -> Instantiate -> Add
// -> 

// 풀타입, 나무타입, 돌타입
// 10, 10, 10
// if(itemType == HarvertableType.Grass)
// {
//      for(int i = 0; i < 10; i++) // 10번
//      {
//          1개 생성한다.
//          List.Add();
//      }
// }
// 

// Destroy -> 
// 델리게이트 -> Destory -> ResourceManager에 있는 개수를 하나 줄여주는 메서드를 같이 실행시킨다.
// 
// 