using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log("Help");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCoins(float coins)
    {
        GameManager.instance.AddTheseValues(coins, 0);
    }
}
