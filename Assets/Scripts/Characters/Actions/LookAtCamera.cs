using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class LookAtCamera : Action
{
    CharacterMove cmScript;

    public override void OnStart()
    {
        cmScript = GetComponent<CharacterMove>();
    }

    public override TaskStatus OnUpdate()
    {
        if(!cmScript.clickedOn)
        {
            cmScript.aiPath.canMove = true;
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
