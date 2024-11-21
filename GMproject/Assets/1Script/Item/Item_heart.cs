using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_heart : MonoBehaviour
{
    public AudioClip heartSound; // ���� ���� Ŭ��
    private AudioSource audioSource; // AudioSource ������Ʈ
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        audioSource.clip = heartSound; // ���� ���� Ŭ�� ����
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
            // ���� ���
            if (heartSound != null && audioSource != null)
            {
                audioSource.Play();
                Debug.Log("��Ʈ �Ҹ������");
            }
            Destroy(this.gameObject, 0.3f);
        }
    }
    private bool hasTriggered = false;
}
