using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_heart : MonoBehaviour
{
    public AudioClip heartSound; // 동전 사운드 클립
    private AudioSource audioSource; // AudioSource 컴포넌트
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
        audioSource.clip = heartSound; // 동전 사운드 클립 연결
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (hasTriggered) return;
        PlayerStat playerstat = other.gameObject.GetComponent<PlayerStat>();
        if (Input.GetKey(KeyCode.Z))
        {
            playerstat.lives++;
            // 사운드 재생
            if (heartSound != null && audioSource != null)
            {
                audioSource.Play();
                Debug.Log("하트 소리재생중");
            }
            Destroy(this.gameObject, 0.3f);
        }
    }
    private bool hasTriggered = false;
}
