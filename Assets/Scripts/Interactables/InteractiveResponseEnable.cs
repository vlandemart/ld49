using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseEnable : InteractiveResponse
{
    public override void DoResponseAction()
    {
        base.DoResponseAction();
        gameObject.SetActive(true);
    }

    public override void UndoResponseAction()
    {
        base.UndoResponseAction();
        gameObject.SetActive(false);
    }
}