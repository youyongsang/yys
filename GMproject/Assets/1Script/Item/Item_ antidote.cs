using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_antidote : MonoBehaviour
{
    public AudioClip antidoteSound; // �ص��� ȿ����
    private AudioSource audioSource; // AudioSource ������Ʈ

    private void Start()
    {
        // AudioSource ������Ʈ �߰� �� �ʱ�ȭ
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = antidoteSound;
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� �浹 Ȯ��
        {
            PlayerDisplay playerDisplay = FindObjectOfType<PlayerDisplay>();
            Poison_trap poisonTrap = FindObjectOfType<Poison_trap>(); // ���� �� Ʈ�� ����

            if (playerDisplay != null && playerDisplay.IsPoisoned()) // �÷��̾ �� �������� Ȯ��
            {
                // �� ���� ����
                poisonTrap?.CurePoison(playerDisplay);

                // �ص��� ȿ���� ���
                if (antidoteSound != null && audioSource != null)
                {
                    audioSource.Play();
                }

                // ������ ���Ÿ� ���� �����Ͽ� ȿ������ ������ �����ǵ��� ����
                Destroy(gameObject, antidoteSound != null ? antidoteSound.length : 0);

                Debug.Log("�ص����� ���Ǿ����ϴ�. �� ���°� �����Ǿ����ϴ�.");
            }
        }
    }
}
