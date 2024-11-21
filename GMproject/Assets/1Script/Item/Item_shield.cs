using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_shield : MonoBehaviour
{
    public AudioClip shieldSound; // ���� ���� Ŭ��
    private AudioSource audioSource; // AudioSource ������Ʈ
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        audioSource.clip = shieldSound; // ���� ���� Ŭ�� ����

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        // �̹� ó���� ��� �� �̻� �������� �ʵ��� �÷��� ���
        if (hasTriggered) return;

        PlayerStat playerstat = other.gameObject.GetComponent<PlayerStat>();
        if (Input.GetKey(KeyCode.Z))
        {
            hasTriggered = true; // �÷��� ����

            // �� ����
            playerstat.shield++;

            // ���� ���
            if (shieldSound != null && audioSource != null)
            {
                audioSource.Play();
            }

            // ������ ����
            Destroy(this.gameObject, 0.1f); // 0.1�� �� ������Ʈ ����
        }
    }

    // Ŭ���� ���ο� �÷��� ����
    private bool hasTriggered = false;
}

