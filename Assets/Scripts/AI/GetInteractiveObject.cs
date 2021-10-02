using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Tasks
{
    public class GetInteractiveObject : Action
    {
        public SharedVector3 targetPosition;
        
        public override TaskStatus OnUpdate()
        {
            targetPosition.Value = gameObject.GetComponent<AiMovement>().GetInteractiveObjectPosition();
            return TaskStatus.Success;
        }
    }
}