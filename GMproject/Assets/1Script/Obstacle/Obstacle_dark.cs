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
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾���� �浹�� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �浹�� �÷��̾��� Transform ����
            playerTransform = collision.transform;

            // Mainlight�� ����, ���, ���� ����
            if (mainLight != null)
            {
                mainLight.transform.rotation = Quaternion.Euler(-30, -20, 30);

                Light mainLightComponent = mainLight.GetComponent<Light>();
                mainLightComponent.intensity = 0.01f; // ���� ���� ���¿� ������ ��� ����
                mainLightComponent.color = new Color(0.05f, 0.05f, 0.05f); // �ſ� ��ο� ȸ������ ���� ����

                // ���� ������ ������Ʈ�Ͽ� ���� ���� �ݿ�
                DynamicGI.UpdateEnvironment();
            }

            // Ambient Light(ȯ�汤)�� ������ ����
            RenderSettings.ambientIntensity = 0f;
            RenderSettings.ambientLight = Color.black;

            // �Ȱ� ȿ�� �߰� (��ο� ������ ��ȭ�� ����)
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.05f;

            // �÷��̾� ��ġ�� ����ٴϴ� ����Ʈ ���� ����
            currentPointLight = Instantiate(pointLightPrefab, playerTransform.position, Quaternion.identity);

            // ����Ʈ ����Ʈ�� �÷��̾ ���󰡵��� �����ϴ� �ڷ�ƾ ����
            StartCoroutine(FollowPlayer());

            // 5�� �� �ٽ� Mainlight ���� �ڷ�ƾ ����
            StartCoroutine(RestoreMainLight());
        }
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

        // Mainlight�� �����ϸ� ����� ���, ������ ������� ����
        if (mainLight != null)
        {
            mainLight.transform.rotation = Quaternion.Euler(50, -35, 350);

            Light mainLightComponent = mainLight.GetComponent<Light>();
            mainLightComponent.intensity = originalIntensity; // ���� ���� ����
            mainLightComponent.color = originalColor; // ���� �������� ����

            // ������ �ٽ� ������Ʈ�Ͽ� ���� ȿ�� �ݿ�
            DynamicGI.UpdateEnvironment();
        }

        // Ambient Light(ȯ�汤) ������ ���� ������� ����
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.ambientLight = originalAmbientColor;

        // �Ȱ� ȿ�� ����
        RenderSettings.fog = false;
    }
}