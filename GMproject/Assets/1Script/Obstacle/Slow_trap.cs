using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_trap : MonoBehaviour
{
    public AudioClip slowSound; // 동전 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트
    PlayerMovement playerMovement = null;
    bool OnCo = true;

    void Start()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = slowSound; // 동전 사운드 클립 연결

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnCo == true)
        {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            float movespeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = movespeed * 0.1f;

            StartCoroutine(Speed(movespeed));
            OnCo = false;
        }
    }

    IEnumerator Speed(float movespeed)
    {
        yield return new WaitForSeconds(3.0f);
        playerMovement.moveSpeed = movespeed;
        OnCo = true;
    }
}
