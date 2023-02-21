using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureScript : MonoBehaviour
{
    public bool interactable;
    public bool changeToTrigger;

    public bool floorObject;
    public bool wallObject;

    public bool touchingFloor;
    public bool touchingWall;

    public bool canPlace;
    public bool touchingFurniture;

    public bool moving;

    public Material cantPlaceMat;
    public Material ogFurnitureMat;

    public AudioSource audSource;
    public AudioClip placeSound;

    public FurnitureStats fStats;

    public MMFeedbacks feedback;
    public TMPro.TextMeshPro text;

    public Transform walkTransform;
    public string animationToPlay;

    public Transform moveToPosition;

    public Transform aimTargetPosition;

    public GameObject sphereCheck;

    public FurnitureUI myUI;

    // Start is called before the first frame update
    void Start()
    {
        ogFurnitureMat = GetComponent<Renderer>().material;
        //text.text = fStats.quality.ToString("0.00");
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            if (GetComponent<FurnitureStats>().cost <= GameManager.instance.coinsTotal)
            {
                if (!touchingFurniture)
                {
                    if (floorObject)
                    {
                        if (!touchingWall)
                        {
                            if (touchingFloor)
                            {
                                GetComponent<Renderer>().material = ogFurnitureMat;
                                canPlace = true;
                            }
                            else
                            {
                                GetComponent<Renderer>().material = cantPlaceMat;
                                canPlace = false;
                                Debug.Log("Can't place due to not touching floor");
                            }
                        }
                        else
                        {
                            GetComponent<Renderer>().material = cantPlaceMat;
                            canPlace = false;
                            Debug.Log("Can't place due to touching a wall");
                        }
                    }
                    if (wallObject)
                    {
                        if (touchingWall)
                        {
                            //canplace is true
                            //change back to og color
                            GetComponent<Renderer>().material = ogFurnitureMat;
                            canPlace = true;
                        }
                        else
                        {
                            //change color to red or something
                            //canplace is false

                            GetComponent<Renderer>().material = cantPlaceMat;
                            canPlace = false;
                            Debug.Log("Can't place due to not touching a wall");
                        }
                    }
                }
                else
                {
                    GetComponent<Renderer>().material = cantPlaceMat;
                    canPlace = false;
                    Debug.Log("Can't place due to touching another furniture");
                }
            }
            else
            {
                GetComponent<Renderer>().material = cantPlaceMat;
                canPlace = false;
                Debug.Log("Can't place due to not enough money");
            }
        }
        else
        {
            if (!touchingFurniture)
            {
                if (floorObject)
                {
                    if (!touchingWall)
                    {
                        if (touchingFloor)
                        {
                            GetComponent<Renderer>().material = ogFurnitureMat;
                            canPlace = true;
                        }
                        else
                        {
                            GetComponent<Renderer>().material = cantPlaceMat;
                            canPlace = false;
                            Debug.Log("Can't place due to not touching the floor");
                        }
                    }
                    else
                    {
                        GetComponent<Renderer>().material = cantPlaceMat;
                        canPlace = false;
                        Debug.Log("Can't place due to touching a wall");
                    }
                }
                if (wallObject)
                {
                    if (touchingWall)
                    {
                        //canplace is true
                        //change back to og color
                        GetComponent<Renderer>().material = ogFurnitureMat;
                        canPlace = true;
                    }
                    else
                    {
                        //change color to red or something
                        //canplace is false

                        GetComponent<Renderer>().material = cantPlaceMat;
                        canPlace = false;
                        Debug.Log("Can't place due to ");
                    }
                }
            }
            else
            {
                GetComponent<Renderer>().material = cantPlaceMat;
                canPlace = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
            if (collision.gameObject.tag == "Floor")
            {
                touchingFloor = true;
            }
            if (collision.gameObject.tag == "Wall")
            {
                touchingWall = true;
            }

        if (collision.gameObject.tag == "Furniture")
        {
            touchingFurniture = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
            if (collision.gameObject.tag == "Floor")
            {
                touchingFloor = false;
            }

            if (collision.gameObject.tag == "Wall")
            {
                touchingWall = false;
            }

        if(collision.gameObject.tag == "Furniture")
        {
            touchingFurniture = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            touchingFloor = true;
        }
        if (collision.gameObject.tag == "Wall")
        {
            touchingWall = true;
        }

        if (collision.gameObject.tag == "Furniture")
        {
            touchingFurniture = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            touchingFloor = false;
        }

        if (collision.gameObject.tag == "Wall")
        {
            touchingWall = false;
        }

        if (collision.gameObject.tag == "Furniture")
        {
            touchingFurniture = false;
        }
    }
}
