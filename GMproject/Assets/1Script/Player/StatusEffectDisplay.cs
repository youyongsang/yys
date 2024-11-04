using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusEffectPrefab
{
    public string effectName;       // ���� �̸� (��: "slow")
    public GameObject prefab;       // ���� ������ ������
}

public class StatusEffectDisplay : MonoBehaviour
{
    public List<StatusEffectPrefab> statusEffectPrefabsList; // Inspector���� ������ ���� ȿ�� ������ ����Ʈ
    private Dictionary<string, GameObject> statusIconPrefabs; // ���ο��� ����ϴ� Dictionary

    public Dictionary<string, float> statusEffects = new Dictionary<string, float>()
    {
        {"slow", 0f},
        {"dotDamage", 0f}
    };

    public Transform statusEffectPanel; // ���� �̻� �̹����� ǥ���� UI �г�
    private List<GameObject> activeIcons = new List<GameObject>(); // ���� Ȱ��ȭ�� ���� �̻� �̹�����

    // Start �޼��忡�� List �����͸� Dictionary�� ��ȯ
    void Start()
    {
        // List�� Dictionary�� ��ȯ�Ͽ� ���� �̸����� �������� ���� ã�� �� �ְ� ��
        statusIconPrefabs = new Dictionary<string, GameObject>();
        foreach (var item in statusEffectPrefabsList)
        {
            if (!statusIconPrefabs.ContainsKey(item.effectName))
            {
                statusIconPrefabs.Add(item.effectName, item.prefab);
            }
        }
    }

    // ���� �̻� �̹��� ������Ʈ �Լ�
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

    // ���� �̻� �̹����� Ȱ��ȭ�ϰų� �����ϴ� �Լ�
    void UpdateStatusEffectIcon(string effectName, float duration)
    {
        // ���� �̸��� �´� �������� �ִ��� Ȯ��
        if (!statusIconPrefabs.ContainsKey(effectName))
        {
            Debug.LogWarning("No icon prefab found for effect: " + effectName);
            return;
        }

        // ������ �����ϴ� �������� ���ٸ� ���ο� ������ ����
        if (!activeIcons.Exists(icon => icon.name == effectName))
        {
            GameObject newIcon = Instantiate(statusIconPrefabs[effectName], statusEffectPanel);
            newIcon.name = effectName;
            activeIcons.Add(newIcon);
        }

        // �������� �̹����� ȸ�� �ð�������� ����
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

    // ���� �̻� �̹��� ���� �Լ�
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
