using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusEffectPrefab
{
    public string effectName;       // 상태 이름 (예: "slow")
    public GameObject prefab;       // 상태 아이콘 프리팹
}

public class StatusEffectDisplay : MonoBehaviour
{
    public List<StatusEffectPrefab> statusEffectPrefabsList; // Inspector에서 설정할 상태 효과 프리팹 리스트
    private Dictionary<string, GameObject> statusIconPrefabs; // 내부에서 사용하는 Dictionary

    public Dictionary<string, float> statusEffects = new Dictionary<string, float>()
    {
        {"slow", 0f},
        {"dotDamage", 0f}
    };

    public Transform statusEffectPanel; // 상태 이상 이미지를 표시할 UI 패널
    private List<GameObject> activeIcons = new List<GameObject>(); // 현재 활성화된 상태 이상 이미지들

    // Start 메서드에서 List 데이터를 Dictionary로 변환
    void Start()
    {
        // List를 Dictionary로 변환하여 상태 이름으로 프리팹을 쉽게 찾을 수 있게 함
        statusIconPrefabs = new Dictionary<string, GameObject>();
        foreach (var item in statusEffectPrefabsList)
        {
            if (!statusIconPrefabs.ContainsKey(item.effectName))
            {
                statusIconPrefabs.Add(item.effectName, item.prefab);
            }
        }
    }

    // 상태 이상 이미지 업데이트 함수
    void Update()
    {
        List<string> effectsToRemove = new List<string>();

        foreach (var effect in statusEffects)
        {
            if (effect.Value > 0)
            {
                UpdateStatusEffectIcon(effect.Key, effect.Value);
                statusEffects[effect.Key] -= Time.deltaTime;
            }
            else
            {
                effectsToRemove.Add(effect.Key);
            }
        }

        foreach (var effectName in effectsToRemove)
        {
            RemoveStatusEffectIcon(effectName);
        }

        RearrangeIcons();
    }

    // 상태 이상 이미지를 활성화하거나 갱신하는 함수
    void UpdateStatusEffectIcon(string effectName, float duration)
    {
        // 상태 이름에 맞는 프리팹이 있는지 확인
        if (!statusIconPrefabs.ContainsKey(effectName))
        {
            Debug.LogWarning("No icon prefab found for effect: " + effectName);
            return;
        }

        // 기존에 존재하는 아이콘이 없다면 새로운 아이콘 생성
        if (!activeIcons.Exists(icon => icon.name == effectName))
        {
            GameObject newIcon = Instantiate(statusIconPrefabs[effectName], statusEffectPanel);
            newIcon.name = effectName;
            activeIcons.Add(newIcon);
        }

        // 아이콘의 이미지와 회전 시계방향으로 설정
        GameObject iconObject = activeIcons.Find(icon => icon.name == effectName);
        if (iconObject != null)
        {
            Image iconImage = iconObject.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.fillAmount = duration / 3.0f;
            }
        }
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
