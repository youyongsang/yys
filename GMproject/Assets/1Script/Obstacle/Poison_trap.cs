using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_trap : MonoBehaviour
{
    private bool OnCo = true; // 독 트랩 활성화 여부
    private Coroutine poisonCoroutine; // 실행 중인 코루틴 참조

    public AudioClip poisonSound; // 동전 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = poisonSound; // 독 사운드 클립 연결
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnCo)
        {
            PlayerStat playerStat = collision.gameObject.GetComponent<PlayerStat>();
            PlayerDisplay playerDisplay = FindObjectOfType<PlayerDisplay>();

            if (playerStat != null && playerDisplay != null)
            {
                // 기존 코루틴 중단 후 새 코루틴 실행
                if (poisonCoroutine != null)
                {
                    StopCoroutine(poisonCoroutine);
                }

                poisonCoroutine = StartCoroutine(Poison(playerStat, playerDisplay));
                OnCo = false; // 트랩 비활성화
                if (poisonSound != null && audioSource != null)
                {
                    audioSource.Play(); // 사운드 재생
                }
            }
        }
    }

    IEnumerator Poison(PlayerStat playerStat, PlayerDisplay playerDisplay)
    {
        // Poison 상태 시작: 하트 아이콘 변경
        playerDisplay.SetPoisonState(true);

        float duration = 10.0f; // 독 지속 시간
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Poison 상태 종료: 하트 아이콘 복구 및 체력 감소
        playerDisplay.SetPoisonState(false);
        playerStat.lives -= 1;

        // 트랩 다시 활성화
        OnCo = true;
        poisonCoroutine = null; // 코루틴 참조 초기화
    }

    public void CurePoison(PlayerDisplay playerDisplay)
    {
        // 독 상태 해제
        if (poisonCoroutine != null)
        {
            StopCoroutine(poisonCoroutine);
            poisonCoroutine = null;
        }

        playerDisplay.SetPoisonState(false); // 독 상태 해제
        OnCo = true; // 독 트랩 즉시 재활성화
        Debug.Log("Poison cured and trap reactivated.");
    }
}
