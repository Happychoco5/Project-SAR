using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FurnitureCheck : Conditional
{
    public SharedGameObject furnitureObject;
    public SharedTransform target;
    int randomNum;

    public CharacterMove cmScript;

    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        cmScript = GetComponent<CharacterMove>();
        randomNum = Random.Range(0, GameManager.instance.currRoom.interactableFurniture.Count);
    }

    public override TaskStatus OnUpdate()
    {
        //If we can find a furniture that is interactable, go to the furniture
        //else, unsuccessful

        //Get a random number, if that number is one of the furniture and it has an interactable script, store the value
        //If not, task fails.
        //Tries again later.
        //aimTarget.transform.localPosition = aimTargetPrevPos;
        //if (furnitureScript)
        //{
            //animator.SetBool(furnitureScript.animationToPlay, false);
        //}
        //newTarget = null;
       // aiPath.canMove = true;
        //int randomNum;
        if (cmScript.rs.interactableFurniture.Count > 0)
        {
            if (cmScript.rs.interactableFurniture[randomNum] != null)
            {
                //Go to the target attached to the furniture script on this furniture.
                //Then perform the corresponding animation.
                if (cmScript.rs.interactableFurniture[randomNum].GetComponent<FurnitureScript>().interactable)
                {
                    furnitureObject.Value = cmScript.rs.interactableFurniture[randomNum];
                    target.Value = cmScript.rs.interactableFurniture[randomNum].GetComponent<FurnitureScript>().walkTransform;
                    return TaskStatus.Success;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
