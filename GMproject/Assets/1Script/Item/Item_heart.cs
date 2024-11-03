using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_heart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(60.0f, 0.0f, 0.0f) * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        PlayerStat playerstat = other.gameObject.GetComponent<PlayerStat>();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerstat.lives++;
            Destroy(this.gameObject);
        }
        {
            
        }
    }
}
