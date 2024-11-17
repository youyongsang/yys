using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdraftBlock : MonoBehaviour
{
    public float liftForce = 20f; // ���� �ø��� ��
    public float maxLiftSpeed = 50f; // �ִ� ��� �ӵ� ����
    public float decelerationRate = 5f; // ���� ��� �� ���� �ӵ�

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // ���� ���ϴ� �� �߰�
                Vector3 upwardForce = Vector3.up * liftForce;

                // ���� �ӵ��� �ִ� ��� �ӵ��� �ʰ����� �ʵ��� ����
                if (playerRigidbody.velocity.y < maxLiftSpeed)
                {
                    playerRigidbody.AddForce(upwardForce, ForceMode.Acceleration);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // ��� �ӵ��� ���������� ���̵��� ���� ó��
                StartCoroutine(SlowDown(playerRigidbody));
            }
        }
    }

    private IEnumerator SlowDown(Rigidbody playerRigidbody)
    {
        // ��� �ӵ��� �پ�� ������ �ݺ�
        while (playerRigidbody.velocity.y > 0)
        {
            Vector3 velocity = playerRigidbody.velocity;
            velocity.y -= decelerationRate * Time.deltaTime; // ����
            playerRigidbody.velocity = velocity;

            yield return null;
        }
    }
}

