using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int lives;
    public int item_Key;
    public int shield;
    public int score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
