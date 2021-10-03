using System;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerFollow;
    [SerializeField] private Transform rigidbodyFollow;
    [SerializeField] private float lerpSpeed = .5f;
    [SerializeField] private Vector3 cameraOffset;

    private RagdollOnDeath playerRagdoll;

    private void Start()
    {
        playerRagdoll = playerFollow.transform.GetComponent<RagdollOnDeath>();
    }

    private void Update()
    {
        Vector3 movePos;

        if (playerRagdoll.IsRagdolled())
            movePos = rigidbodyFollow.position + cameraOffset;
        else
            movePos = playerFollow.position + cameraOffset;
        transform.position = Vector3.Lerp(transform.position, movePos, lerpSpeed);
    }
}
