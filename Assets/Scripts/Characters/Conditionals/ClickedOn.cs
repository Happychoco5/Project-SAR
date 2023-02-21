using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ClickedOn : Conditional
{
    CharacterMove cmScript;

    public bool opposite;

    public override void OnStart()
    {
        cmScript = GetComponent<CharacterMove>();
    }

    public override TaskStatus OnUpdate()
    {
        if(!opposite)
        {
            if (cmScript.clickedOn)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        else
        {
            if (!cmScript.clickedOn)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
