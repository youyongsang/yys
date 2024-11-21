using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    public GameObject player;
    public Sprite heartSprite; // 기본 하트 이미지
    public Sprite poisonHeartSprite; // 독 상태 하트 이미지
    public Sprite shieldSprite; // 실드 스프라이트
    public Sprite keySprite; // 열쇠 스프라이트
    public Font customFont; // 사용할 폰트
    public Vector2 startPosition = new Vector2(0, 10);
    public float iconSpacing = 35f;
    public Vector2 keyPosition = new Vector2(-70, 10);
    public Vector2 shieldPositionOffset = new Vector2(0, -40);

    public AudioClip shieldDecreaseSound; // 실드 감소 효과음
    private AudioSource audioSource; // AudioSource 컴포넌트

    private int playerLives;
    private List<Image> lifeImages = new List<Image>();
    private RectTransform canvasTransform;

    private int playerKeys;
    private Image keyImage;
    private Text keyCountText;

    private int playerShield;
    private List<Image> shieldImages = new List<Image>();

    private bool isPoisoned = false;

    private Text scoreText; // 점수를 표시할 텍스트
    private int currentScore = 0; // 현재 점수

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player 오브젝트가 할당되지 않았습니다!");
            return;
        }

        PlayerStat playerstat = player.GetComponent<PlayerStat>();
        if (playerstat == null)
        {
            Debug.LogError("Player 오브젝트에 PlayerStat 컴포넌트가 없습니다!");
            return;
        }

        playerLives = playerstat.lives;
        playerKeys = playerstat.item_Key;
        playerShield = playerstat.shield;

        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // AudioSource 컴포넌트 추가
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // 점수 텍스트 생성
        CreateScoreText();

        // 목숨 아이콘 생성
        for (int i = 0; i < playerLives; i++)
        {
            CreateLifeIcon(i);
        }

        // 열쇠 아이콘 생성
        CreateKeyIcon();

        // 실드 아이콘 생성
        UpdateShieldIcons(playerShield);
    }

    void Update()
    {
        PlayerStat playerStat = player.GetComponent<PlayerStat>();

        // 목숨 갯수 업데이트
        int currentLives = playerStat.lives;
        if (currentLives != playerLives)
        {
            UpdateLifeIcons(currentLives);
            playerLives = currentLives;
        }

        // 열쇠 갯수 업데이트
        int currentKeys = playerStat.item_Key;
        if (currentKeys != playerKeys)
        {
            UpdateKeyCount(currentKeys);
            playerKeys = currentKeys;
        }

        // 실드 갯수 업데이트
        int currentShield = playerStat.shield;
        if (currentShield != playerShield)
        {
            // 실드 감소 시 효과음 재생
            if (currentShield < playerShield && shieldDecreaseSound != null)
            {
                Debug.Log("쉴드 감소효과음 발생");
                audioSource.clip = shieldDecreaseSound;
                audioSource.Play();
            }

            UpdateShieldIcons(currentShield);
            playerShield = currentShield;
        }

        // 점수 업데이트
        int score = playerStat.score; // PlayerStat에서 점수를 가져옴
        if (score != currentScore)
        {
            UpdateScoreText(score);
            currentScore = score;
        }

        // 독 상태 업데이트
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

        // 화면 가운데 상단에 배치
        RectTransform rectTransform = scoreText.rectTransform;
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0, 30); // 화면 위에서 약간 아래로 위치
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
        // 목숨 갯수에 따라 아이콘 활성화/비활성화
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
                // 독 상태면 독 하트 이미지로 변경
                lifeImages[i].sprite = poisonHeartSprite;
            }
            else
            {
                // 독 상태가 아니면 기본 하트 이미지로 변경
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
        // 필요한 실드 아이콘 생성
        if (shieldImages.Count < currentShield)
        {
            for (int i = shieldImages.Count; i < currentShield; i++)
            {
                CreateShieldIcon(i);
            }
        }

        // 실드 활성화/비활성화
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
