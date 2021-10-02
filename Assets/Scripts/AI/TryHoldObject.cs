using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


[TaskCategory("AI")]
public class TryHoldObject : Action
{
    // Component references
    private AiObjectThrower objectThrower;

    /// <summary>
    /// Cache the component references.
    /// </summary>
    public override void OnAwake()
    {
        objectThrower = gameObject.GetComponentInChildren<AiObjectThrower>();
    }

    public override TaskStatus OnUpdate()
    {
        if (objectThrower.TryHoldObject())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}