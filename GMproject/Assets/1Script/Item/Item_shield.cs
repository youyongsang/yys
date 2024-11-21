using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_shield : MonoBehaviour
{
    public AudioClip shieldSound; // 동전 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = shieldSound; // 동전 사운드 클립 연결

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        // 이미 처리된 경우 더 이상 실행하지 않도록 플래그 사용
        if (hasTriggered) return;

        PlayerStat playerstat = other.gameObject.GetComponent<PlayerStat>();
        if (Input.GetKey(KeyCode.Z))
        {
            hasTriggered = true; // 플래그 설정

            // 방어막 증가
            playerstat.shield++;

            // 사운드 재생
            if (shieldSound != null && audioSource != null)
            {
                audioSource.Play();
            }

            // 아이템 제거
            Destroy(this.gameObject, 0.1f); // 0.1초 후 오브젝트 제거
        }
    }

    // 클래스 내부에 플래그 선언
    private bool hasTriggered = false;
}

