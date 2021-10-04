using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PechkaDomain : MonoBehaviour
{
    public ParticleSystem onNuclearBoxEnterParticles;

    public NuclearConsole NuclearConsole;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PechkaBox>())
        {
            FindObjectOfType<NuclearCountdown>().addPoints(NuclearCountdown.pointsByPechkaHit);
            Destroy(other.gameObject);
            onNuclearBoxEnterParticles.Play();
        }
    }
}
