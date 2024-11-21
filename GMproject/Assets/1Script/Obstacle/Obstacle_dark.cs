using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_dark : MonoBehaviour
{
    public GameObject mainLight; // �������� (Mainlight)
    public GameObject pointLightPrefab; // ����Ʈ���� ������
    private GameObject currentPointLight; // ������ ����Ʈ ���� ������Ʈ
    private Transform playerTransform; // �÷��̾� ��ġ�� �����ϱ� ���� ����

    private float originalIntensity; // Mainlight�� ���� Intensity
    private Color originalColor; // Mainlight�� ���� ����
    private float originalAmbientIntensity; // Ambient Light ���� ����
    private Color originalAmbientColor; // Ambient Light ���� ����

    public AudioClip darkSound; // ����Ʈ ����Ʈ Ȱ��ȭ �� ����
    public AudioClip restoreSound; // ���� ���� ���� �� ����
    private AudioSource audioSource; // AudioSource ������Ʈ

    public float fadeDuration = 2f; // ��� ���� ���� �ð�

    private void Start()
    {
        // Mainlight�� Ambient Light�� ���� ���� �����ص�
        if (mainLight != null)
        {
            Light mainLightComponent = mainLight.GetComponent<Light>();
            originalIntensity = mainLightComponent.intensity;
            originalColor = mainLightComponent.color;
        }

        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalAmbientColor = RenderSettings.ambientLight;

        // AudioSource ������Ʈ �߰� �� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾���� �浹�� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾� Transform ����
            playerTransform = collision.transform;

            // �浹 ��� ���� ���
            if (darkSound != null && audioSource != null)
            {
                audioSource.clip = darkSound;
                audioSource.Play();
            }

            // ���������� ��ο����� �ڷ�ƾ ����
            StartCoroutine(DimLights());

            // �÷��̾� ��ġ�� ����ٴϴ� ����Ʈ ���� ����
            currentPointLight = Instantiate(pointLightPrefab, playerTransform.position, Quaternion.identity);

            // ����Ʈ ����Ʈ�� �÷��̾ ���󰡵��� �����ϴ� �ڷ�ƾ ����
            StartCoroutine(FollowPlayer());

            // 5�� �� ���� ���·� ����
            StartCoroutine(RestoreMainLight());
        }
    }

    private IEnumerator DimLights()
    {
        // Mainlight�� Ambient Light�� ���� ���� ������
        Light mainLightComponent = mainLight != null ? mainLight.GetComponent<Light>() : null;

        float startIntensity = mainLightComponent != null ? mainLightComponent.intensity : 1f;
        Color startColor = mainLightComponent != null ? mainLightComponent.color : Color.white;
        float startAmbientIntensity = RenderSettings.ambientIntensity;
        Color startAmbientColor = RenderSettings.ambientLight;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            // Mainlight ���� ��Ӱ�
            if (mainLightComponent != null)
            {
                mainLightComponent.intensity = Mathf.Lerp(startIntensity, 0.01f, t);
                mainLightComponent.color = Color.Lerp(startColor, new Color(0.05f, 0.05f, 0.05f), t);
            }

            // Ambient Light ���� ��Ӱ�
            RenderSettings.ambientIntensity = Mathf.Lerp(startAmbientIntensity, 0f, t);
            RenderSettings.ambientLight = Color.Lerp(startAmbientColor, Color.black, t);

            // �Ȱ��� ���� £����
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = Mathf.Lerp(0f, 0.05f, t);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ������ �ܰ迡�� �� Ȯ��
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
        // ����Ʈ ����Ʈ�� ������ ���� �÷��̾ ����ٴϰ� ��
        while (currentPointLight != null)
        {
            if (playerTransform != null)
            {
                // ����Ʈ ����Ʈ ��ġ�� �÷��̾� ��ġ + y�� 2��ŭ ���� ����
                currentPointLight.transform.position = playerTransform.position + new Vector3(0, 2, 0);
            }
            yield return null; // �� �����Ӹ��� ��ġ ������Ʈ
        }
    }

    private IEnumerator RestoreMainLight()
    {
        // 5�� ���
        yield return new WaitForSeconds(5);

        // ������ ����Ʈ ������ ������ ����
        if (currentPointLight != null)
        {
            Destroy(currentPointLight);
        }

        // ���� ���� ���������� ����
        Light mainLightComponent = mainLight != null ? mainLight.GetComponent<Light>() : null;

        float startIntensity = mainLightComponent != null ? mainLightComponent.intensity : 0.01f;
        Color startColor = mainLightComponent != null ? mainLightComponent.color : new Color(0.05f, 0.05f, 0.05f);
        float startAmbientIntensity = RenderSettings.ambientIntensity;
        Color startAmbientColor = RenderSettings.ambientLight;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            // Mainlight ���� �����
            if (mainLightComponent != null)
            {
                mainLightComponent.intensity = Mathf.Lerp(startIntensity, originalIntensity, t);
                mainLightComponent.color = Color.Lerp(startColor, originalColor, t);
            }

            // Ambient Light ���� �����
            RenderSettings.ambientIntensity = Mathf.Lerp(startAmbientIntensity, originalAmbientIntensity, t);
            RenderSettings.ambientLight = Color.Lerp(startAmbientColor, originalAmbientColor, t);

            // �Ȱ��� ���� �����
            RenderSettings.fogDensity = Mathf.Lerp(0.05f, 0f, t);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ������ �ܰ迡�� �� Ȯ��
        if (mainLightComponent != null)
        {
            mainLightComponent.intensity = originalIntensity;
            mainLightComponent.color = originalColor;
        }
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.ambientLight = originalAmbientColor;
        RenderSettings.fog = false;

        // ���� ���� ���
        if (restoreSound != null && audioSource != null)
        {
            audioSource.clip = restoreSound;
            audioSource.Play();
        }
    }
}
