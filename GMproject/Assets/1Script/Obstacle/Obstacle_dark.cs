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
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와의 충돌을 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 플레이어의 Transform 참조
            playerTransform = collision.transform;

            // Mainlight의 기울기, 밝기, 색상 변경
            if (mainLight != null)
            {
                mainLight.transform.rotation = Quaternion.Euler(-30, -20, 30);

                Light mainLightComponent = mainLight.GetComponent<Light>();
                mainLightComponent.intensity = 0.01f; // 거의 꺼진 상태에 가깝게 밝기 줄임
                mainLightComponent.color = new Color(0.05f, 0.05f, 0.05f); // 매우 어두운 회색으로 색상 변경

                // 씬의 조명을 업데이트하여 변경 사항 반영
                DynamicGI.UpdateEnvironment();
            }

            // Ambient Light(환경광)을 완전히 제거
            RenderSettings.ambientIntensity = 0f;
            RenderSettings.ambientLight = Color.black;

            // 안개 효과 추가 (어두운 분위기 강화를 위해)
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.05f;

            // 플레이어 위치에 따라다니는 포인트 광원 생성
            currentPointLight = Instantiate(pointLightPrefab, playerTransform.position, Quaternion.identity);

            // 포인트 라이트가 플레이어를 따라가도록 설정하는 코루틴 시작
            StartCoroutine(FollowPlayer());

            // 5초 후 다시 Mainlight 복원 코루틴 실행
            StartCoroutine(RestoreMainLight());
        }
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

        // Mainlight가 존재하면 기울기와 밝기, 색상을 원래대로 복원
        if (mainLight != null)
        {
            mainLight.transform.rotation = Quaternion.Euler(50, -35, 350);

            Light mainLightComponent = mainLight.GetComponent<Light>();
            mainLightComponent.intensity = originalIntensity; // 원래 밝기로 복원
            mainLightComponent.color = originalColor; // 원래 색상으로 복원

            // 조명을 다시 업데이트하여 복원 효과 반영
            DynamicGI.UpdateEnvironment();
        }

        // Ambient Light(환경광) 강도와 색상도 원래대로 복원
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.ambientLight = originalAmbientColor;

        // 안개 효과 제거
        RenderSettings.fog = false;
    }
}