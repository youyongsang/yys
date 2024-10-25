using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    public GameObject player;          // Player 오브젝트
    public Sprite heartSprite;         // 목숨을 나타내는 이미지 스프라이트
    public Vector2 startPosition = new Vector2(10, 10); // GUI 시작 위치 (좌측 상단)
    public float iconSpacing = 35f;    // 목숨 아이콘 사이 간격

    private int playerLives;
    private List<Image> lifeImages = new List<Image>(); // 이미지 관리용 리스트
    private RectTransform canvasTransform;

    void Start()
    {

        if (player == null)
        {
            Debug.LogError("Player 오브젝트가 할당되지 않았습니다!");
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("Player 오브젝트에 PlayerHealth 컴포넌트가 없습니다!");
            return;
        }
        // Canvas 및 Player의 Health 컴포넌트 가져오기
        playerLives = player.GetComponent<PlayerHealth>().lives;
        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        for (int i = 0; i < playerLives; i++)
        {
            CreateLifeIcon(i);
        }
    }

    void Update()
    {
        // 플레이어의 현재 목숨 수 확인
        int currentLives = player.GetComponent<PlayerHealth>().lives;

        // 목숨 수가 변하면 갱신
        if (currentLives != playerLives)
        {
            UpdateLifeIcons(currentLives);
            playerLives = currentLives;
        }
    }

    void CreateLifeIcon(int index)
    {
        // Image 생성 및 초기 설정
        GameObject lifeObject = new GameObject("LifeIcon_" + index, typeof(Image));
        lifeObject.transform.SetParent(canvasTransform, false);
        Image lifeImage = lifeObject.GetComponent<Image>();
        lifeImage.sprite = heartSprite;
        lifeImage.rectTransform.sizeDelta = new Vector2(30, 30);

        lifeImage.rectTransform.anchorMin = new Vector2(0, 1); // 좌측 상단
        lifeImage.rectTransform.anchorMax = new Vector2(0, 1); // 좌측 상단
        lifeImage.rectTransform.pivot = new Vector2(0, 1);     // 좌측 상단 기준

        // 좌측 상단에 하트 이미지 위치 배치
        lifeImage.rectTransform.anchoredPosition = startPosition + new Vector2(iconSpacing * index, -10);
        lifeImages.Add(lifeImage);
    }

    void UpdateLifeIcons(int currentLives)
    {
        // 기존 아이콘을 숨기거나 활성화
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }
}
