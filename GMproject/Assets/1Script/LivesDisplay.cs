using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    public GameObject player;          // Player ������Ʈ
    public Sprite heartSprite;         // ����� ��Ÿ���� �̹��� ��������Ʈ
    public Vector2 startPosition = new Vector2(10, 10); // GUI ���� ��ġ (���� ���)
    public float iconSpacing = 35f;    // ��� ������ ���� ����

    private int playerLives;
    private List<Image> lifeImages = new List<Image>(); // �̹��� ������ ����Ʈ
    private RectTransform canvasTransform;

    void Start()
    {

        if (player == null)
        {
            Debug.LogError("Player ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("Player ������Ʈ�� PlayerHealth ������Ʈ�� �����ϴ�!");
            return;
        }
        // Canvas �� Player�� Health ������Ʈ ��������
        playerLives = player.GetComponent<PlayerHealth>().lives;
        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        for (int i = 0; i < playerLives; i++)
        {
            CreateLifeIcon(i);
        }
    }

    void Update()
    {
        // �÷��̾��� ���� ��� �� Ȯ��
        int currentLives = player.GetComponent<PlayerHealth>().lives;

        // ��� ���� ���ϸ� ����
        if (currentLives != playerLives)
        {
            UpdateLifeIcons(currentLives);
            playerLives = currentLives;
        }
    }

    void CreateLifeIcon(int index)
    {
        // Image ���� �� �ʱ� ����
        GameObject lifeObject = new GameObject("LifeIcon_" + index, typeof(Image));
        lifeObject.transform.SetParent(canvasTransform, false);
        Image lifeImage = lifeObject.GetComponent<Image>();
        lifeImage.sprite = heartSprite;
        lifeImage.rectTransform.sizeDelta = new Vector2(30, 30);

        lifeImage.rectTransform.anchorMin = new Vector2(0, 1); // ���� ���
        lifeImage.rectTransform.anchorMax = new Vector2(0, 1); // ���� ���
        lifeImage.rectTransform.pivot = new Vector2(0, 1);     // ���� ��� ����

        // ���� ��ܿ� ��Ʈ �̹��� ��ġ ��ġ
        lifeImage.rectTransform.anchoredPosition = startPosition + new Vector2(iconSpacing * index, -10);
        lifeImages.Add(lifeImage);
    }

    void UpdateLifeIcons(int currentLives)
    {
        // ���� �������� ����ų� Ȱ��ȭ
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }
}
