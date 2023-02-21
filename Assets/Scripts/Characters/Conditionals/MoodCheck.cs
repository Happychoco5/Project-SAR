using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoodCheck : Conditional
{
    public CharacterMood moodScript;
    public override void OnStart()
    {
        //Check the character mood;
        moodScript = GetComponent<CharacterMood>();

        moodScript.tired = Random.Range(0, 100);
    }

    public override TaskStatus OnUpdate()
    {
        if (moodScript.tired >= 50)
        {
            //Move to see if there is somewhere to sit.
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}