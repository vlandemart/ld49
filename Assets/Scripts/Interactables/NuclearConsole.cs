using System;
using Sigtrap.Relays;
using UnityEngine;

public class NuclearConsole : InteractiveResponse
{
    public static NuclearConsole Instance;
    public Relay<float> OnCountdownTimerChanged = new Relay<float>();
    public Relay OnBecameStable = new Relay();
    public Relay OnBecameUnstable = new Relay();
    
    [SerializeField] private int signalsNeeded = 5;
    [SerializeField] private float countdownMaxTime = 30f;

    private float currentTimer = 0f;
    private int currentSignals = 0;
    private bool isUnstable;

    public override void DoResponseAction()
    {
        base.DoResponseAction();

        currentSignals++;
    }

    public override void UndoResponseAction()
    {
        base.UndoResponseAction();

        currentSignals--;
    }

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Several instances of singleton in the scene!");
        Instance = this;
    }

    private void Start()
    {
        currentTimer = countdownMaxTime;
    }

    private void Update()
    {
        if (!isUnstable && currentSignals < signalsNeeded)
            MakeUnstable();

        if (isUnstable && currentSignals >= signalsNeeded)
            MakeStable();

        if (isUnstable)
            UpdateTimer();
    }

    private void MakeUnstable()
    {
        isUnstable = true;
        OnBecameUnstable?.Dispatch();
    }

    private void MakeStable()
    {
        isUnstable = false;
        OnBecameStable?.Dispatch();
    }

    private void UpdateTimer()
    {
        currentTimer -= Time.deltaTime;
        OnCountdownTimerChanged?.Dispatch(currentTimer);
        
        //if timer < 0
        //  explode()
    }
}