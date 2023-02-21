using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurnitureControl : MonoBehaviour
{
    [SerializeField]
    PlayerScript PS;

    //Furniture handling
    public LayerMask furnIgnoreLayer;

    public GameObject furniture;
    public GameObject tempFurniture;
    public Material ogFurnitureMat;
    public Rigidbody tempRigidBody;

    public bool hasPlaced;
    bool canPlace;

    public bool moving;

    public Vector3 furnitureLocation;

    public bool editMode;
    public bool liveMode;

    public float speed;

    bool rotating;

    public Vector3 tempFurniturePosition;

    public FurnitureUI furnitureUI;
    public List<FurnitureUI> furnitureUIs;

    public Dictionary<string, int> tempFurnitureAmounts;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get a raycast
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~PS.IgnoreLayer))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButtonDown(0))
                {
                    //If we are in edit mode
                    if (editMode)
                    {
                        //And we hit something tagged with furniture
                        if (hit.collider.tag == "Furniture")
                        {
                            //Only if we are not currently placing an object
                            if (hasPlaced)
                            {
                                //Open the furniture display
                                GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(true);

                                //Set the furniture name to the object name
                                GameManager.instance.UI.transform.Find("FurnitureDisplay").transform.Find("FurnitureText").GetComponent<Text>().text = hit.collider.GetComponent<FurnitureStats>().objectName;

                                //Temp furniture is the object we hit.
                                tempFurniture = hit.collider.gameObject;
                            }
                        }
                        //If we didn't hit the furniture
                        else
                        {
                            //Set the furniture display to false
                            GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(false);
                        }
                    }
                    //If we are not in edit mode
                    else
                    {
                        //If we hit a furniture
                        if (hit.collider.tag == "Furniture")
                        {
                            //Open the livemode furniture display
                            GameManager.instance.UI.transform.Find("LiveModeDisplay").gameObject.SetActive(true);

                            //Set the furniture name to the object name
                            GameManager.instance.UI.transform.Find("LiveModeDisplay").transform.Find("FurnitureText").GetComponent<Text>().text = hit.collider.GetComponent<FurnitureStats>().objectName;

                            //Set tempfurniture to the object we hit
                            tempFurniture = hit.collider.gameObject;

                            //The upgrade cost text to how much it costs to upgrade
                            GameManager.instance.UI.transform.Find("LiveModeDisplay").transform.Find("UpgradeCost").GetComponent<Text>().text = "$" + hit.collider.GetComponent<FurnitureStats>().upgradeCost.ToString("0.00");
                        }
                        else
                        {
                            tempFurniture = null;
                            GameManager.instance.UI.transform.Find("LiveModeDisplay").gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        //If we are placing a piece of furniture
        if (!hasPlaced)
        {
            RaycastHit furnHit;
            Ray furnRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(furnRay, out furnHit, Mathf.Infinity, ~furnIgnoreLayer))
            {
                //If we are not rotating, move the furniture
                if (!rotating)
                {
                    Vector3 newPos = new Vector3(furnHit.point.x, furnHit.point.y, furnHit.point.z);
                    tempRigidBody.MovePosition(newPos);
                }

                //If we can place the furniture, when we click, place the furniture
                if (tempFurniture.GetComponent<FurnitureScript>().canPlace)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        PlaceFurniture();
                    }
                }
            }

            //If we get escape, cancel the furniture
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelFurniture();
            }

            //Check for getting the control key down
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //Save the funiture position
                tempFurniturePosition = tempFurniture.transform.position;

            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //We are rotating the furniture
                rotating = true;

                if(tempFurniture)
                {
                    //Set the cursor lockstate to true to help control the object
                    Cursor.lockState = CursorLockMode.Locked;
                    //Keep the furniture at it's current location
                    tempFurniture.transform.position = tempFurniturePosition;
                    //Rotate the furniture
                    tempFurniture.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                //On control up, we are no longer rotating
                Cursor.lockState = CursorLockMode.None;
                rotating = false;
            }
        }
    }

    //Furniture code
    public void StartPlacement()
    {
        if (editMode)
        {
            if (hasPlaced)
            {
                //Give a new pieve of furniture and assign it to tempFurniture
                GameManager.instance.UI.transform.Find("FurnitureCatalog").gameObject.SetActive(false);
                tempFurniture = Instantiate(furniture);
                tempRigidBody = tempFurniture.GetComponent<Rigidbody>();
                ogFurnitureMat = tempFurniture.GetComponent<Renderer>().material;
                hasPlaced = false;
            }
        }
    }

    public void PlaceFurniture()
    {
        //Check if we can place the furniture before executing the code
        if (tempFurniture.GetComponent<FurnitureScript>().canPlace)
        {
            //If we are NOT moving the furniture
            if (!tempFurniture.GetComponent<FurnitureScript>().moving)
            {
                //Remove the cost of the furniture from the coin amount
                GameManager.instance.AddTheseValues(-tempFurniture.GetComponent<FurnitureStats>().cost, 0);
                //Add the quality to the room
                GameManager.instance.currRoom.roomQuality += tempFurniture.GetComponent<FurnitureStats>().quality;

                //Increase the amount of the current furniture
                GameManager.instance.currRoom.furnitureAmounts[tempFurniture.GetComponent<FurnitureStats>().objectName] += 1;
                furnitureUI.UpdateUI(1);
            }
            
            //Set the cursor lockstate to none, just in case we place while rotating
            Cursor.lockState = CursorLockMode.None;
            rotating = false;

            //Play the sound for the furniture placement
            tempFurniture.GetComponent<FurnitureScript>().audSource.clip = tempFurniture.GetComponent<FurnitureScript>().placeSound;
            tempFurniture.GetComponent<FurnitureScript>().audSource.Play();

            //Check if there is a sphere object on the furniture
            if (tempFurniture.GetComponent<FurnitureScript>().sphereCheck)
            {
                //If there is, set it inactive
                tempFurniture.GetComponent<FurnitureScript>().sphereCheck.SetActive(false);
            }

            //Add to the room list for the current room
            GameManager.instance.currRoom.interactableFurniture.Add(tempFurniture);

            //Add the stats to the room
            GameManager.instance.currRoom.roomCoins += tempFurniture.GetComponent<FurnitureStats>().coins;
            GameManager.instance.currRoom.roomExperience += tempFurniture.GetComponent<FurnitureStats>().experiencePoints;
            GameManager.instance.currRoom.roomQuality += tempFurniture.GetComponent<FurnitureStats>().quality;
            GameManager.instance.currRoom.roomResearch += tempFurniture.GetComponent<FurnitureStats>().researchPoints;

            //Set the furniture catalog back to true and the furniture display to false (to clear a bit of clutter on the screen)
            GameManager.instance.UI.transform.Find("FurnitureCatalog").gameObject.SetActive(true);
            GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(false);

            //Change the furniture to the original color on placement(will need to change)
            tempFurniture.GetComponent<Renderer>().material = tempFurniture.GetComponent<FurnitureScript>().ogFurnitureMat;

            //Just set moving to false just in case we are moving the object
            tempFurniture.GetComponent<FurnitureScript>().moving = false;

            //Play the feedbacks for the text and set the value holder to the furniture place position
            PS.valueHolder.gameObject.transform.position = tempFurniture.transform.position;
            PS.valueHolder.PlayFeedbacks();

            //Scan and update the navmesh
            AstarPath.active.Scan();

            //Change the furniture back to trigger
            tempFurniture.GetComponent<Collider>().isTrigger = true;

            //Can't remember why I'm doing this, but set the furniture script to false
            tempFurniture.GetComponent<FurnitureScript>().enabled = false;

            //Freeze all the rigidbody constraints
            tempRigidBody.constraints = RigidbodyConstraints.FreezeAll;
            
            //Set the layer to "furniture"
            tempFurniture.layer = 10;

            //We placed the furniture, so we set tempFurniture to null
            tempFurniture = null;
            hasPlaced = true;
        }
    }

    public void MoveFurniture()
    {
        //Check if we have placed a furniture
        if (hasPlaced)
        {
            //Check if tempFurniture exists
            if (tempFurniture)
            {
                //Check if this object has a sphere check
                if (tempFurniture.GetComponent<FurnitureScript>().sphereCheck)
                {
                    tempFurniture.GetComponent<FurnitureScript>().sphereCheck.SetActive(true);
                }
                
                //Change the trigger back to false
                tempFurniture.GetComponent<Collider>().isTrigger = false;

                //Save the furniture location in case we cancel
                furnitureLocation = tempFurniture.transform.position;

                //Set the furniture catalog to false
                GameManager.instance.UI.transform.Find("FurnitureCatalog").gameObject.SetActive(false);

                //Set the furniture script back to enabled
                tempFurniture.GetComponent<FurnitureScript>().enabled = true;

                //Save the rigidbody
                tempRigidBody = tempFurniture.GetComponent<Rigidbody>();

                //Change the bool moving to true
                tempFurniture.GetComponent<FurnitureScript>().moving = true;

                //Set the freeze rotation of the constraints
                tempFurniture.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                //Set the layer back to ignore furniture
                tempFurniture.layer = 8;
                hasPlaced = false;
            }
        }
    }

    public void RemoveFurniture()
    {
        //Remove selected furniture
        //Refund the moneys
        //Re-add it to the catalog

        //Increase the amount of the current furniture
        GameManager.instance.currRoom.furnitureAmounts[tempFurniture.GetComponent<FurnitureStats>().objectName] -= 1;
        furnitureUI.UpdateUI(-1);

        GameManager.instance.currRoom.interactableFurniture.Remove(tempFurniture);
        GameManager.instance.AddTheseValues(tempFurniture.GetComponent<FurnitureStats>().cost / 2, 0);
        Destroy(tempFurniture);
        GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(false);
        hasPlaced = true;
        AstarPath.active.Scan();
    }

    public void CancelFurniture()
    {
        //Remove the object
        if (!tempFurniture.GetComponent<FurnitureScript>().moving)
        {
            hasPlaced = true;
            Destroy(tempFurniture);
        }
        else
        {
            //restore to last location
            tempFurniture.transform.position = furnitureLocation;
            PlaceFurniture();
        }
        GameManager.instance.UI.transform.Find("FurnitureCatalog").gameObject.SetActive(true);
        GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(false);
    }

    public void EditMode()
    {
        if (!editMode)
        {
            //Bring up the UI
            //Disable the character
            GameManager.instance.UI.transform.Find("LiveModeDisplay").gameObject.SetActive(false);
            GameManager.instance.UI.transform.Find("FurnitureCatalog").gameObject.SetActive(true);
            if (GameManager.instance.currRoom.character)
            {
                GameManager.instance.currRoom.character.SetActive(false);
            }
            liveMode = false;
            editMode = true;
        }
    }
    public void LiveMode()
    {
        if (!liveMode)
        {
            //Close the UI
            //Enable the character at the door location
            if (!hasPlaced)
            {
                CancelFurniture();
            }
            GameManager.instance.UI.transform.Find("FurnitureCatalog").gameObject.SetActive(false);
            GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(false);
            if (GameManager.instance.currRoom.character)
            {
                GameManager.instance.currRoom.character.SetActive(true);
            }
            if (GameManager.instance.currRoom.character)
            {
                GameManager.instance.currRoom.character.transform.position = GameManager.instance.currRoom.respawnLocation.position;
            }
            editMode = false;
            liveMode = true;

            foreach (GameObject furn in GameManager.instance.currRoom.interactableFurniture)
            {
                furn.GetComponent<Collider>().isTrigger = false;
            }

            AstarPath.active.Scan();

            foreach (GameObject furn in GameManager.instance.currRoom.interactableFurniture)
            {
                furn.GetComponent<Collider>().isTrigger = true;
            }
        }
    }

    public void UpgradeFurniture()
    {
        if (GameManager.instance.coinsTotal >= tempFurniture.GetComponent<FurnitureStats>().upgradeCost)
        {
            //Have the text = the gained amount from the upgrade
            tempFurniture.GetComponent<FurnitureStats>().experiencePoints += tempFurniture.GetComponent<FurnitureStats>().experiencePoints * 0.2f;
            PS.valueHolder.expText.text = "+" + (tempFurniture.GetComponent<FurnitureStats>().experiencePoints * 0.2f).ToString("0.00");

            tempFurniture.GetComponent<FurnitureStats>().coins += tempFurniture.GetComponent<FurnitureStats>().coins * 0.2f;
            PS.valueHolder.coinText.text = "+" + (tempFurniture.GetComponent<FurnitureStats>().coins * 0.2f).ToString("0.00");

            tempFurniture.GetComponent<FurnitureStats>().quality += tempFurniture.GetComponent<FurnitureStats>().quality * 0.2f;
            PS.valueHolder.qualityText.text = "+" + (tempFurniture.GetComponent<FurnitureStats>().experiencePoints * 0.2f).ToString("0.00");

            tempFurniture.GetComponent<FurnitureStats>().researchPoints += tempFurniture.GetComponent<FurnitureStats>().researchPoints * 0.2f;
            PS.valueHolder.researchText.text = "+" + (tempFurniture.GetComponent<FurnitureStats>().experiencePoints * 0.2f).ToString("0.00");

            GameManager.instance.AddTheseValues(-tempFurniture.GetComponent<FurnitureStats>().upgradeCost, 0);

            tempFurniture.GetComponent<FurnitureStats>().upgradeCost += (tempFurniture.GetComponent<FurnitureStats>().upgradeCost * 0.2f);
            PS.valueHolder.researchText.text = "+" + (tempFurniture.GetComponent<FurnitureStats>().experiencePoints * 0.2f).ToString("0.00");

            GameManager.instance.UI.transform.Find("LiveModeDisplay").transform.Find("UpgradeCost").GetComponent<Text>().text = "$" + tempFurniture.GetComponent<FurnitureStats>().upgradeCost.ToString("0.00");
            //Play the feedbacks
            PS.valueHolder.transform.position = tempFurniture.transform.position;
            PS.valueHolder.PlayFeedbacks();
        }
        else
        {
            //Not enough money
            Debug.Log("Not enough money");
        }
    }
}
