using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCheck : MonoBehaviour
{
    Color ogColor;
    // Start is called before the first frame update
    void Start()
    {
        ogColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Floor")
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Floor")
        {
            GetComponent<Renderer>().material.color = ogColor;
        }
    }
}
