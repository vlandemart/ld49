using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseDisable : InteractiveResponse
{
    public override void DoResponseAction()
    {
        base.DoResponseAction();
        gameObject.SetActive(false);
    }

    public override void UndoResponseAction()
    {
        base.UndoResponseAction();
        gameObject.SetActive(true);
    }
}
