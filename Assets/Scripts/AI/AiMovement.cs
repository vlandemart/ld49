using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    public bool HasInteractiveObject()
    {
        List<GameObjectWithDist> availableInteractiveObject = GetAvailableInteractiveObject();
        return availableInteractiveObject.Count != 0;
    }

    public Vector3 GetInteractiveObjectPosition()
    {
        List<GameObjectWithDist> availableInteractiveObject = GetAvailableInteractiveObject();
        availableInteractiveObject.Sort((p1, p2) => p1.Distance.CompareTo(p2.Distance));

        return availableInteractiveObject[0].GameObject.transform.position;
    }
    
    [SerializeField] private float maxObjectSpeedToCatch = 0.3f;
    [SerializeField] private float range = 10f;
    [SerializeField] private string interactableTag = "Interactable";

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private BehaviorTree behaviorTree;
    private GameObject[] interactiveObjects;
    private static readonly int Velocity = Animator.StringToHash("velocity");

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        behaviorTree = GetComponentInChildren<BehaviorTree>();
        
        interactiveObjects = GameObject.FindGameObjectsWithTag(interactableTag);
    }
    
    private void Update()
    {
        if (this.IsStunned())
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            behaviorTree.DisableBehavior();
        }
        else
        {
            navMeshAgent.isStopped = false;
            behaviorTree.EnableBehavior();
        }

        if (RandomPoint(transform.position, out var point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        }
    }

    private void FixedUpdate()
    {
        if (this.IsStunned())
        {
            animator.SetFloat(Velocity, 0);
            return;
        }
        
        animator.SetFloat(Velocity, navMeshAgent.velocity.magnitude);
    }

    private bool RandomPoint(Vector3 center, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            var randomPoint = center + Random.insideUnitSphere * range;
            if (!NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
                continue;

            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private List<GameObjectWithDist> GetAvailableInteractiveObject()
    {
        var list = new List<GameObjectWithDist>();
        if (interactiveObjects == null)
            return list;

        foreach (var interactiveObject in interactiveObjects)
        {
            var throwableObject = interactiveObject.GetComponent<ThrowableObject>();
            if (throwableObject == null || throwableObject.taken || throwableObject.GetCurrentSpeed() > maxObjectSpeedToCatch)
                continue;

            var path = new NavMeshPath();

            if (!NavMesh.CalculatePath(transform.position, interactiveObject.transform.position, NavMesh.AllAreas,
                path))
            {
                Debug.LogWarning("No path found!");
                continue;
            }

            var pathLength = GetPathLength(path);
            var gameObjectWithDist = new GameObjectWithDist();
            gameObjectWithDist.Distance = pathLength;
            gameObjectWithDist.GameObject = interactiveObject.gameObject;

            list.Add(gameObjectWithDist);
        }

        return list;
    }

    private static float GetPathLength(NavMeshPath path)
    {
        var pathLength = 0.0f;
        if (path.status == NavMeshPathStatus.PathInvalid)
            return pathLength;
        
        for (int i = 1; i < path.corners.Length; ++i)
        {
            pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return pathLength;
    }
}

internal class GameObjectWithDist
{
    public GameObject GameObject;
    public float Distance;
}