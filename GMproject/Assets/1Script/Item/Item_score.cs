using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_score : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("동전이 플레이어와 닿았음");
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                // 동전 태그에 따라 점수 추가
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

                Debug.Log($"점수 추가: {playerStat.score}");

                // 동전 아이템 제거
                Destroy(gameObject);
            }
        }
    }
}
