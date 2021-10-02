using System;
using UnityEngine;

public class InteractiveResponseBlendedAnimation : InteractiveResponse
{
    public float animationSpeed = 1f;
    public string blendingParam = "blendParam";
    public Animator _animator;

    [Header("1 means open, -1 close. You can invert them for inversed logic")]
    [SerializeField] private int startLogicValue = -1;
    [SerializeField] private int doLogicValue = 1;
    [SerializeField] private int undoLogicValue = -1;
    
    private float blendParam;
    private int currIncrement;

    private void Start()
    {
        currIncrement = startLogicValue;
    }

    private void Update()
    {
        blendParam = Mathf.Clamp(blendParam + animationSpeed * Time.deltaTime * currIncrement, 0, 1) ;
        _animator.SetFloat(blendingParam, blendParam);
    }

    public override void DoResponseAction()
    {
        base.DoResponseAction();
        currIncrement = doLogicValue;
    }

    public override void UndoResponseAction()
    {
        base.UndoResponseAction();
        currIncrement = undoLogicValue;
    }
}   