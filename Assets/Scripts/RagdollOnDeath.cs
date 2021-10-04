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

    private Rigidbody[] bones;
    private Quaternion[] rotations;
    
    public float rotationSpeed = 5f;
    private bool blendEnabled;

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

        bones = GetCompNoRoot<Rigidbody>(RootRagdoll);
        rotations = new Quaternion[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            rotations[i] = bones[i].transform.localRotation;
        }
    }

    T[] GetCompNoRoot<T>(GameObject obj) where T : Component
    {
        List<T> tList = new List<T>();
        T[] comps = GetComponentsInChildren<T>();
        foreach (T comp in comps)
        {
            if (comp.gameObject.GetInstanceID() != obj.GetInstanceID())
            {
                tList.Add(comp);
            }
        }

        return tList.ToArray();
    }

    private void EnableRagdoll(Vector3 hitObjectVelocity)
    {
        if (transform.name != "Player")
        {
            FindObjectOfType<NuclearCountdown>().addPoints(NuclearCountdown.pointsByAIHit);
        }
        
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
        blendEnabled = true;
        StartCoroutine(ToggleAnimator(1f));
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

    void FixedUpdate()
    {
        if (blendEnabled)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].transform.localRotation =
                    Quaternion.Lerp(bones[i].transform.localRotation, rotations[i], Time.deltaTime * rotationSpeed);
            }
        }
    }

    private IEnumerator ToggleAnimator(float time)
    {
        // Wait for "time" seconds and then set animator to "actv"
        //mainAnimator.SetTrigger("StandUp");
        mainAnimator.Play("GetUp");

        yield return new WaitForSeconds(time);

        blendEnabled = false;
        mainAnimator.enabled = true;

        Vector3 position = RootRagdoll.transform.position;
        position.y += 1;
        transform.position = position;

        RootRagdoll.transform.SetParent(AttachRagdoll.transform);
        RootRagdoll.transform.localPosition = torsoLocalPosition;
        isRagdolled = false;
    }
}