using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_antidote : MonoBehaviour
{
    public AudioClip antidoteSound; // 해독제 효과음
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        // AudioSource 컴포넌트 추가 및 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = antidoteSound;
        audioSource.playOnAwake = false; // 자동 재생 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌 확인
        {
            PlayerDisplay playerDisplay = FindObjectOfType<PlayerDisplay>();
            Poison_trap poisonTrap = FindObjectOfType<Poison_trap>(); // 현재 독 트랩 참조

            if (playerDisplay != null && playerDisplay.IsPoisoned()) // 플레이어가 독 상태인지 확인
            {
                // 독 상태 해제
                poisonTrap?.CurePoison(playerDisplay);

                // 해독제 효과음 재생
                if (antidoteSound != null && audioSource != null)
                {
                    audioSource.Play();
                }

                // 아이템 제거를 조금 지연하여 효과음이 끝나고 삭제되도록 설정
                Destroy(gameObject, antidoteSound != null ? antidoteSound.length : 0);

                Debug.Log("해독제가 사용되었습니다. 독 상태가 해제되었습니다.");
            }
        }
    }
}
