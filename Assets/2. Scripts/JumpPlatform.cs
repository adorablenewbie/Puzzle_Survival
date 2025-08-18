using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 200f; // 점프 플랫폼이 주는 힘

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 점프 플랫폼에 닿았을 때
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // 플레이어에게 점프 힘을 적용
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
