using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_trap : MonoBehaviour
{
    PlayerMovement playerMovement = null;
    bool OnCo = true;
    StatusEffectDisplay statusEffectDisplay; // ���� �̻� ���÷��� ��ũ��Ʈ ����

    void Start()
    {
        // ���� �̻� ���÷��� ��ü�� ã��
        statusEffectDisplay = FindObjectOfType<StatusEffectDisplay>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnCo == true)
        {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            float movespeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = movespeed * 0.1f;

            // ���� �̻� ȿ���� ��ųʸ��� �߰� (slow: 3�� ����)
            statusEffectDisplay.statusEffects["slow"] = 3.0f;

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
