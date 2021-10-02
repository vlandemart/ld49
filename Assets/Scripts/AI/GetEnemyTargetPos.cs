using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("AI")]
public class GetEnemyTargetPos : Action
{
    public SharedGameObject enemyTarget;
    public SharedVector3 targetPosition;
    public SharedVector3 rndEnemyTargetPos;
    public SharedFloat minRadius;
    public SharedFloat maxRadius;

    /// <summary>
    /// Cache the component references.
    /// </summary>
    public override void OnAwake()
    {
    }

    public override void OnStart()
    {
        Vector3 enemyPosition = enemyTarget.Value.transform.position;
        targetPosition.Value = enemyPosition;
        if (RandomPoint(enemyPosition, minRadius.Value, maxRadius.Value, out var result))
        {
            rndEnemyTargetPos.Value = result;
        }
        else
        {
            rndEnemyTargetPos.Value = enemyPosition;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

    private bool RandomPoint(Vector3 center, float min, float max, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 rndDir = Random.insideUnitSphere;
            Vector3 randomPoint = center + (rndDir.normalized * min) + (rndDir * (max - min));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}