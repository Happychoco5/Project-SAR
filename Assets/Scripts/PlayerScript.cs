using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public LayerMask IgnoreLayer;

    public Vector3 prevLookPos;
    CharacerScript cs;

    bool characterClicked;

    private Vector3 velocity = Vector3.zero;
    Vector3 camVelocity = Vector3.zero;
    public float smoothTime = 0.3f;

    bool resetLookPos;

    public ParticleSystem clickParticle;
    public AudioSource particleAudioSource;

    public AudioMixer audioMixer;

    public AudioClip openDialogue;
    public AudioSource audSource;

    public int currRoom;

    public ValueControl valueHolder;

    public FurnitureControl FC;

    public GameObject DebugMenu;

    public GameObject pauseMenu;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.playerScript = this;
        FC = GetComponent<FurnitureControl>();
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
                    //Place the particle at the cursor position and play the PFI
                    clickParticle.gameObject.transform.position = hit.point;
                    clickParticle.Stop();
                    clickParticle.Play();
                    particleAudioSource.Play();

                    //If we hit a character
                    CheckCharacter(hit);
                }
            }
        }

        //Make the camera's position move over to the center of the room's x transform
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(GameManager.instance.Rooms[currRoom].transform.position.x, transform.position.y, transform.position.z), ref camVelocity, 1);

        //Check if the charater is clicked
        if (characterClicked)
        {
            //If the character is looking in the direction of the camera, have them look at the camera

            Vector3 targetDir = GameManager.instance.currRoom.character.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, GameManager.instance.currRoom.character.transform.forward);
            if (angle > 90)
            {
                //cs.GetComponent<CharacterMove>().aimTarget.GetComponent<LookAtIK>().
                cs.lookObject.transform.position = Vector3.SmoothDamp(cs.lookObject.transform.position, Camera.main.transform.position, ref velocity, smoothTime);
                Debug.Log("Looking at the camera");
            }

        }

        //If we reset the look position, make the lookobject go back to the previous position
        if (resetLookPos)
        {
            cs.lookObject.transform.localPosition = Vector3.SmoothDamp(cs.lookObject.transform.localPosition, prevLookPos, ref velocity, 0.3f);
            if (cs.lookObject.transform.localPosition == prevLookPos)
            {
                Debug.Log("Reached position");
                resetLookPos = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            //Open debug menu
            if(DebugMenu.activeInHierarchy)
            {
                DebugMenu.SetActive(false);
            }
            else
            {
                DebugMenu.SetActive(true);
            }
            //Debug.Log("AAAAAAAAA");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Open the pause menu
            if (FC.editMode)
            {
                if(FC.hasPlaced)
                {
                    FC.LiveMode();
                }
            }
            else
            {
                if(pauseMenu.activeInHierarchy)
                {
                    pauseMenu.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(true);
                }
            }
        }
    }

    public void ChangeRooms(int num)
    {
            //Check if we can increment room by 1
            //If can, room increment + 1
            //Pan camera over
            //Set active room to the current room and deactivate the prev room

            //Check if this room exists
            if (currRoom + num <= GameManager.instance.Rooms.Count - 1 && currRoom + num >= 0)
            {
                //Go into live mode(if we weren't already)
                FC.LiveMode();

                //Change the frontwall layer of the previous room to unclickable
                GameManager.instance.Rooms[currRoom].GetComponent<RoomScript>().frontWall.layer = 11;

                //Set the activeroom to false for the previous room
                GameManager.instance.Rooms[currRoom].GetComponent<RoomScript>().activeRoom = false;

                //Increment to the next room
                currRoom += num;

                //Set the room to active
                GameManager.instance.Rooms[currRoom].GetComponent<RoomScript>().activeRoom = true;

                //Set the frontwall's layer to frontWall so we can click through it.
                GameManager.instance.Rooms[currRoom].GetComponent<RoomScript>().frontWall.layer = 9;

                //Set the room script in the current room in the game manager to this room
                GameManager.instance.currRoom = GameManager.instance.Rooms[currRoom].GetComponent<RoomScript>();

                foreach (FurnitureUI furnUI in FC.furnitureUIs)
                {
                    furnUI.currentFurniture = 0;
                    furnUI.UpdateUI(GameManager.instance.currRoom.furnitureAmounts[furnUI.furnName.text]);
                }

                //Set the furniture display to inactive(in case it was active)
                GameManager.instance.UI.transform.Find("FurnitureDisplay").gameObject.SetActive(false);

                //Set the value holder to the current room's value holder
                valueHolder = GameManager.instance.currRoom.valueHolder;

                //If we have yet to place a furniture, cancel it
                if (!FC.hasPlaced)
                {
                    FC.CancelFurniture();
                }
            }
    }

    public void CheckCharacter(RaycastHit hit)
    {
        if (hit.collider.tag == "Character")
        {
            //Character has been clicked once
            if (!characterClicked)
            {
                //Get the character script of the hit character
                cs = hit.collider.gameObject.GetComponent<CharacerScript>();

                //If reset look position is false
                if (!resetLookPos)
                {
                    //The previous look position will always need to be infront of the character.
                    //So we save the previous look position(which is infront of the character)
                    //We will only save this if the previous look position is in front of the character
                    prevLookPos = cs.lookObject.transform.localPosition;
                }
                else
                {
                    resetLookPos = false;
                }
            }
            //Check if character has been clicked again
            else
            {
                //rotate towards the camera
                //stop what they are doing
                //open the dialogue option

                cs.GetComponent<CharacterMove>().usingFurniture = false;

                Vector3 targetDirection = GameManager.instance.currRoom.character.transform.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, 0, 0);

                cs.transform.rotation = Quaternion.LookRotation(new Vector3(-newDir.x, 0, -newDir.z));
                // GameManager.instance.currRoom.character.transform.rotation = new Quaternion(0, GameManager.instance.currRoom.character.transform.rotation.y, 0, 0);

                cs.GetComponent<CharacterMove>().aiPath.canMove = false;
                cs.GetComponent<CharacterMove>().clickedOn = true;

                audSource.clip = openDialogue;
                audSource.Play();

                DialogueManager.StartConversation("New Conversation 2", transform, hit.collider.transform);

                Debug.Log("Character clicked again.");
            }
            Debug.Log("Character clicked.");
            characterClicked = true;
        }
        else
        {
            //Reset clicked once check
            if (characterClicked)
            {
                //Reset the look position back to the original position
                resetLookPos = true;
                characterClicked = false;
                cs.GetComponent<CharacterMove>().clickedOn = false;
                cs.GetComponent<CharacterMove>().aiPath.canMove = true;
                DialogueManager.StopConversation();
            }
        }
    }
}
