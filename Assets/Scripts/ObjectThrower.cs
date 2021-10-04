using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    [SerializeField] private float throwForce = 10;
    [SerializeField] private GameObject targetPositionMarker;
    [SerializeField] private Transform throwableObjectAttachTransform;
    [SerializeField] private float maxThrowDistance = 10f;
    [SerializeField] private AudioSource throwSound;

    private InteractibleObjectsProvider _provider;

    private ThrowableObject _currentThrowable;
    private Vector3 ogPosition;

    private Animator animator;
    private static readonly int Throw = Animator.StringToHash("Throw");

    public bool IsHoldingObject()
    {
        return _currentThrowable != null;
    }

    private void Awake()
    {
        _provider = gameObject.GetComponent<InteractibleObjectsProvider>();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        InputManager.Instance.OnLeftMouseButtonDown.AddListener(PlayAnim);
        ogPosition = transform.position;
    }

    private void Update()
    {
        DrawAim();

        ThrowableObject obj = _provider.closestThrowable;
        if (obj == null)
        {
            return;
        }

        if (_currentThrowable == null && Input.GetKeyDown(KeyCode.E))
        {
            TrySetObjectAsCurrent(obj);
            return;
        }

        if (_currentThrowable == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DropObject();
        }
    }

    private void DrawAim()
    {
        Vector3 markerPos = InputManager.Instance.GetCursorPosition();
        Vector3 dir = markerPos - gameObject.transform.position;
        float dist = dir.magnitude;

        if (dist > maxThrowDistance)
        {
            dir = dir.normalized * _provider.maxDistanceToInteractible;
            markerPos = gameObject.transform.position + dir;
        }

        targetPositionMarker.transform.position = markerPos;
    }

    private void TrySetObjectAsCurrent(ThrowableObject obj)
    {
        if (obj.taken)
            return;

        obj.Take(gameObject.GetComponent<Collider>());
        _currentThrowable = obj;

        _currentThrowable.transform.parent = throwableObjectAttachTransform;
        _currentThrowable.transform.position = throwableObjectAttachTransform.position;
        _currentThrowable.transform.rotation = throwableObjectAttachTransform.rotation;

        targetPositionMarker.SetActive(true);
        animator.SetLayerWeight(1, 1);
    }

    public void DropObject()
    {
        if (_currentThrowable == null)
            return;

        _currentThrowable.Throw(gameObject.GetComponent<Collider>());
        _currentThrowable.gameObject.transform.parent = null;
        _currentThrowable = null;

        targetPositionMarker.SetActive(false);
        animator.SetLayerWeight(1, 0);
    }

    //Called on LMB event
    public void PlayAnim()
    {
        animator.SetTrigger(Throw);
    }

    public void ThrowObject()
    {
        if (_currentThrowable == null)
            return;

        var startPos = transform.position;
        var throwPos = targetPositionMarker.transform.position;

        _currentThrowable.transform.LookAt(InputManager.Instance.GetCursorPosition());
        _currentThrowable.GetComponent<Rigidbody>().velocity = (throwPos - startPos).normalized * throwForce;

        _currentThrowable.Throw(gameObject.GetComponent<Collider>());
        _currentThrowable.gameObject.transform.parent = null;
        _currentThrowable = null;

        targetPositionMarker.SetActive(false);
        animator.SetLayerWeight(1, 0);
        PlaySound();
    }
    
    public void PlaySound()
    {
        throwSound.pitch = Random.Range(.9f, 1.1f);
        throwSound.Play();
    }
}