using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundOnAnimEvent : MonoBehaviour
{
    private AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    //This should be called from animation
    public void PlaySound()
    {
        sound.pitch = Random.Range(.9f, 1.1f);
        sound.Play();
    }
}