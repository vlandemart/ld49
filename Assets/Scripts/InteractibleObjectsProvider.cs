using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractibleObjectsProvider : MonoBehaviour
{
    public LayerMask interactibleLayer;
    public float maxDistanceToInteractible = 2f;

    [NonSerialized] public InteractiveObject closestInteractive;
    [NonSerialized] public ThrowableObject closestThrowable;

    void Update()
    {
        Collider[] colliders =
            Physics.OverlapSphere(gameObject.transform.position + new Vector3(0, maxDistanceToInteractible / 2, 0),
                maxDistanceToInteractible, interactibleLayer);

        closestInteractive = GetClosestObject<InteractiveObject>(colliders);
        closestThrowable = GetClosestObject<ThrowableObject>(colliders);
    }

    private T GetClosestObject<T>(Collider[] objects)
    {
        float minDist = float.MaxValue;
        T chosenObject = default(T);
        foreach (Collider coll in objects)
        {
            T objectCasted = coll.GetComponent<T>();
            if (objectCasted == null)
            {
                continue;
            }

            if (!IsObjectRaycastable(coll.gameObject))
                continue;

            float distance = Vector3.Distance(gameObject.transform.position, coll.transform.position);
            if (distance < minDist)
            {
                chosenObject = objectCasted;
            }
        }

        return chosenObject;
    }

    private bool IsObjectRaycastable(GameObject objectToCheck)
    {
        Debug.DrawLine(transform.position, objectToCheck.transform.position, Color.blue);

        var origin = transform.position;
        var direction = objectToCheck.transform.position - transform.position;
        Ray ray = new Ray(origin, direction);

        List<RaycastHit> hitsList = Physics.RaycastAll(ray, direction.magnitude).ToList();

        hitsList.Sort((emp1, emp2) =>
        {
            float dist1 = Vector3.Distance(ray.origin, emp1.point);
            float dist2 = Vector3.Distance(ray.origin, emp2.point);
            return dist1 < dist2 ? -1 : 1;
        });

        foreach (RaycastHit hit in hitsList)
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                continue;
            }

            if (hit.transform.gameObject == objectToCheck)
            {
                return true;
            }

            return false;
        }

        return false;
    }
}