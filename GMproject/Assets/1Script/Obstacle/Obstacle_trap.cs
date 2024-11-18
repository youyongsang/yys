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
        // 충돌한 객체가 플레이어일 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            // PlayerMovement 스크립트에서 마지막 이동 방향 가져오기
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerStat Playerstat = collision.gameObject.GetComponent<PlayerStat>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerMovement != null && playerRb != null)
            {
                // 마지막 이동 방향의 반대 방향으로 밀쳐내기
                Vector3 pushDirection = -playerMovement.lastMoveDirection.normalized;

                if (pushDirection != Vector3.zero)
                {
                    Debug.Log("진입확인");
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
