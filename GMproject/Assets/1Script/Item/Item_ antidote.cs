using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_antidote : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌 확인
        {
            PlayerDisplay playerDisplay = FindObjectOfType<PlayerDisplay>();
            Poison_trap poisonTrap = FindObjectOfType<Poison_trap>(); // 현재 독 트랩 참조

            if (playerDisplay != null && playerDisplay.IsPoisoned()) // 플레이어가 독 상태인지 확인
            {
                // 독 상태 해제
                poisonTrap?.CurePoison(playerDisplay);

                // 해독제 아이템 제거
                Destroy(gameObject);

                Debug.Log("해독제가 사용되었습니다. 독 상태가 해제되었습니다.");
            }
        }
    }
}
