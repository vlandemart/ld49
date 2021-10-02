using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stuneable : MonoBehaviour
{
    [SerializeField] private float stunTime = 2.0f;
    [SerializeField] private bool stunFromEditor;

    [SerializeField] private GameObject stubEffectGo;
    [SerializeField] private AudioSource stunSound;

    [SerializeField] private GameObject aliveFace;
    [SerializeField] private GameObject deadFace;

    float stunEndTime;

    private RagdollOnDeath ragdollOnDeath;

    private void Awake()
    {
        ragdollOnDeath = GetComponent<RagdollOnDeath>();
    }

    public bool IsStunned()
    {
        return Time.time < stunEndTime;
    }

    public void Stun(GameObject damager)
    {
        if (IsStunned())
            return;

        ragdollOnDeath.EnableRagdoll(damager);
        aliveFace.SetActive(false);
        deadFace.SetActive(true);

        stunEndTime = Time.time + stunTime;
        GetComponent<ObjectThrower>()?.ThrowObject();

        if (stunSound == null)
            return;
        stunSound.pitch = Random.Range(0.9f, 1.1f);
        stunSound.Play();
    }

    private void Update()
    {
        if (stunFromEditor)
        {
            stunFromEditor = false;
            Stun(gameObject);
        }

        //stubEffectGo.SetActive(IsStunned());
    }
}

public static class ShitExtensions
{
    public static bool IsStunned(this MonoBehaviour beh)
    {
        var stuneable = beh.GetComponent<Stuneable>();
        return stuneable != null && stuneable.IsStunned();
    }
}