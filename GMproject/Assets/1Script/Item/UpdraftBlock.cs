using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdraftBlock : MonoBehaviour
{
    public float liftForce = 20f; // 위로 올리는 힘
    public float maxLiftSpeed = 50f; // 최대 상승 속도 제한
    public float decelerationRate = 5f; // 블럭을 벗어난 후 감속 속도

    public AudioClip updraftSound; // 점프대 효과음
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        // AudioSource 컴포넌트 추가 및 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = updraftSound;
        audioSource.loop = true; // 점프대에 있는 동안 반복 재생
        audioSource.playOnAwake = false; // 자동 재생 비활성화
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // 위로 향하는 힘 추가
                Vector3 upwardForce = Vector3.up * liftForce;

                // 현재 속도가 최대 상승 속도를 초과하지 않도록 제한
                if (playerRigidbody.velocity.y < maxLiftSpeed)
                {
                    playerRigidbody.AddForce(upwardForce, ForceMode.Acceleration);
                }
            }

            // 효과음 재생 (이미 재생 중이 아니라면)
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
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
                // 상승 속도를 점진적으로 줄이도록 감속 처리
                StartCoroutine(SlowDown(playerRigidbody));
            }

            // 효과음 정지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private IEnumerator SlowDown(Rigidbody playerRigidbody)
    {
        // 상승 속도가 줄어들 때까지 반복
        while (playerRigidbody.velocity.y > 0)
        {
            Vector3 velocity = playerRigidbody.velocity;
            velocity.y -= decelerationRate * Time.deltaTime; // 감속
            playerRigidbody.velocity = velocity;

            yield return null;
        }
    }
}
