using System;
using UnityEngine;

public class PersonalMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator Animator;

    public float speed = 6f;
    private static readonly int Velocity = Animator.StringToHash("Velocity");

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.IsStunned())
        {
            Animator.SetFloat(Velocity, 0);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            Vector3 movementVector = forward * vertical + right * horizontal;
            movementVector.Normalize();
            movementVector *= speed * Time.fixedDeltaTime;
            movementVector.y = rb.velocity.y;
            rb.velocity = movementVector;

            Animator.SetFloat(Velocity, movementVector.magnitude);
        }
        else
        {
            Animator.SetFloat(Velocity, 0);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
}