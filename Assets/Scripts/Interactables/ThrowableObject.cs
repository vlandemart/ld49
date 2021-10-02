using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private string throwableText = "throw me";
    
    private Rigidbody rb;
    private Collider coll;
    public bool taken;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Take(Collider carrier)
    {
        rb.isKinematic = true;
        taken = true;

        gameObject.transform.rotation = Quaternion.identity;
        Physics.IgnoreCollision(carrier, coll, true);
    }

    public void Throw(Collider carrier)
    {
        rb.isKinematic = false;
        taken = false;

        StartCoroutine(EnableCollisionWithCarrier(carrier));
    }

    private IEnumerator EnableCollisionWithCarrier(Collider carrier)
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(carrier, coll, false);
    }
}