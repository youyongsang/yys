using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_key : MonoBehaviour
{
    public AudioClip keySound; // ���� ���� Ŭ��
    private AudioSource audioSource; // AudioSource ������Ʈ

    private void Start()
    {
        // AudioSource ������Ʈ �߰� �� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        audioSource.clip = keySound; // ���� ���� Ŭ�� ����
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
                audioSource.Play(); // ���� ���
            }
            playerstat.item_Key++;
            Destroy(this.gameObject, 0.1f);

        }
        // ���� ���� ���
    }
}
