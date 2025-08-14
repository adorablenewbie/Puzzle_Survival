using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 200f; // ���� �÷����� �ִ� ��

    void OnTriggerEnter(Collider other)
    {
        // �÷��̾ ���� �÷����� ����� ��
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // �÷��̾�� ���� ���� ����
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
