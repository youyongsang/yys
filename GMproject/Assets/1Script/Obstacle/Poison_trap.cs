using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_trap : MonoBehaviour
{
    private bool OnCo = true; // �� Ʈ�� Ȱ��ȭ ����
    private Coroutine poisonCoroutine; // ���� ���� �ڷ�ƾ ����

    public AudioClip poisonSound; // ���� ���� Ŭ��
    private AudioSource audioSource; // AudioSource ������Ʈ

    private void Start()
    {
        // AudioSource ������Ʈ �߰� �� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        audioSource.clip = poisonSound; // �� ���� Ŭ�� ����
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnCo)
        {
            PlayerStat playerStat = collision.gameObject.GetComponent<PlayerStat>();
            PlayerDisplay playerDisplay = FindObjectOfType<PlayerDisplay>();

            if (playerStat != null && playerDisplay != null)
            {
                // ���� �ڷ�ƾ �ߴ� �� �� �ڷ�ƾ ����
                if (poisonCoroutine != null)
                {
                    StopCoroutine(poisonCoroutine);
                }

                poisonCoroutine = StartCoroutine(Poison(playerStat, playerDisplay));
                OnCo = false; // Ʈ�� ��Ȱ��ȭ
                if (poisonSound != null && audioSource != null)
                {
                    audioSource.Play(); // ���� ���
                }
            }
        }
    }

    IEnumerator Poison(PlayerStat playerStat, PlayerDisplay playerDisplay)
    {
        // Poison ���� ����: ��Ʈ ������ ����
        playerDisplay.SetPoisonState(true);

        float duration = 10.0f; // �� ���� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Poison ���� ����: ��Ʈ ������ ���� �� ü�� ����
        playerDisplay.SetPoisonState(false);
        playerStat.lives -= 1;

        // Ʈ�� �ٽ� Ȱ��ȭ
        OnCo = true;
        poisonCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
    }

    public void CurePoison(PlayerDisplay playerDisplay)
    {
        // �� ���� ����
        if (poisonCoroutine != null)
        {
            StopCoroutine(poisonCoroutine);
            poisonCoroutine = null;
        }

        playerDisplay.SetPoisonState(false); // �� ���� ����
        OnCo = true; // �� Ʈ�� ��� ��Ȱ��ȭ
        Debug.Log("Poison cured and trap reactivated.");
    }
}
