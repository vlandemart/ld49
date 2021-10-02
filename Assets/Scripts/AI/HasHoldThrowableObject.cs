using BehaviorDesigner.Runtime.Tasks;


[TaskCategory("AI")]
public class HasThrowableObject : Decorator
{
    public override bool CanExecute()
    {
        return gameObject.GetComponent<AiObjectThrower>().IsHoldingObject();
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        // Update the execution status after a child has finished running.
    }

    public override void OnEnd()
    {
    }
}