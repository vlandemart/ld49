using System;
using UnityEngine;
using UnityEngine.UI;

public class AiObjectThrower : MonoBehaviour
{
    [SerializeField] private float throwForce = 10;
    [SerializeField] private Transform throwableObjectAttachTransform;

    private InteractibleObjectsProvider _provider;

    private ThrowableObject _currentThrowable;

    public bool IsHoldingObject()
    {
        return _currentThrowable != null;
    }

    public bool TryHoldObject()
    {
        ThrowableObject obj = _provider.closestThrowable;
        if (obj != null && _currentThrowable == null && !obj.taken)
        {
            TrySetObjectAsCurrent(obj);
            return true;
        }

        return false;
    }

    public void ThrowObject(Vector3 targetPos)
    {
        if (_currentThrowable == null)
            return;

        var startPos = transform.position;
        _currentThrowable.transform.LookAt(InputManager.Instance.GetCursorPosition());
        _currentThrowable.GetComponent<Rigidbody>().velocity = (targetPos - startPos).normalized * throwForce;

        _currentThrowable.Throw(gameObject.GetComponent<Collider>());
        _currentThrowable.gameObject.transform.parent = null;
        _currentThrowable = null;
    }

    private void Awake()
    {
        _provider = gameObject.GetComponent<InteractibleObjectsProvider>();
    }

    private void TrySetObjectAsCurrent(ThrowableObject obj)
    {
        if (obj.taken)
            return;

        obj.Take(gameObject.GetComponent<Collider>());
        _currentThrowable = obj;

        _currentThrowable.transform.parent = throwableObjectAttachTransform;
        _currentThrowable.transform.localPosition = Vector3.zero;
    }
}