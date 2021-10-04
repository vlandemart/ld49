using System;
using Sigtrap.Relays;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stuneable : MonoBehaviour
{
    public readonly Relay<Vector3> OnEnterStun = new Relay<Vector3>();
    public readonly Relay OnPrepareExitStun = new Relay();
    public readonly Relay OnExitStun = new Relay();
    
    [SerializeField] private float stunTime = 2.0f;
    [SerializeField] private float preStunTime = 1f; //Just don't change it, it is for animator in OnRagdollDeath()
    [SerializeField] private float speedToStun = 8f;
    [SerializeField] private bool stunFromEditor;
    [SerializeField] private GameObject stubEffectGo;
    [SerializeField] private AudioSource stunSound;
    [SerializeField] private GameObject aliveFace;
    [SerializeField] private GameObject deadFace;

    public bool IsStunned()
    {
        return isStunned;
    }

    public void Stun(Vector3 velocity)
    {
        if (IsStunned())
            return;
        if (velocity.magnitude < speedToStun)
            return;

        EnterStun(velocity);
    }
    
    //Private
    
    private float stunEndTime;
    private float preExitStunTime;
    private bool isStunned;
    private bool hasPreExitedStun;

    private void Start()
    {
        SwitchFaces(true);
        stubEffectGo.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isStunned)
                ExitStun();
            else
                EnterStun(Vector3.zero);
        }
        
        if (stunFromEditor)
        {
            stunFromEditor = false;
            Stun(Vector3.zero);
        }

        if (!hasPreExitedStun && Time.time > preExitStunTime)
        {
            PrepareExitStun(); 
        }

        if (isStunned && Time.time > stunEndTime)
        {
            ExitStun();
        }
    }

    private void EnterStun(Vector3 velocity)
    {
        isStunned = true;
        hasPreExitedStun = false;
        stunEndTime = Time.time + stunTime;
        preExitStunTime = stunEndTime - preStunTime;

        GetComponent<ObjectThrower>()?.DropObject();
        PlaySound();
        SwitchFaces(false);
        stubEffectGo.SetActive(true);
        OnEnterStun?.Dispatch(velocity);
    }

    private void PrepareExitStun()
    {
        hasPreExitedStun = true;
        SwitchFaces(true);
        OnPrepareExitStun?.Dispatch();
    }

    private void ExitStun()
    {
        isStunned = false;
        stubEffectGo.SetActive(false);
        OnExitStun?.Dispatch();
    }

    private void PlaySound()
    {
        if (stunSound == null)
            return;
        stunSound.pitch = Random.Range(0.9f, 1.1f);
        stunSound.Play();
    }

    private void SwitchFaces(bool isAlive)
    {
        deadFace.SetActive(!isAlive);
        aliveFace.SetActive(isAlive);
    }
}

public static class ShitExtensions
{
    public static bool IsStunned(this MonoBehaviour beh)
    {
        var stunable = beh.GetComponent<Stuneable>();
        return stunable != null && stunable.IsStunned();
    }
}