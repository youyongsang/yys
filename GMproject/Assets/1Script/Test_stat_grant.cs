using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_stat_grant : MonoBehaviour
{
    StatusEffectDisplay statusEffectDisplay;
    // Start is called before the first frame update
    void Start()
    {
        statusEffectDisplay = FindObjectOfType<StatusEffectDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            statusEffectDisplay.statusEffects["dotDamage"] = 3.0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            statusEffectDisplay.statusEffects["slow"] = 3.0f;
        }
    }
}
