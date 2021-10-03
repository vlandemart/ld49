using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


[TaskCategory("AI")]
public class ThrowObject : Action
{
    public SharedVector3 targetPosition;
    
    // Component references
    private AiObjectThrower objectThrower;

    /// <summary>
    /// Cache the component references.
    /// </summary>
    public override void OnAwake()
    {
        objectThrower = gameObject.GetComponentInChildren<AiObjectThrower>();
    }

    public override void OnStart()
    {
        objectThrower.PlayAnimThrow(targetPosition.Value);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}