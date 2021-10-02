using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    public class EnemyCheckDist : Conditional
    {
        public SharedGameObject enemyTarget;
        public SharedFloat dist;

        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            if (Vector3.Distance(transform.position, enemyTarget.Value.transform.position) <= dist.Value)
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