using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_trap : MonoBehaviour
{
    PlayerMovement playerMovement = null;
    bool OnCo = true;
    StatusEffectDisplay statusEffectDisplay; // 상태 이상 디스플레이 스크립트 참조

    void Start()
    {
        // 상태 이상 디스플레이 객체를 찾음
        statusEffectDisplay = FindObjectOfType<StatusEffectDisplay>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnCo == true)
        {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            float movespeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = movespeed * 0.1f;

            // 상태 이상 효과를 딕셔너리에 추가 (slow: 3초 지속)
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
