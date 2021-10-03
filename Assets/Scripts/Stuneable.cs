using System;
using Sigtrap.Relays;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stuneable : MonoBehaviour
{
    public readonly Relay<Vector3> OnEnterStun = new Relay<Vector3>();
    public readonly Relay OnExitStun = new Relay();
    
    [SerializeField] private float stunTime = 2.0f;
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
    private bool isStunned;

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

        if (isStunned && Time.time > stunEndTime)
        {
            ExitStun();
        }
    }

    private void EnterStun(Vector3 velocity)
    {
        isStunned = true;
        stunEndTime = Time.time + stunTime;

        GetComponent<ObjectThrower>()?.DropObject();
        PlaySound();
        SwitchFaces(false);
        stubEffectGo.SetActive(true);
        //OnEnterStun?.Dispatch(velocity);
    }

    private void ExitStun()
    {
        isStunned = false;

        SwitchFaces(true);
        stubEffectGo.SetActive(false);
        //OnExitStun?.Dispatch();
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