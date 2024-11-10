using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        PlayerStat Playerstat = collision.gameObject.GetComponent<PlayerStat>();
        Debug.Log("aa");
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("gkgk");
            if (Playerstat.item_Key >= 1)
            {
                Playerstat.item_Key--;
                Destroy(this.gameObject);
            }
        }
    }
}
