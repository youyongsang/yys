using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_fade : MonoBehaviour
{
    public float fadeDuration = 2f; // ���������� �� �ɸ��� �ð�
    public float resetDelay = 3f; // ������ �������� �� �����Ǳ������ ��� �ð�

    private Material obstacleMaterial;
    private Color originalColor;
    private Collider obstacleCollider;
    private bool isFading = false;

    void Start()
    {
        // ������Ʈ�� ������ ���� ����, Collider ������Ʈ�� �����մϴ�.
        obstacleMaterial = GetComponent<Renderer>().material;
        originalColor = obstacleMaterial.color;
        obstacleCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !isFading)
        {
            Debug.Log("���̵� �ƿ� ����");
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // ���� �� ����Ͽ� ������ ���� �ٿ������ϴ�.
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            obstacleMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������ ���������� �� ���� ���� 0���� �����ϰ� �浹�� ��Ȱ��ȭ�մϴ�.
        obstacleMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        obstacleCollider.enabled = false;

        // ������ �ð� ���� ��� �� ���� ���·� �����մϴ�.
        yield return new WaitForSeconds(resetDelay);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // ���� ���� ���� ������ ���� �������� ���·� ���ư��ϴ�.
            float alpha = Mathf.Lerp(0f, originalColor.a, elapsedTime / fadeDuration);
            obstacleMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������ ������������ �� �浹�� �ٽ� Ȱ��ȭ�մϴ�.
        obstacleMaterial.color = originalColor;
        obstacleCollider.enabled = true;
        isFading = false;
    }
}
