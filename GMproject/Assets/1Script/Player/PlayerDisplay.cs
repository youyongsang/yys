using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    public GameObject player;
    public Sprite heartSprite; // �⺻ ��Ʈ �̹���
    public Sprite poisonHeartSprite; // �� ���� ��Ʈ �̹���
    public Sprite shieldSprite; // �ǵ� ��������Ʈ
    public Sprite keySprite; // ���� ��������Ʈ
    public Font customFont; // ����� ��Ʈ
    public Vector2 startPosition = new Vector2(0, 10);
    public float iconSpacing = 35f;
    public Vector2 keyPosition = new Vector2(-70, 10);
    public Vector2 shieldPositionOffset = new Vector2(0, -40);

    public AudioClip shieldDecreaseSound; // �ǵ� ���� ȿ����
    private AudioSource audioSource; // AudioSource ������Ʈ

    private int playerLives;
    private List<Image> lifeImages = new List<Image>();
    private RectTransform canvasTransform;

    private int playerKeys;
    private Image keyImage;
    private Text keyCountText;

    private int playerShield;
    private List<Image> shieldImages = new List<Image>();

    private bool isPoisoned = false;

    private Text scoreText; // ������ ǥ���� �ؽ�Ʈ
    private int currentScore = 0; // ���� ����

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
        playerKeys = playerstat.item_Key;
        playerShield = playerstat.shield;

        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // AudioSource ������Ʈ �߰�
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // ���� �ؽ�Ʈ ����
        CreateScoreText();

        // ��� ������ ����
        for (int i = 0; i < playerLives; i++)
        {
            CreateLifeIcon(i);
        }

        // ���� ������ ����
        CreateKeyIcon();

        // �ǵ� ������ ����
        UpdateShieldIcons(playerShield);
    }

    void Update()
    {
        PlayerStat playerStat = player.GetComponent<PlayerStat>();

        // ��� ���� ������Ʈ
        int currentLives = playerStat.lives;
        if (currentLives != playerLives)
        {
            UpdateLifeIcons(currentLives);
            playerLives = currentLives;
        }

        // ���� ���� ������Ʈ
        int currentKeys = playerStat.item_Key;
        if (currentKeys != playerKeys)
        {
            UpdateKeyCount(currentKeys);
            playerKeys = currentKeys;
        }

        // �ǵ� ���� ������Ʈ
        int currentShield = playerStat.shield;
        if (currentShield != playerShield)
        {
            // �ǵ� ���� �� ȿ���� ���
            if (currentShield < playerShield && shieldDecreaseSound != null)
            {
                Debug.Log("���� ����ȿ���� �߻�");
                audioSource.clip = shieldDecreaseSound;
                audioSource.Play();
            }

            UpdateShieldIcons(currentShield);
            playerShield = currentShield;
        }

        // ���� ������Ʈ
        int score = playerStat.score; // PlayerStat���� ������ ������
        if (score != currentScore)
        {
            UpdateScoreText(score);
            currentScore = score;
        }

        // �� ���� ������Ʈ
        UpdatePoisonedIcons();
    }

    void CreateScoreText()
    {
        GameObject scoreObject = new GameObject("ScoreText", typeof(Text));
        scoreObject.transform.SetParent(canvasTransform, false);

        scoreText = scoreObject.GetComponent<Text>();
        scoreText.font = customFont;
        scoreText.fontSize = 36;
        scoreText.color = Color.white;
        scoreText.alignment = TextAnchor.MiddleCenter;

        // ȭ�� ��� ��ܿ� ��ġ
        RectTransform rectTransform = scoreText.rectTransform;
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0, 30); // ȭ�� ������ �ణ �Ʒ��� ��ġ
        scoreText.text = "0";
    }

    void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    void CreateLifeIcon(int index)
    {
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
        // ��� ������ ���� ������ Ȱ��ȭ/��Ȱ��ȭ
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }

    public void SetPoisonState(bool poisoned)
    {
        isPoisoned = poisoned;
    }

    private void UpdatePoisonedIcons()
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (isPoisoned)
            {
                // �� ���¸� �� ��Ʈ �̹����� ����
                lifeImages[i].sprite = poisonHeartSprite;
            }
            else
            {
                // �� ���°� �ƴϸ� �⺻ ��Ʈ �̹����� ����
                lifeImages[i].sprite = heartSprite;
            }
        }
    }

    void CreateKeyIcon()
    {
        GameObject keyObject = new GameObject("KeyIcon", typeof(Image));
        keyObject.transform.SetParent(canvasTransform, false);
        keyImage = keyObject.GetComponent<Image>();
        keyImage.sprite = keySprite;
        keyImage.rectTransform.sizeDelta = new Vector2(30, 30);
        keyImage.rectTransform.anchorMin = new Vector2(1, 1);
        keyImage.rectTransform.anchorMax = new Vector2(1, 1);
        keyImage.rectTransform.pivot = new Vector2(1, 1);
        keyImage.rectTransform.anchoredPosition = keyPosition;

        GameObject textObject = new GameObject("KeyCountText", typeof(Text));
        textObject.transform.SetParent(canvasTransform, false);
        keyCountText = textObject.GetComponent<Text>();
        keyCountText.text = "X " + playerKeys.ToString();
        keyCountText.font = customFont;
        keyCountText.fontSize = 24;
        keyCountText.color = Color.white;
        keyCountText.alignment = TextAnchor.MiddleLeft;
        keyCountText.rectTransform.anchorMin = new Vector2(1, 1);
        keyCountText.rectTransform.anchorMax = new Vector2(1, 1);
        keyCountText.rectTransform.pivot = new Vector2(1, 1);
        keyCountText.rectTransform.anchoredPosition = new Vector2(keyPosition.x + 110.0f, keyPosition.y + 40);
    }

    void UpdateKeyCount(int currentKeys)
    {
        keyCountText.text = "X " + currentKeys.ToString();
    }

    void CreateShieldIcon(int index)
    {
        GameObject shieldObject = new GameObject("ShieldIcon_" + index, typeof(Image));
        shieldObject.transform.SetParent(canvasTransform, false);
        Image shieldImage = shieldObject.GetComponent<Image>();
        shieldImage.sprite = shieldSprite;
        shieldImage.rectTransform.sizeDelta = new Vector2(30, 30);
        shieldImage.rectTransform.anchorMin = new Vector2(0, 1);
        shieldImage.rectTransform.anchorMax = new Vector2(0, 1);
        shieldImage.rectTransform.pivot = new Vector2(0, 1);
        shieldImage.rectTransform.anchoredPosition = startPosition + shieldPositionOffset + new Vector2(iconSpacing * index, -10);
        shieldImages.Add(shieldImage);
    }

    void UpdateShieldIcons(int currentShield)
    {
        // �ʿ��� �ǵ� ������ ����
        if (shieldImages.Count < currentShield)
        {
            for (int i = shieldImages.Count; i < currentShield; i++)
            {
                CreateShieldIcon(i);
            }
        }

        // �ǵ� Ȱ��ȭ/��Ȱ��ȭ
        for (int i = 0; i < shieldImages.Count; i++)
        {
            shieldImages[i].enabled = i < currentShield;
        }
    }

    public bool IsPoisoned()
    {
        return isPoisoned;
    }
}
