using System;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnDeath : MonoBehaviour
{
    [SerializeField] private Animator mainAnimator;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody mainRigidbody;

    private List<Collider> allColliders = new List<Collider>();
    private List<Rigidbody> allRigidbodies = new List<Rigidbody>();
    private bool isKinematicByDefault;
    private Stuneable stuneable;

    private void Awake()
    {
        stuneable = GetComponent<Stuneable>();
        isKinematicByDefault = mainRigidbody.isKinematic;
    }
    
    private void Start()
    {
        DisableRagdoll();
        stuneable.OnEnterStun.AddListener(EnableRagdoll);
        stuneable.OnExitStun.AddListener(DisableRagdoll);
    }

    private void EnableRagdoll(Vector3 hitObjectVelocity)
    {
        mainAnimator.enabled = false;
        
        if (mainCollider != null)
        {
            foreach (var col in allColliders)
            {
                col.enabled = true;
            }

            mainCollider.enabled = false;
        }

        Vector3 velocity = hitObjectVelocity;
        velocity.Normalize();
        velocity *= 7f;

        foreach (var rb in allRigidbodies)
        {
            rb.mass = 0.1f;
            rb.isKinematic = false;
            rb.velocity = velocity;
        }

        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = true;
        }
    }

    private void DisableRagdoll()
    {
        allColliders.AddRange(GetComponentsInChildren<Collider>());
        foreach (var col in allColliders)
        {
            col.enabled = false;
        }

        if (mainCollider != null)
        {
            mainCollider.enabled = true;
        }

        allRigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        foreach (var rb in allRigidbodies)
        {
            rb.isKinematic = true;
        }

        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = isKinematicByDefault;
        }
    }

    public Vector3 Random(float min, float max)
    {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max));
    }
}