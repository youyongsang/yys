using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_space : MonoBehaviour
{
    public AudioClip slowSound; // ���ο� ȿ���� Ŭ��
    private AudioSource audioSource; // AudioSource ������Ʈ
    private PlayerMovement playerMovement = null;
    private float originalMoveSpeed = 0; // ���� �̵� �ӵ� ����
    private bool isInSlowZone = false; // ���ο� ���� ���� ���� Ȯ�ο�

    void Start()
    {
        // AudioSource ������Ʈ �߰� �� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        audioSource.clip = slowSound; // ���ο� ȿ���� ����
        audioSource.loop = true; // �ݺ� ��� ����
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isInSlowZone)
        {
            Debug.Log("���ο� ���� ����");
            isInSlowZone = true; // ���ο� ���� Ȱ��ȭ
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                // �ӵ� ����
                originalMoveSpeed = playerMovement.moveSpeed;
                playerMovement.moveSpeed = originalMoveSpeed * 0.3f;
            }

            // ���� ���
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isInSlowZone)
        {
            Debug.Log("���ο� ���� Ż��");
            isInSlowZone = false; // ���ο� ���� ��Ȱ��ȭ

            if (playerMovement != null)
            {
                // �ӵ� ����
                playerMovement.moveSpeed = originalMoveSpeed;
            }

            // ���� ����
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
