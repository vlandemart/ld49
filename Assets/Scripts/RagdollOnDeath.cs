using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

public class RagdollOnDeath : MonoBehaviour
{
    public GameObject RootRagdoll;
    public GameObject AttachRagdoll;

    public bool IsRagdolled()
    {
        return isRagdolled;
    }

    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody mainRigidbody;

    private List<Collider> allColliders = new List<Collider>();
    private List<Rigidbody> allRigidbodies = new List<Rigidbody>();
    private Stuneable stuneable;
    private Animator mainAnimator;
    private Vector3 torsoLocalPosition;
    private bool isKinematicByDefault;
    private bool isRagdolled;

    private void Awake()
    {
        stuneable = GetComponent<Stuneable>();
        isKinematicByDefault = mainRigidbody.isKinematic;
    }

    private void Start()
    {
        mainAnimator = GetComponentInChildren<Animator>();
        torsoLocalPosition = RootRagdoll.transform.localPosition;
        DisableRagdollStep();
        stuneable.OnEnterStun.AddListener(EnableRagdoll);
        stuneable.OnExitStun.AddListener(DisableRagdoll);
    }

    private void EnableRagdoll(Vector3 hitObjectVelocity)
    {
        isRagdolled = true;
        RootRagdoll.transform.SetParent(null);
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
        DisableRagdollStep();
        StartCoroutine(ToggleAnimator(0.5f));
    }

    private void DisableRagdollStep()
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

    private IEnumerator ToggleAnimator(float time)
    {
        // Wait for "time" seconds and then set animator to "actv"
        //mainAnimator.SetTrigger("StandUp");
        mainAnimator.Play("GetUp");

        yield return new WaitForSeconds(time);

        mainAnimator.enabled = true;

        Vector3 position = RootRagdoll.transform.position;
        position.y += 1;
        transform.position = position;

        RootRagdoll.transform.SetParent(AttachRagdoll.transform);
        RootRagdoll.transform.localPosition = torsoLocalPosition;
        isRagdolled = false;
    }
}