using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleThrowAnimEvent : MonoBehaviour
{
    private ObjectThrower objectThrower;
    private AiObjectThrower aiObjectThrower;

    // Start is called before the first frame update
    void Start()
    {
        objectThrower = GetComponentInParent<ObjectThrower>();
        aiObjectThrower = GetComponentInParent<AiObjectThrower>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Handle()
    {
        if (objectThrower != null)
        {
            objectThrower.ThrowObject();
        }

        if (aiObjectThrower != null)
        {
            aiObjectThrower.ThrowObject();
        }
    }
}