using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_key : MonoBehaviour
{
    public AudioClip keySound; // 동전 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = keySound; // 동전 사운드 클립 연결
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        PlayerStat playerstat = other.gameObject.GetComponent<PlayerStat>();
        if (Input.GetKey(KeyCode.Z))
        {
            if (keySound != null && audioSource != null)
            {
                audioSource.Play(); // 사운드 재생
            }
            playerstat.item_Key++;
            Destroy(this.gameObject, 0.1f);

        }
        // 동전 사운드 재생
    }
}
