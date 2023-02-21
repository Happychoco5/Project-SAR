using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CharacterMove : MonoBehaviour
{
    public AIDestinationSetter destinationSetter;
    public AIPath aiPath;
    public Animator animator;
    public Seeker seeker;

    public RoomScript rs;

    public Transform newTarget;
    public FurnitureScript furnitureScript;

    public GameObject aimTarget;
    public Vector3 aimTargetPrevPos;

    public bool usingFurniture;

    public bool clickedOn;
    Rigidbody rb;

    bool rotated;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(3, 10);
        aimTargetPrevPos = aimTarget.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isMoving", aiPath.reachedEndOfPath);

        if (!aiPath.reachedEndOfPath)
        {
            if (aiPath.canMove)
            {
                //start walking if not reached end of path
                animator.SetBool("isMoving", true);
                Debug.Log("Still moving." + aiPath.reachedEndOfPath);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
