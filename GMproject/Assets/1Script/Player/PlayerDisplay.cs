using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    public GameObject player;
    public Sprite heartSprite;
    public Sprite keySprite;
    public Font customFont; // ����� ��Ʈ�� Inspector���� ������ �� �ֵ��� ���� �߰�
    public Vector2 startPosition = new Vector2(0, 10);
    public float iconSpacing = 35f;
    public Vector2 keyPosition = new Vector2(-70, 10);

    private int playerLives;
    private List<Image> lifeImages = new List<Image>();
    private RectTransform canvasTransform;

    private int playerKeys;
    private Image keyImage;
    private Text keyCountText;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        PlayerStat playerstat = player.GetComponent<PlayerStat>();
        if (playerstat == null)
        {
            Debug.LogError("Player ������Ʈ�� PlayerStat ������Ʈ�� �����ϴ�!");
            return;
        }

        playerLives = playerstat.lives;
        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        for (int i = 0; i < playerLives; i++)
        {
            CreateLifeIcon(i);
        }

        playerKeys = playerstat.item_Key;
        CreateKeyIcon();
    }
    void Update()
    {
        // ��� ���� ������Ʈ
        int currentLives = player.GetComponent<PlayerStat>().lives;
        if (currentLives != playerLives)
        {
            UpdateLifeIcons(currentLives);
            playerLives = currentLives;
        }

        // Ű ���� ������Ʈ
        int currentKeys = player.GetComponent<PlayerStat>().item_Key;
        if (currentKeys != playerKeys)
        {
            UpdateKeyCount(currentKeys);
            playerKeys = currentKeys;
        }
    }

    void CreateLifeIcon(int index)
    {
        // ��� �̹��� ���� �� ����
        GameObject lifeObject = new GameObject("LifeIcon_" + index, typeof(Image));
        lifeObject.transform.SetParent(canvasTransform, false);
        Image lifeImage = lifeObject.GetComponent<Image>();
        lifeImage.sprite = heartSprite;
        lifeImage.rectTransform.sizeDelta = new Vector2(30, 30);
        lifeImage.rectTransform.anchorMin = new Vector2(0, 1);
        lifeImage.rectTransform.anchorMax = new Vector2(0, 1);
        lifeImage.rectTransform.pivot = new Vector2(0, 1);

        lifeImage.rectTransform.anchoredPosition = startPosition + new Vector2(iconSpacing * index, -10);
        lifeImages.Add(lifeImage);
    }

    void UpdateLifeIcons(int currentLives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }

    void CreateKeyIcon()
    {
        // Ű ������ ����
        GameObject keyObject = new GameObject("KeyIcon", typeof(Image));
        keyObject.transform.SetParent(canvasTransform, false);
        keyImage = keyObject.GetComponent<Image>();
        keyImage.sprite = keySprite;
        keyImage.rectTransform.sizeDelta = new Vector2(30, 30);
        keyImage.rectTransform.anchorMin = new Vector2(1, 1); // ���� ���
        keyImage.rectTransform.anchorMax = new Vector2(1, 1); // ���� ���
        keyImage.rectTransform.pivot = new Vector2(1, 1);     // ���� ��� ����
        keyImage.rectTransform.anchoredPosition = keyPosition;

        // Ű ���� �ؽ�Ʈ ����
        GameObject textObject = new GameObject("KeyCountText", typeof(Text));
        textObject.transform.SetParent(canvasTransform, false);
        keyCountText = textObject.GetComponent<Text>();
        keyCountText.text = "X " + playerKeys.ToString();
        keyCountText.font = customFont; // Inspector���� ������ ��Ʈ�� ���
        keyCountText.fontSize = 24;
        keyCountText.color = Color.white;
        keyCountText.alignment = TextAnchor.MiddleLeft;
        keyCountText.rectTransform.anchorMin = new Vector2(1, 1); // ���� ���
        keyCountText.rectTransform.anchorMax = new Vector2(1, 1); // ���� ���
        keyCountText.rectTransform.pivot = new Vector2(1, 1);     // ���� ��� ����

        // ���� �ٷ� �����ʿ� �ؽ�Ʈ ��ġ
        keyCountText.rectTransform.anchoredPosition = new Vector2(keyPosition.x + 110.0f, keyPosition.y + 40);
    }

    void UpdateKeyCount(int currentKeys)
    {
        // Ű ���� �ؽ�Ʈ ������Ʈ
        keyCountText.text = "X " + currentKeys.ToString();
    }
}
