using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    public class HasEnemyCondition : Conditional
    {
        public SharedGameObject Enemy;

        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            if (Enemy.Value == null)
            {
                GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
                Enemy.Value = gameObject;
            }

            if (Enemy.Value == null)
            {
                return TaskStatus.Failure;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
    }
}