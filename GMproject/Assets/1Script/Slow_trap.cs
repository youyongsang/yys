using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_trap : MonoBehaviour
{
    PlayerMovement playerMovement = null;
    bool OnCo = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnCo == true)
        {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            float movespeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed = movespeed*0.1f;
            StartCoroutine(Speed(movespeed));
            OnCo = false;
        }
    }

    IEnumerator Speed(float movespeed)
    {
        Debug.Log("코루틴 실행 지남");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("3초 지남");
        playerMovement.moveSpeed = movespeed;
        OnCo = true;
    }

}
