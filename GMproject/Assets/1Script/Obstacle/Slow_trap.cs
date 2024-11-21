using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_trap : MonoBehaviour
{
    public AudioClip slowSound; // ���� ���� Ŭ��
    private AudioSource audioSource; // AudioSource ������Ʈ
    PlayerMovement playerMovement = null;
    bool OnCo = true;

    void Start()
    {
        // AudioSource ������Ʈ �߰� �� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        audioSource.clip = slowSound; // ���� ���� Ŭ�� ����

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
