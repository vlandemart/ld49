using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("AI")]
public class HasInteractiveObject : Conditional
{
    private AiMovement movement;

    public override void OnStart()
    {
        movement = gameObject.GetComponent<AiMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        if (movement.HasInteractiveObject())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}