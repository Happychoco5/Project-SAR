using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicCameraControl : MonoBehaviour
{
    public LayerMask IgnoreLayer;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get a raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            //Cast the ray
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreLayer))
            {
                //Check to make sure it's not over UI
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    GameManager.instance.playerScript.CheckCharacter(hit);
                }
            }
        }
    }
}
