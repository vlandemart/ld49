using System;
using UnityEngine;
using UnityEngine.UI;

public class AiObjectThrower : MonoBehaviour
{
    [SerializeField] private float throwForce = 10;
    [SerializeField] private Transform throwableObjectAttachTransform;

    private InteractibleObjectsProvider _provider;

    private ThrowableObject _currentThrowable;

    private Animator animator;
    private static readonly int Throw = Animator.StringToHash("Throw");

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public bool IsHoldingObject()
    {
        return _currentThrowable != null;
    }

    public bool TryHoldObject()
    {
        ThrowableObject obj = _provider.closestThrowable;
        if (obj == null)
            return false;

        if (obj.taken || obj.GetCurrentSpeed() > 0.5f)
            return false;

        if (_currentThrowable != null)
            return false;
        
        TrySetObjectAsCurrent(obj);
        return true;
    }

    private Vector3 _targetPos;
    
    public void PlayAnimThrow(Vector3 targetPos)
    {
        animator.SetTrigger(Throw);
        _targetPos = targetPos;
    }
    
    public void ThrowObject()
    {
        if (_currentThrowable == null)
            return;
        
        var startPos = throwableObjectAttachTransform.position;
        _currentThrowable.transform.LookAt(InputManager.Instance.GetCursorPosition());
        _currentThrowable.GetComponent<Rigidbody>().velocity = (_targetPos - startPos).normalized * throwForce;
        Debug.DrawLine(_targetPos, startPos, Color.cyan, 2f);

        _currentThrowable.Throw(gameObject.GetComponent<Collider>());
        _currentThrowable.gameObject.transform.parent = null;
        _currentThrowable = null;

        animator.SetLayerWeight(1, 0);
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
        animator.SetLayerWeight(1, 1);
    }
}