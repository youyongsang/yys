using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    private bool isJump = true;
    private Rigidbody rb;
    public Transform cameraTransform;

    // 이동 방향을 추적하는 변수
    [HideInInspector] public Vector3 lastMoveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 move = (cameraRight * moveX + cameraForward * moveZ).normalized;
        Vector3 velocity = rb.velocity;

        if (move.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

            // 이동 중인 방향을 업데이트
            lastMoveDirection = move;
        }
        else
        {
            velocity.x = 0f;
            velocity.z = 0f;
            rb.velocity = velocity;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.CompareTag("Plan"))
        {
            isJump = true;
        }

    }

    void OnCollisionExit()
    {
        isJump = false;
    }
}

