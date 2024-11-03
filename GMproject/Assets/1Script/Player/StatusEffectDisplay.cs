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

    public GameObject statusIconPrefab; // ���� �̻� �̹����� ���� ������
    public Transform statusEffectPanel; // ���� �̻� �̹����� ǥ���� UI �г�
    private List<GameObject> activeIcons = new List<GameObject>(); // ���� Ȱ��ȭ�� ���� �̻� �̹�����

    // ���� �̻� �̹��� ������Ʈ �Լ�
    void Update()
    {
        // ��� ���� �̻� ��ųʸ��� Ȯ���Ͽ� ���¿� �´� �̹����� ������Ʈ
        foreach (var effect in statusEffects)
        {
            // ���� �ð��� 0���� ū ��쿡�� ó��
            if (effect.Value > 0)
            {
                // �ش� ���¿� �´� �̹����� Ȱ��ȭ �� ������Ʈ
                UpdateStatusEffectIcon(effect.Key, effect.Value);

                // �ð��� ����Կ� ���� ���� ���� �ð� ����
                statusEffects[effect.Key] -= Time.deltaTime;
            }
            else
            {
                // ���� �ð��� 0�̰ų� �� ���ϰ� �Ǹ� �̹����� ����
                RemoveStatusEffectIcon(effect.Key);
            }
        }

        // ���°� ���� �� �̹������� ������ ���
        RearrangeIcons();
    }

    // ���� �̻� �̹����� Ȱ��ȭ�ϰų� �����ϴ� �Լ�
    void UpdateStatusEffectIcon(string effectName, float duration)
    {
        // ������ �����ϴ� �������� ���ٸ� ���ο� ������ ����
        if (!activeIcons.Exists(icon => icon.name == effectName))
        {
            GameObject newIcon = Instantiate(statusIconPrefab, statusEffectPanel);
            newIcon.name = effectName;
            activeIcons.Add(newIcon);
        }

        // �������� �̹����� ȸ�� �ð�������� ����
        GameObject iconObject = activeIcons.Find(icon => icon.name == effectName);
        Image iconImage = iconObject.GetComponent<Image>();
        iconImage.fillAmount = duration / 3.0f; // 3�ʸ� �������� fillAmount�� ����
    }

    // ���� �̻� �̹��� ���� �Լ�
    void RemoveStatusEffectIcon(string effectName)
    {
        GameObject iconObject = activeIcons.Find(icon => icon.name == effectName);
        if (iconObject != null)
        {
            Destroy(iconObject);
            activeIcons.Remove(iconObject);
        }
        // ���� ���� �ð��� 0���� �ʱ�ȭ
        statusEffects[effectName] = 0;
    }

    // ��� �����ܵ��� ������ ��� ���ġ
    void RearrangeIcons()
    {
        for (int i = 0; i < activeIcons.Count; i++)
        {
            RectTransform iconTransform = activeIcons[i].GetComponent<RectTransform>();
            iconTransform.anchoredPosition = new Vector2(-i * 50, 0); // �������� ��ġ
        }
    }
}
