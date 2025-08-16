using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreviewObject : MonoBehaviour
{
    // ㅇ

    [SerializeField]public Vector3 cubeSize = Vector3.zero; // 큐브 크기
    public Color cubeColor = Color.green;  // 큐브 색상


    [SerializeField] private LayerMask grondLayer;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = cubeColor;
        Gizmos.DrawCube(transform.position, cubeSize); // 중심 위치, 크기
    }
}
