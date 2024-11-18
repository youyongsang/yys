using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_space : MonoBehaviour
{
    PlayerMovement playerMovement = null;
    private float movespeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("���ο� ���� ����");
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            movespeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = movespeed * 0.3f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("���ο� ���� Ż��");
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            playerMovement.moveSpeed = movespeed;
        }
    }
}
