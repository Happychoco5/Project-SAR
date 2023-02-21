using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class UseFurniture : Action
{
    public CharacterMove cmScript;
    public SharedTransform target;
    public SharedGameObject furnitureObject;
    Vector3 prevAimTarget;

    AudioSource audSource;



    float timer;

    public override void OnStart()
    {
        timer = 0;
        cmScript = GetComponent<CharacterMove>();
        cmScript.usingFurniture = true;
        cmScript.gameObject.transform.rotation = target.Value.rotation;
        cmScript.aiPath.canMove = false;
        cmScript.animator.SetBool(furnitureObject.Value.GetComponent<FurnitureScript>().animationToPlay, true);

        prevAimTarget = cmScript.aimTarget.transform.localPosition;

        audSource = GetComponent<AudioSource>();

        cmScript.aimTarget.transform.position = furnitureObject.Value.GetComponent<FurnitureScript>().aimTargetPosition.position;
        if (furnitureObject.Value.GetComponent<FurnitureScript>().moveToPosition)
        {
            transform.position = furnitureObject.Value.GetComponent<FurnitureScript>().moveToPosition.position;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if(cmScript.usingFurniture)
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                //Add the exp gain and research gain values from the furniture to the player/gamemanager.
                //Overall the amounts will be summed up
                cmScript.rs.valueHolder.PlayValuesOnUse(furnitureObject.Value.GetComponent<FurnitureStats>().coins, furnitureObject.Value.GetComponent<FurnitureStats>().researchPoints);
                GameManager.instance.AddTheseValues(furnitureObject.Value.GetComponent<FurnitureStats>().coins, furnitureObject.Value.GetComponent<FurnitureStats>().researchPoints);

                //Add the xp
                cmScript.GetComponent<CharacerScript>().GainExperience(furnitureObject.Value.GetComponent<FurnitureStats>().experiencePoints);
                GameManager.instance.GainExperience(furnitureObject.Value.GetComponent<FurnitureStats>().experiencePoints);

                furnitureObject.Value.GetComponent<FurnitureStats>().PlayAudio();

                cmScript.aimTarget.transform.localPosition = prevAimTarget;
                cmScript.animator.SetBool(furnitureObject.Value.GetComponent<FurnitureScript>().animationToPlay, false);
                furnitureObject.Value = null;
                target.Value = null;
                cmScript.aiPath.canMove = true;

                timer = 0;
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Running;
            }
        }
        else
        {
            cmScript.aimTarget.transform.localPosition = prevAimTarget;
            cmScript.animator.SetBool(furnitureObject.Value.GetComponent<FurnitureScript>().animationToPlay, false);
            furnitureObject.Value = null;
            target.Value = null;
            cmScript.aiPath.canMove = true;

            timer = 0;
            return TaskStatus.Failure;
        }
    }
}
