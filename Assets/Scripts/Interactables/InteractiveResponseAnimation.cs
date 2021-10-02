using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseAnimation : InteractiveResponse
{
    public string doAnimationName;
    public string undoAnimationName;
    public string idleStateName = "Idle";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void DoResponseAction()
    {
        base.DoResponseAction();
        _animator.Play(doAnimationName);
    }

    public override void UndoResponseAction()
    {
        base.UndoResponseAction();
        _animator.Play(undoAnimationName);
    }

    public override bool IsAvailable()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(idleStateName);
    }
}
