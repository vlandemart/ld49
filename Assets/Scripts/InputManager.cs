using Sigtrap.Relays;
using UnityEngine;

//All in-game controls should be stored here
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    public readonly Relay OnLeftMouseButtonUp = new Relay();
    public readonly Relay OnLeftMouseButtonDown = new Relay();
    public readonly Relay OnRightMouseButtonUp = new Relay();
    public readonly Relay OnRightMouseButtonDown = new Relay();
    
    /// <summary>
    /// Returns last valid position if no collider under cursor
    /// </summary>
    /// <returns> World position of the cursor </returns>
    public Vector3 GetCursorPosition()
    {
        if (cachedCursorPosition != Vector3.zero)
            return cachedCursorPosition;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit))
            return lastValidCursorPosition;

        var newCursorPosition = hit.point;
        cachedCursorPosition = newCursorPosition;
        lastValidCursorPosition = newCursorPosition;
        return newCursorPosition;
    }
    
    // Private

    private Vector3 cachedCursorPosition;
    private Vector3 lastValidCursorPosition;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("There are two Instances in the scene! " + Instance + ", " + this);

        Instance = this;
    }

    private void Update()
    {
        ResetCachedValues();
        TakeInput();
    }

    private void ResetCachedValues()
    {
        cachedCursorPosition = Vector3.zero;
    }

    private void TakeInput()
    {
        TakeInputLeftMouseButton();
        TakeInputRightMouseButton();
    }
    
    private void TakeInputLeftMouseButton()
    {
        if (Input.GetMouseButtonUp(0))
            OnLeftMouseButtonUp?.Dispatch();
        if (Input.GetMouseButtonDown(0))
            OnLeftMouseButtonDown?.Dispatch();
    }

    private void TakeInputRightMouseButton()
    {
        if (Input.GetMouseButtonUp(1))
            OnRightMouseButtonUp?.Dispatch();        
        if (Input.GetMouseButtonDown(1))
            OnRightMouseButtonDown?.Dispatch();
    }
}