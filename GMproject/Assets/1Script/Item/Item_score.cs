using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_score : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹 Ȯ��
        if (other.CompareTag("Player"))
        {
            Debug.Log("������ �÷��̾�� �����");
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                // ���� �±׿� ���� ���� �߰�
                if (gameObject.CompareTag("score500"))
                {
                    playerStat.score += 500;
                }
                else if (gameObject.CompareTag("score300"))
                {
                    playerStat.score += 300;
                }
                else if (gameObject.CompareTag("score100"))
                {
                    playerStat.score += 100;
                }

                Debug.Log($"���� �߰�: {playerStat.score}");

                // ���� ������ ����
                Destroy(gameObject);
            }
        }
    }
}
