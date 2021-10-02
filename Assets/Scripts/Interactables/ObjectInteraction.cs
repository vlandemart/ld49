using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    private Text uiInteractiveTextBox;
    private InteractibleObjectsProvider _provider;
    private ObjectThrower _thrower;

    private void Awake()
    {
        _provider = gameObject.GetComponent<InteractibleObjectsProvider>();
        _thrower = gameObject.GetComponent<ObjectThrower>();

        var textObject = GameObject.FindWithTag("interactableTextBox");
        if (textObject != null)
        {
            uiInteractiveTextBox = textObject.GetComponent<Text>();
        }
    }

    void Update()
    {
        TrySetText("");

        InteractiveObject obj = _provider.closestInteractive;
        if (obj == null)
        {
            return;
        }

        if (obj.needUiText)
        {
            if (obj.IsCanInteract())
            {
                TrySetText("Press 'E' to " + obj.interactionText);
            }
            else
            {
                TrySetText(obj.interactionText + " (unavailable)");
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && obj.canBeActivatedWithMouse &&
            (_thrower == null || !_thrower.IsHoldingObject()))
        {
            if (obj.IsCanInteract())
            {
                obj.TryDoInteract();
            }
        }
    }

    void TrySetText(string value)
    {
        if (uiInteractiveTextBox != null)
        {
            uiInteractiveTextBox.text = value;
            uiInteractiveTextBox.enabled = value != "";
        }
    }
}