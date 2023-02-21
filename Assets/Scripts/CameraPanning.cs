using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    public float startingX;
    public float startingY;

    public float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        startingX = transform.position.x;
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        

        Mathf.Clamp(transform.position.x, startingX, 5);
        Mathf.Clamp(transform.position.y, startingY, 5);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        //Get mouse positition

        //Clamp camera
        //Lerp camera to position with delay
    }
}
