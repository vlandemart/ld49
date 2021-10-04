using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMarkerRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;
    void FixedUpdate()
    {
        transform.RotateAround(Vector3.up,rotationSpeed);
    }
}
