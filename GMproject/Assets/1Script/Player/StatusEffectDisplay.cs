using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectDisplay : MonoBehaviour
{
    public Dictionary<string, float> statusEffects = new Dictionary<string, float>()
    {
        {"slow", 0f},
        {"dotDamage", 0f}
    };

    public GameObject statusIconPrefab; // 상태 이상 이미지를 위한 프리팹
    public Transform statusEffectPanel; // 상태 이상 이미지를 표시할 UI 패널
    private List<GameObject> activeIcons = new List<GameObject>(); // 현재 활성화된 상태 이상 이미지들

    // 상태 이상 이미지 업데이트 함수
    void Update()
    {
        // 모든 상태 이상 딕셔너리를 확인하여 상태에 맞는 이미지를 업데이트
        foreach (var effect in statusEffects)
        {
            // 지속 시간이 0보다 큰 경우에만 처리
            if (effect.Value > 0)
            {
                // 해당 상태에 맞는 이미지를 활성화 및 업데이트
                UpdateStatusEffectIcon(effect.Key, effect.Value);

                // 시간이 경과함에 따라 상태 지속 시간 감소
                statusEffects[effect.Key] -= Time.deltaTime;
            }
            else
            {
                // 지속 시간이 0이거나 그 이하가 되면 이미지를 삭제
                RemoveStatusEffectIcon(effect.Key);
            }
        }

        // 상태가 없는 빈 이미지들을 앞으로 당김
        RearrangeIcons();
    }

    // 상태 이상 이미지를 활성화하거나 갱신하는 함수
    void UpdateStatusEffectIcon(string effectName, float duration)
    {
        // 기존에 존재하는 아이콘이 없다면 새로운 아이콘 생성
        if (!activeIcons.Exists(icon => icon.name == effectName))
        {
            GameObject newIcon = Instantiate(statusIconPrefab, statusEffectPanel);
            newIcon.name = effectName;
            activeIcons.Add(newIcon);
        }

        // 아이콘의 이미지와 회전 시계방향으로 설정
        GameObject iconObject = activeIcons.Find(icon => icon.name == effectName);
        Image iconImage = iconObject.GetComponent<Image>();
        iconImage.fillAmount = duration / 3.0f; // 3초를 기준으로 fillAmount를 설정
    }

    // 상태 이상 이미지 삭제 함수
    void RemoveStatusEffectIcon(string effectName)
    {
        GameObject iconObject = activeIcons.Find(icon => icon.name == effectName);
        if (iconObject != null)
        {
            Destroy(iconObject);
            activeIcons.Remove(iconObject);
        }
        // 상태 지속 시간을 0으로 초기화
        statusEffects[effectName] = 0;
    }

    // 모든 아이콘들을 앞으로 당겨 재배치
    void RearrangeIcons()
    {
        for (int i = 0; i < activeIcons.Count; i++)
        {
            RectTransform iconTransform = activeIcons[i].GetComponent<RectTransform>();
            iconTransform.anchoredPosition = new Vector2(-i * 50, 0); // 좌측으로 배치
        }
    }
}
