using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    public GameObject player;
    public Sprite heartSprite;
    public Sprite keySprite;
    public Font customFont; // 사용할 폰트를 Inspector에서 지정할 수 있도록 변수 추가
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
        // 목숨 갯수 업데이트
        int currentLives = player.GetComponent<PlayerStat>().lives;
        if (currentLives != playerLives)
        {
            UpdateLifeIcons(currentLives);
            playerLives = currentLives;
        }

        // 키 갯수 업데이트
        int currentKeys = player.GetComponent<PlayerStat>().item_Key;
        if (currentKeys != playerKeys)
        {
            UpdateKeyCount(currentKeys);
            playerKeys = currentKeys;
        }
    }

    void CreateLifeIcon(int index)
    {
        // 목숨 이미지 생성 및 설정
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
        // 키 아이콘 생성
        GameObject keyObject = new GameObject("KeyIcon", typeof(Image));
        keyObject.transform.SetParent(canvasTransform, false);
        keyImage = keyObject.GetComponent<Image>();
        keyImage.sprite = keySprite;
        keyImage.rectTransform.sizeDelta = new Vector2(30, 30);
        keyImage.rectTransform.anchorMin = new Vector2(1, 1); // 우측 상단
        keyImage.rectTransform.anchorMax = new Vector2(1, 1); // 우측 상단
        keyImage.rectTransform.pivot = new Vector2(1, 1);     // 우측 상단 기준
        keyImage.rectTransform.anchoredPosition = keyPosition;

        // 키 갯수 텍스트 생성
        GameObject textObject = new GameObject("KeyCountText", typeof(Text));
        textObject.transform.SetParent(canvasTransform, false);
        keyCountText = textObject.GetComponent<Text>();
        keyCountText.text = "X " + playerKeys.ToString();
        keyCountText.font = customFont; // Inspector에서 지정한 폰트를 사용
        keyCountText.fontSize = 24;
        keyCountText.color = Color.white;
        keyCountText.alignment = TextAnchor.MiddleLeft;
        keyCountText.rectTransform.anchorMin = new Vector2(1, 1); // 우측 상단
        keyCountText.rectTransform.anchorMax = new Vector2(1, 1); // 우측 상단
        keyCountText.rectTransform.pivot = new Vector2(1, 1);     // 우측 상단 기준

        // 열쇠 바로 오른쪽에 텍스트 배치
        keyCountText.rectTransform.anchoredPosition = new Vector2(keyPosition.x + 110.0f, keyPosition.y + 40);
    }

    void UpdateKeyCount(int currentKeys)
    {
        // 키 갯수 텍스트 업데이트
        keyCountText.text = "X " + currentKeys.ToString();
    }
}
