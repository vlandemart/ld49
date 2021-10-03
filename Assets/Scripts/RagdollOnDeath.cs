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

    private void Start()
    {
        DisableRagdoll();
    }

    private bool isKinematicByDefault;

    private void Awake()
    {
        isKinematicByDefault = mainRigidbody.isKinematic;
    }

    public void EnableRagdoll(GameObject damager)
    {
        mainAnimator.enabled = false;
        
        if (mainCollider != null)
        {
            foreach (var collider in allColliders)
            {
                collider.enabled = true;
            }

            mainCollider.enabled = false;
        }

        Vector3 velocity = transform.position - damager.transform.position;
        velocity.Normalize();
        velocity *= 7f;

        foreach (var rigidbody in allRigidbodies)
        {
            rigidbody.mass = 0.1f;
            rigidbody.isKinematic = false;
            rigidbody.velocity = velocity;
        }

        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = true;
        }
    }

    public void DisableRagdoll()
    {
        mainAnimator.enabled = true;
        
        allColliders.AddRange(GetComponentsInChildren<Collider>());
        foreach (var collider in allColliders)
        {
            collider.enabled = false;
        }

        if (mainCollider != null)
        {
            mainCollider.enabled = true;
        }

        allRigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        foreach (var rigidbody in allRigidbodies)
        {
            rigidbody.isKinematic = true;
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