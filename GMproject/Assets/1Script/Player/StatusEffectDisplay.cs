using System.Collections;
using System.Collections.Generic;
using System.Linq; // 이 줄을 추가
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusEffect
{
    public string name;       // 상태 이상 이름
    public Sprite icon;       // 상태 이상 아이콘
}

public class StatusEffectDisplay : MonoBehaviour
{
    public GameObject panel;                      // 상태 이상 이미지를 표시할 패널
    public List<StatusEffect> statusEffects;      // 상태 이상 이름 및 아이콘을 인스펙터에서 설정
    public int iconSize = 32;                     // 아이콘 기본 크기

    public Dictionary<string, float> statusTimers = new Dictionary<string, float>(); // 상태 이상 이름과 지속 시간
    private Dictionary<string, Image> statusImages = new Dictionary<string, Image>(); // 상태 이상 이름과 이미지 컴포넌트
    private RectTransform panelRectTransform;      // 패널의 RectTransform 참조

    void Start()
    {
        if (panel == null)
        {
            Debug.LogError("패널이 할당되지 않았습니다!");
            return;
        }

        foreach (StatusEffect effect in statusEffects)
        {
            Debug.Log($"Adding status effect: {effect.name}");
            statusTimers[effect.name] = 0f;
        }

        panelRectTransform = panel.GetComponent<RectTransform>();
        UpdatePanelSize();
    }

    void Update()
    {
        // 상태 이상 지속 시간을 순회하며 갱신
        List<string> expiredEffects = new List<string>();
        var keys = statusTimers.Keys.ToList(); // ToList() 한 번만 호출
        
        foreach (var effectName in keys)
        {
            if (statusTimers.ContainsKey(effectName) && statusTimers[effectName] > 0)
            {
                statusTimers[effectName] -= Time.deltaTime;

                if (!statusImages.ContainsKey(effectName))
                {
                    CreateStatusIcon(effectName);
                }

                // 아이콘의 사라지는 애니메이션 (시계방향으로 감소)
                if (statusImages.ContainsKey(effectName))
                {
                    Debug.Log("if (statusImages.ContainsKey(effectName)) 진입확인");
                    UpdateStatusIcon(statusImages[effectName], statusTimers[effectName]);
                }
            }

            // 지속 시간이 0 이하가 되면 제거할 항목에 추가
            if (statusTimers.ContainsKey(effectName) && statusTimers[effectName] <= 0)
            {
                expiredEffects.Add(effectName);
            }
        }

        // 만료된 상태 이상 제거
        foreach (var effectName in expiredEffects)
        {
            if (statusImages.ContainsKey(effectName))
            {
                Destroy(statusImages[effectName].gameObject);
                statusImages.Remove(effectName);
            }
            if (statusTimers.ContainsKey(effectName))
            {
                statusTimers.Remove(effectName); // 딕셔너리에서 상태이상 삭제
            }
        }

        // 상태 이상 아이콘 재정렬
        ReorderIcons();
    }
    private Dictionary<string, float> statusMaxTimers = new Dictionary<string, float>(); // 초기 최대 지속 시간
    public void SetStatusEffect(string effectName, float duration)
    {
        if (statusTimers.ContainsKey(effectName))
        {
            statusTimers[effectName] = duration;
            if (!statusMaxTimers.ContainsKey(effectName))
            {
                statusMaxTimers[effectName] = duration; // 초기 지속 시간 저장
            }
        }
        else
        {
            Debug.LogWarning($"Status effect '{effectName}' not found!");
        }
    }

    private void CreateStatusIcon(string effectName)
    {
        StatusEffect effect = statusEffects.Find(e => e.name == effectName);
        if (effect == null || effect.icon == null) return;

        GameObject iconObject = new GameObject(effectName, typeof(Image));
        iconObject.transform.SetParent(panel.transform, false);

        Image iconImage = iconObject.GetComponent<Image>();
        iconImage.sprite = effect.icon;
        iconImage.rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
        iconImage.rectTransform.pivot = new Vector2(0, 1);

        statusImages[effectName] = iconImage;

        UpdatePanelSize();
    }

    private void UpdateStatusIcon(Image iconImage, float remainingTime)
    {
        if (statusTimers.ContainsKey(iconImage.name) && statusMaxTimers.ContainsKey(iconImage.name))
        {
            float maxTime = statusMaxTimers[iconImage.name]; // 초기 최대 시간 참조
            float percentage = Mathf.Clamp01(remainingTime / maxTime);
            Debug.Log(percentage);
            iconImage.fillMethod = Image.FillMethod.Radial360; // Radial360 방식으로 설정
            iconImage.fillClockwise = true;                    // 시계방향으로 감소
            iconImage.fillOrigin = (int)Image.Origin360.Top;    // 위쪽에서 시작
            iconImage.fillAmount = percentage;                 // fillAmount 설정

            Debug.Log($"Effect: {iconImage.name}, Percentage: {percentage}");
        }
    }

    private void ReorderIcons()
    {
        int index = 0;
        foreach (var icon in statusImages.Values)
        {
            icon.rectTransform.anchoredPosition = new Vector2(index * iconSize, 0);
            index++;
        }
    }

    private void UpdatePanelSize()
    {
        int activeEffects = statusImages.Count;
        panelRectTransform.sizeDelta = new Vector2(activeEffects * iconSize, panelRectTransform.sizeDelta.y);
    }
}