using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_space : MonoBehaviour
{
    public AudioClip slowSound; // 슬로우 효과음 클립
    private AudioSource audioSource; // AudioSource 컴포넌트
    private PlayerMovement playerMovement = null;
    private float originalMoveSpeed = 0; // 원래 이동 속도 저장
    private bool isInSlowZone = false; // 슬로우 영역 진입 여부 확인용

    void Start()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = slowSound; // 슬로우 효과음 연결
        audioSource.loop = true; // 반복 재생 설정
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isInSlowZone)
        {
            Debug.Log("슬로우 장판 진입");
            isInSlowZone = true; // 슬로우 상태 활성화
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                // 속도 감소
                originalMoveSpeed = playerMovement.moveSpeed;
                playerMovement.moveSpeed = originalMoveSpeed * 0.3f;
            }

            // 사운드 재생
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isInSlowZone)
        {
            Debug.Log("슬로우 장판 탈출");
            isInSlowZone = false; // 슬로우 상태 비활성화

            if (playerMovement != null)
            {
                // 속도 복원
                playerMovement.moveSpeed = originalMoveSpeed;
            }

            // 사운드 정지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
