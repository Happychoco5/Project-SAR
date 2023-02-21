using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed;

    Vector3 holdPosition;


    //public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public Rigidbody rb;

    bool holdingMiddleMouse;

    float x = 0.0f;
    float y = 0.0f;

    Vector3 orbitPoint;

    float mY;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(3, 22);
        Physics.IgnoreLayerCollision(10, 22);
        Physics.IgnoreLayerCollision(8, 22);

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");

        //rigidbody.velocity = new Vector3(mH , rigidbody.velocity.y, mV) * 3;

        //transform.Translate(transform.forward)
        //Move along z axis forward

        //Move along x axis


        rb.velocity = (transform.parent.forward * mV + transform.right * mH + transform.parent.up * mY) * moveSpeed;

        if (Input.GetKey(KeyCode.E))
        {
            //Go up
            mY = 1;
        }

        else if(Input.GetKey(KeyCode.Q))
        {
            //Go down
            mY = -1;
        }
        
        else
        {
            mY = 0;
        }

        //Rotate cam
        if (Input.GetMouseButtonDown(1))
        {
            holdPosition = transform.position;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                orbitPoint = hit.point;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        if(Input.GetMouseButton(1))
        {
            holdingMiddleMouse = true;
        }
        if(Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            holdingMiddleMouse = false;
        }

        //if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        //{
            //Zoom out
            //transform.Translate(-transform.forward * Time.deltaTime * moveSpeed);
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        //{
            //Zoom in
            //transform.Translate(transform.forward * Time.deltaTime * moveSpeed);
        //}
    }
    void LateUpdate()
    {
        //if (target)
        //{
        if(holdingMiddleMouse)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            RaycastHit hit;
            //if (Physics.Linecast(orbitPoint, transform.position, out hit))
            //{
                //distance -= hit.distance;
            //}
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + orbitPoint;

            Quaternion parentRotation = Quaternion.Euler(0, x, 0);

            transform.parent.rotation = parentRotation;
            transform.rotation = rotation;
        }

            //transform.position = position;
        //}
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
