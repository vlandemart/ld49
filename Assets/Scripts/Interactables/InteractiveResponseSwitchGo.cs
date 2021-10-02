using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseSwitchGo : InteractiveResponse
{
    public GameObject defaultGo;
    public GameObject switchedGo;

    private void Start()
    {
        if (defaultGo == null || switchedGo == null)
            return;
        defaultGo.SetActive(true);
        switchedGo.SetActive(false);
    }

    public override void DoResponseAction()
    {
        base.DoResponseAction();
        if (defaultGo == null || switchedGo == null)
            return;
        defaultGo.SetActive(false);
        switchedGo.SetActive(true);
    }

    public override void UndoResponseAction()
    {
        base.UndoResponseAction();
        Debug.Assert(false);
    }
}