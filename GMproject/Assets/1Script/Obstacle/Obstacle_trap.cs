using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_trap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public float pushBackSpeed = 10f;
    public float pushBackDuration = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �÷��̾��� ���
        if (collision.gameObject.CompareTag("Player"))
        {
            // PlayerMovement ��ũ��Ʈ���� ������ �̵� ���� ��������
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerStat Playerstat = collision.gameObject.GetComponent<PlayerStat>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerMovement != null && playerRb != null)
            {
                // ������ �̵� ������ �ݴ� �������� ���ĳ���
                Vector3 pushDirection = -playerMovement.lastMoveDirection.normalized;

                if (pushDirection != Vector3.zero)
                {
                    Debug.Log("����Ȯ��");
                    if (Playerstat.shield >= 1)
                    {
                        Playerstat.shield -= 1;
                    }
                    else
                    {
                        Playerstat.lives -= 1;
                    }
                    playerRb.AddForce(pushDirection * 15f, ForceMode.Impulse);
                    //collision.transform.Translate(pushDirection * 15f);
                    

                }
            }
        }
    }

}
