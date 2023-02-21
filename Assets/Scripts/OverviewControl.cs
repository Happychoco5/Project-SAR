using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewControl : MonoBehaviour
{
    public LayerMask IgnoreLayer;
    public Camera cam;

    GameObject gridButton;

    public int amountOfRooms;

    public int houseDistance = 5;

    public GameObject storedCharacter;
    public bool placingCharacter;

    public Transform walkTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreLayer))
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.collider.tag == "BuildGrid")
                {
                    //set a bool
                    if (gridButton)
                    {
                        //Reset
                        gridButton.SetActive(false);
                    }
                    gridButton = hit.collider.gameObject.GetComponent<BuildingControl>().button;
                    gridButton.SetActive(true);
                }
                else
                {
                    if (gridButton)
                    {
                        //Reset
                        gridButton.SetActive(false);
                    }
                }
                if (hit.collider.tag == "Button")
                {
                    hit.collider.gameObject.transform.parent.transform.parent.GetComponent<BuildingControl>().BuildRoom();
                }
                if(placingCharacter)
                {
                    if (hit.collider.tag == "Wall")
                    {
                        if(!hit.collider.gameObject.transform.parent.transform.parent.GetComponent<RoomScript>().character)
                        {
                            GameObject tempCharacter = Instantiate(storedCharacter, new Vector3(hit.collider.gameObject.transform.parent.transform.parent.transform.position.x, 0.1f, hit.collider.gameObject.transform.parent.transform.parent.transform.position.z), storedCharacter.transform.rotation);
                            Transform tempTarget = Instantiate(walkTarget, tempCharacter.transform.position, walkTarget.rotation);
                            tempCharacter.GetComponent<CharacterMove>().destinationSetter.target = tempTarget;
                            hit.collider.gameObject.transform.parent.transform.parent.GetComponent<RoomScript>().character = tempCharacter;
                            tempCharacter.GetComponent<CharacterMove>().rs = hit.collider.gameObject.transform.parent.transform.parent.GetComponent<RoomScript>();
                        }
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            //go up
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //go down
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //go left
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //go right
        }
    }
}
