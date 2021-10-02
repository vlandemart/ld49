using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectCollision : InteractiveObject
{
    private List<GameObject> objectInside = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (IsCanInteract() && objectInside.Count == 0)
        {
            TryDoInteract();
        }

        if (!objectInside.Contains(other.gameObject))
            objectInside.Add(other.gameObject);
    }

    private void Update()
    {
        for (int i = objectInside.Count - 1; i >= 0; i--)
        {
            if (!objectInside[i].activeSelf)
                objectInside.Remove(objectInside[i]);
            
            if (objectInside.Count == 0)
                TryDoInteract();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectInside.Remove(other.gameObject);

        if (objectInside.Count > 0)
            return;

        if (IsCanInteract())
        {
            TryDoInteract();
            objectInside.Remove(other.gameObject);
        }
    }
}