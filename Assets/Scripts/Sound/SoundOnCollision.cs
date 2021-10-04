using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    [Header("This script is intended to be used on dynamic objects")]
    [SerializeField] private AudioSource soundToPlay;
    [SerializeField] private float minVelocity = 3f;

    private void OnCollisionEnter(Collision other)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < minVelocity)
            return;

        PlaySound();
    }

    private void PlaySound()
    {
        soundToPlay.pitch = Random.Range(.9f, 1.1f);
        soundToPlay.Play();
    }
}