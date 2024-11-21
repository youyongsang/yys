using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_dark : MonoBehaviour
{
    public GameObject mainLight; // 직선광원 (Mainlight)
    public GameObject pointLightPrefab; // 포인트광원 프리팹
    private GameObject currentPointLight; // 생성된 포인트 광원 오브젝트
    private Transform playerTransform; // 플레이어 위치를 참조하기 위한 변수

    private float originalIntensity; // Mainlight의 원래 Intensity
    private Color originalColor; // Mainlight의 원래 색상
    private float originalAmbientIntensity; // Ambient Light 원래 강도
    private Color originalAmbientColor; // Ambient Light 원래 색상

    public AudioClip darkSound; // 포인트 라이트 활성화 시 사운드
    public AudioClip restoreSound; // 원래 광원 복원 시 사운드
    private AudioSource audioSource; // AudioSource 컴포넌트

    public float fadeDuration = 2f; // 밝기 변경 지속 시간

    private void Start()
    {
        // Mainlight와 Ambient Light의 원래 값을 저장해둠
        if (mainLight != null)
        {
            Light mainLightComponent = mainLight.GetComponent<Light>();
            originalIntensity = mainLightComponent.intensity;
            originalColor = mainLightComponent.color;
        }

        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalAmbientColor = RenderSettings.ambientLight;

        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와의 충돌을 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어 Transform 참조
            playerTransform = collision.transform;

            // 충돌 즉시 사운드 재생
            if (darkSound != null && audioSource != null)
            {
                audioSource.clip = darkSound;
                audioSource.Play();
            }

            // 점진적으로 어두워지는 코루틴 실행
            StartCoroutine(DimLights());

            // 플레이어 위치에 따라다니는 포인트 광원 생성
            currentPointLight = Instantiate(pointLightPrefab, playerTransform.position, Quaternion.identity);

            // 포인트 라이트가 플레이어를 따라가도록 설정하는 코루틴 시작
            StartCoroutine(FollowPlayer());

            // 5초 후 원래 상태로 복원
            StartCoroutine(RestoreMainLight());
        }
    }

    private IEnumerator DimLights()
    {
        // Mainlight와 Ambient Light의 현재 값을 가져옴
        Light mainLightComponent = mainLight != null ? mainLight.GetComponent<Light>() : null;

        float startIntensity = mainLightComponent != null ? mainLightComponent.intensity : 1f;
        Color startColor = mainLightComponent != null ? mainLightComponent.color : Color.white;
        float startAmbientIntensity = RenderSettings.ambientIntensity;
        Color startAmbientColor = RenderSettings.ambientLight;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            // Mainlight 점점 어둡게
            if (mainLightComponent != null)
            {
                mainLightComponent.intensity = Mathf.Lerp(startIntensity, 0.01f, t);
                mainLightComponent.color = Color.Lerp(startColor, new Color(0.05f, 0.05f, 0.05f), t);
            }

            // Ambient Light 점점 어둡게
            RenderSettings.ambientIntensity = Mathf.Lerp(startAmbientIntensity, 0f, t);
            RenderSettings.ambientLight = Color.Lerp(startAmbientColor, Color.black, t);

            // 안개도 점점 짙어짐
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = Mathf.Lerp(0f, 0.05f, t);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막 단계에서 값 확정
        if (mainLightComponent != null)
        {
            mainLightComponent.intensity = 0.01f;
            mainLightComponent.color = new Color(0.05f, 0.05f, 0.05f);
        }
        RenderSettings.ambientIntensity = 0f;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.fogDensity = 0.05f;
    }

    private IEnumerator FollowPlayer()
    {
        // 포인트 라이트가 생성된 동안 플레이어를 따라다니게 함
        while (currentPointLight != null)
        {
            if (playerTransform != null)
            {
                // 포인트 라이트 위치를 플레이어 위치 + y축 2만큼 위로 설정
                currentPointLight.transform.position = playerTransform.position + new Vector3(0, 2, 0);
            }
            yield return null; // 매 프레임마다 위치 업데이트
        }
    }

    private IEnumerator RestoreMainLight()
    {
        // 5초 대기
        yield return new WaitForSeconds(5);

        // 생성된 포인트 광원이 있으면 제거
        if (currentPointLight != null)
        {
            Destroy(currentPointLight);
        }

        // 원래 밝기로 점진적으로 복원
        Light mainLightComponent = mainLight != null ? mainLight.GetComponent<Light>() : null;

        float startIntensity = mainLightComponent != null ? mainLightComponent.intensity : 0.01f;
        Color startColor = mainLightComponent != null ? mainLightComponent.color : new Color(0.05f, 0.05f, 0.05f);
        float startAmbientIntensity = RenderSettings.ambientIntensity;
        Color startAmbientColor = RenderSettings.ambientLight;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            // Mainlight 점점 밝아짐
            if (mainLightComponent != null)
            {
                mainLightComponent.intensity = Mathf.Lerp(startIntensity, originalIntensity, t);
                mainLightComponent.color = Color.Lerp(startColor, originalColor, t);
            }

            // Ambient Light 점점 밝아짐
            RenderSettings.ambientIntensity = Mathf.Lerp(startAmbientIntensity, originalAmbientIntensity, t);
            RenderSettings.ambientLight = Color.Lerp(startAmbientColor, originalAmbientColor, t);

            // 안개도 점점 사라짐
            RenderSettings.fogDensity = Mathf.Lerp(0.05f, 0f, t);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막 단계에서 값 확정
        if (mainLightComponent != null)
        {
            mainLightComponent.intensity = originalIntensity;
            mainLightComponent.color = originalColor;
        }
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.ambientLight = originalAmbientColor;
        RenderSettings.fog = false;

        // 복원 사운드 재생
        if (restoreSound != null && audioSource != null)
        {
            audioSource.clip = restoreSound;
            audioSource.Play();
        }
    }
}
