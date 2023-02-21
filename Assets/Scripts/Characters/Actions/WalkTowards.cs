using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WalkTowards : Action
{
    public CharacterMove cmScript;
    public SharedTransform target;
    public SharedGameObject furnitureObject;

    public float timer;

    public override void OnStart()
    {
        timer = 0;
        cmScript = GetComponent<CharacterMove>();
        cmScript.aiPath.canMove = true;
        cmScript.destinationSetter.target.transform.position = target.Value.position;
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5)
        {
            if (!cmScript.aiPath.reachedEndOfPath)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
