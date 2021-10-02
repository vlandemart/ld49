using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public bool needUiText = true;

    [SerializeField] private List<InteractiveResponse> objectsToInteract = new List<InteractiveResponse>();

    public string interactionText = "Interact";
    public bool isAvailableByDefault = true;
    public bool canBeActivatedWithMouse = false;
    public bool isOneTimeInteraction;

    private bool lastActionIsDo;
    private bool isAvailable;

    private void Start()
    {
        isAvailable = isAvailableByDefault;
        lastActionIsDo = !isAvailable;

        if (objectsToInteract.Count == 0)
            Debug.LogWarning(gameObject.name + " has 0 interactable objects! Set them in the editor.");
    }

    public virtual bool IsCanInteract()
    {
        foreach (var obj in objectsToInteract)
        {
            if (!obj.IsAvailable())
                return false;
        }

        return isAvailable;
    }

    public void TryDoInteract()
    {
        SwitchInteraction();

        if (isOneTimeInteraction)
        {
            MakeUninteractable();
        }
    }

    public void TryUndoInteract()
    {
        SwitchInteraction();

        if (isOneTimeInteraction)
            isAvailable = !isAvailable;
    }

    private void SwitchInteraction()
    {
        if (lastActionIsDo)
        {
            UndoInteractInternal();
        }
        else
        {
            DoInteractInternal();
        }

        lastActionIsDo = !lastActionIsDo;
    }

    private void DoInteractInternal()
    {
        foreach (var objectToInteract in objectsToInteract)
            objectToInteract.DoResponseAction();

        Debug.Log("Do interaction with " + gameObject.name);
    }

    private void UndoInteractInternal()
    {
        foreach (var objectToInteract in objectsToInteract)
            objectToInteract.UndoResponseAction();

        Debug.Log("Undo interaction with " + gameObject.name);
    }

    private void MakeUninteractable()
    {
        isAvailable = false;
    }

    private void OnDrawGizmos()
    {
        foreach (var interactable in objectsToInteract)
        {
            Debug.DrawLine(transform.position, interactable.transform.position);
        }
    }
}