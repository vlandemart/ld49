using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector3 moveVector;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        TakeInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void TakeInputs()
    {
        moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVector.Normalize();
    }

    private void Move()
    {
        rb.velocity = moveVector * speed;
    }
}
