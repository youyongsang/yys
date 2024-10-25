using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;            // �÷��̾��� Transform
    public float distance = 5f;         // �÷��̾�� ī�޶� ������ �Ÿ�
    public float height = 3f;           // ī�޶��� ����
    public float rotationSpeed = 100f;  // ī�޶� ȸ�� �ӵ�

    private Vector3 offset;

    void Start()
    {
        // �÷��̾�� ī�޶� ������ �⺻ ������ ����
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ ����
        transform.position = player.position + offset;

        // Q�� E�� ������ �� ī�޶� ȸ��
        if (Input.GetKey(KeyCode.Q))
        {
            RotateAroundPlayer(true);
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateAroundPlayer(false);
        }

        // �÷��̾ �׻� �ٶ󺸰� ����
        transform.LookAt(player);
    }

    void RotateAroundPlayer(bool clockwise)
    {
        float rotationDirection = clockwise ? 1f : -1f;
        // �÷��̾� ������ �ð���� �Ǵ� �ݽð�������� ȸ��
        offset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime * rotationDirection, Vector3.up) * offset;
    }
}