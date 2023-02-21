using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WalkAround : Action
{
    public CharacterMove cmScript;

    public int[] nums;

    public override void OnStart()
    {
        cmScript = GetComponent<CharacterMove>();
        cmScript.destinationSetter.target.position = RandomNavSphere(cmScript.rs.transform.position, 2F, 12);
    }

    public override TaskStatus OnUpdate()
    {
        if(!cmScript.aiPath.reachedEndOfPath)
        {
            //Still moving
            return TaskStatus.Running;
        }
        else
        {
            //Set a new position to move to.
            return TaskStatus.Success;
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        float newOriginX;
        float newOriginZ;

        int randomIndex;
        randomIndex = Random.Range(0, nums.Length);
        dist *= nums[randomIndex];
        
        newOriginX = Random.Range(origin.x, origin.x + dist);

        randomIndex = Random.Range(0, nums.Length);
        dist *= nums[randomIndex];
        newOriginZ = Random.Range(origin.z, origin.z + dist);
        //Debug.Log("New X " + newOriginX + "New Z " + newOriginZ);

        Vector3 randomLocation = new Vector3(newOriginX, 0, newOriginZ);

        return randomLocation;
    }
}
