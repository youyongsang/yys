using System.Collections;
using System.Collections.Generic;
using System.Linq; // �� ���� �߰�
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusEffect
{
    public string name;       // ���� �̻� �̸�
    public Sprite icon;       // ���� �̻� ������
}

public class StatusEffectDisplay : MonoBehaviour
{
    public GameObject panel;                      // ���� �̻� �̹����� ǥ���� �г�
    public List<StatusEffect> statusEffects;      // ���� �̻� �̸� �� �������� �ν����Ϳ��� ����
    public int iconSize = 32;                     // ������ �⺻ ũ��

    public Dictionary<string, float> statusTimers = new Dictionary<string, float>(); // ���� �̻� �̸��� ���� �ð�
    private Dictionary<string, Image> statusImages = new Dictionary<string, Image>(); // ���� �̻� �̸��� �̹��� ������Ʈ
    private RectTransform panelRectTransform;      // �г��� RectTransform ����

    void Start()
    {
        if (panel == null)
        {
            Debug.LogError("�г��� �Ҵ���� �ʾҽ��ϴ�!");
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
        // ���� �̻� ���� �ð��� ��ȸ�ϸ� ����
        List<string> expiredEffects = new List<string>();
        var keys = statusTimers.Keys.ToList(); // ToList() �� ���� ȣ��
        
        foreach (var effectName in keys)
        {
            if (statusTimers.ContainsKey(effectName) && statusTimers[effectName] > 0)
            {
                statusTimers[effectName] -= Time.deltaTime;

                if (!statusImages.ContainsKey(effectName))
                {
                    CreateStatusIcon(effectName);
                }

                // �������� ������� �ִϸ��̼� (�ð�������� ����)
                if (statusImages.ContainsKey(effectName))
                {
                    Debug.Log("if (statusImages.ContainsKey(effectName)) ����Ȯ��");
                    UpdateStatusIcon(statusImages[effectName], statusTimers[effectName]);
                }
            }

            // ���� �ð��� 0 ���ϰ� �Ǹ� ������ �׸� �߰�
            if (statusTimers.ContainsKey(effectName) && statusTimers[effectName] <= 0)
            {
                expiredEffects.Add(effectName);
            }
        }

        // ����� ���� �̻� ����
        foreach (var effectName in expiredEffects)
        {
            if (statusImages.ContainsKey(effectName))
            {
                Destroy(statusImages[effectName].gameObject);
                statusImages.Remove(effectName);
            }
            if (statusTimers.ContainsKey(effectName))
            {
                statusTimers.Remove(effectName); // ��ųʸ����� �����̻� ����
            }
        }

        // ���� �̻� ������ ������
        ReorderIcons();
    }
    private Dictionary<string, float> statusMaxTimers = new Dictionary<string, float>(); // �ʱ� �ִ� ���� �ð�
    public void SetStatusEffect(string effectName, float duration)
    {
        if (statusTimers.ContainsKey(effectName))
        {
            statusTimers[effectName] = duration;
            if (!statusMaxTimers.ContainsKey(effectName))
            {
                statusMaxTimers[effectName] = duration; // �ʱ� ���� �ð� ����
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
            float maxTime = statusMaxTimers[iconImage.name]; // �ʱ� �ִ� �ð� ����
            float percentage = Mathf.Clamp01(remainingTime / maxTime);
            Debug.Log(percentage);
            iconImage.fillMethod = Image.FillMethod.Radial360; // Radial360 ������� ����
            iconImage.fillClockwise = true;                    // �ð�������� ����
            iconImage.fillOrigin = (int)Image.Origin360.Top;    // ���ʿ��� ����
            iconImage.fillAmount = percentage;                 // fillAmount ����

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