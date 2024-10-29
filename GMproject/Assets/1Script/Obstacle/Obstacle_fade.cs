using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_fade : MonoBehaviour
{
    public float fadeDuration = 2f; // 투명해지는 데 걸리는 시간
    public float resetDelay = 3f; // 완전히 투명해진 후 복구되기까지의 대기 시간

    private Material obstacleMaterial;
    private Color originalColor;
    private Collider obstacleCollider;
    private bool isFading = false;

    void Start()
    {
        // 오브젝트의 재질과 원래 색상, Collider 컴포넌트를 저장합니다.
        obstacleMaterial = GetComponent<Renderer>().material;
        originalColor = obstacleMaterial.color;
        obstacleCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !isFading)
        {
            Debug.Log("페이드 아웃 시작");
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // 알파 값 계산하여 투명도를 점점 줄여나갑니다.
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            obstacleMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 완전히 투명해졌을 때 알파 값을 0으로 설정하고 충돌을 비활성화합니다.
        obstacleMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        obstacleCollider.enabled = false;

        // 지정된 시간 동안 대기 후 원래 상태로 복구합니다.
        yield return new WaitForSeconds(resetDelay);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // 알파 값을 점차 높여서 원래 불투명한 상태로 돌아갑니다.
            float alpha = Mathf.Lerp(0f, originalColor.a, elapsedTime / fadeDuration);
            obstacleMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 완전히 불투명해졌을 때 충돌을 다시 활성화합니다.
        obstacleMaterial.color = originalColor;
        obstacleCollider.enabled = true;
        isFading = false;
    }
}
