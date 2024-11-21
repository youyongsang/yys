using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_score : MonoBehaviour
{
    public AudioClip coinSound; // 동전 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = coinSound; // 동전 사운드 클립 연결
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("동전이 플레이어와 닿았음");
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                // 동전 태그에 따라 점수 추가
                if (gameObject.CompareTag("score500"))
                {
                    playerStat.score += 500;
                }
                else if (gameObject.CompareTag("score300"))
                {
                    playerStat.score += 300;
                }
                else if (gameObject.CompareTag("score100"))
                {
                    playerStat.score += 100;
                }

                Debug.Log($"점수 추가: {playerStat.score}");

                // 동전 사운드 재생
                if (coinSound != null && audioSource != null)
                {
                    audioSource.Play(); // 사운드 재생
                }

                // 동전 아이템 제거 (사운드 재생 후 삭제)
                Destroy(gameObject, 0.1f); // 0.1초 대기 후 오브젝트 제거
            }
        }
    }
}
