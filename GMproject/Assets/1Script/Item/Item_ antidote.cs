using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_antidote : MonoBehaviour
{
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

                // �ص��� ������ ����
                Destroy(gameObject);

                Debug.Log("�ص����� ���Ǿ����ϴ�. �� ���°� �����Ǿ����ϴ�.");
            }
        }
    }
}
