using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationToMouse : MonoBehaviour
{
    [SerializeField] private float rotationStepDegrees = 30.0f;
    [SerializeField] private float offsetDegrees = 45.0f;
    [SerializeField] private float rotateDegreesInStun = 0.0f;
    private Stuneable stunable;

    private void Start()
    {
        stunable = GetComponentInParent<Stuneable>();
    }

    private void Update()
    {
        if (stunable != null && stunable.IsStunned())
        {
            transform.rotation = Quaternion.Euler(0, rotateDegreesInStun, 0);
            return;
        }

        var mousePos = Input.mousePosition;
        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        var vec = mousePos - playerPos;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360.0f;
        }

        int angleFullParts = (int)(angle / rotationStepDegrees);
        float angleRemainder = angle % rotationStepDegrees;

        if (angleRemainder > rotationStepDegrees * 0.5f)
        {
            angleFullParts++;
        }

        float angleSnapped = rotationStepDegrees * angleFullParts;

        // Debug.Log("angle = " + angle + " snapped = " + angleSnapped);

        transform.rotation = Quaternion.Euler(0, -angleSnapped - offsetDegrees, 0);
    }
}