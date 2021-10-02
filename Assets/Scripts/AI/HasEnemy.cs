using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Tasks
{
    public class HasEnemy : Decorator
    {
        public SharedGameObject Enemy;
        public SharedVector3 EnemyTargetPos;
    
        public override bool CanExecute()
        {
            if (Enemy.Value == null)
            {
                GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
                Enemy.Value = gameObject;
                EnemyTargetPos.Value = gameObject.transform.position;
            }

            return Enemy.Value != null;  // && !Enemy.Value.GetComponent<Health>().IsDead();
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // Update the execution status after a child has finished running.
        }

        public override void OnEnd()
        {
        }
    }
}