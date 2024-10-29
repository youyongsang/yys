using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;            // 플레이어의 Transform
    public float distance = 5f;         // 플레이어와 카메라 사이의 거리
    public float height = 3f;           // 카메라의 높이
    public float rotationSpeed = 100f;  // 카메라 회전 속도

    private Vector3 offset;

    void Start()
    {
        // 플레이어와 카메라 사이의 기본 오프셋 설정
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        // 플레이어의 위치에 따라 카메라 위치 조정
        transform.position = player.position + offset;

        // Q와 E를 눌렀을 때 카메라 회전
        if (Input.GetKey(KeyCode.Q))
        {
            RotateAroundPlayer(true);
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateAroundPlayer(false);
        }

        // 플레이어를 항상 바라보게 설정
        transform.LookAt(player);
    }

    void RotateAroundPlayer(bool clockwise)
    {
        float rotationDirection = clockwise ? 1f : -1f;
        // 플레이어 주위를 시계방향 또는 반시계방향으로 회전
        offset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime * rotationDirection, Vector3.up) * offset;
    }
}