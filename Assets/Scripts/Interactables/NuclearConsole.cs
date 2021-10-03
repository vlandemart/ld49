using System;
using Sigtrap.Relays;
using UnityEngine;

public class NuclearConsole : InteractiveResponse
{
    public static NuclearConsole Instance;
    public readonly Relay<float> OnCountdownTimerChanged = new Relay<float>();
    public readonly Relay<float> OnStableTimeChanged = new Relay<float>();
    public readonly Relay OnBecameStable = new Relay();
    public readonly Relay OnBecameUnstable = new Relay();
    public readonly Relay OnLevelLost = new Relay();
    public readonly Relay OnLevelFinished = new Relay();
    
    [SerializeField] private int signalsNeeded = 5;
    [SerializeField] private float countdownMaxTime = 30f;
    [SerializeField] private float timeToStabilize = 4f;
    [SerializeField] private ParticleSystem nuclearBlast;

    private float currentTimer = 0f;
    private float currentStableTime = 0f;
    private int currentSignals = 0;
    private bool isUnstable;
    private bool isLevelFinished;

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

    public float GetTimeToStabilize()
    {
        return timeToStabilize;
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
        if (isLevelFinished)
            return;
        
        if (!isUnstable && currentSignals < signalsNeeded)
            MakeUnstable();

        if (isUnstable && currentSignals >= signalsNeeded)
            MakeStable();

        if (isUnstable)
            UpdateTimer();

        if (!isUnstable)
            Stabilize();
    }

    private void MakeUnstable()
    {
        currentStableTime = 0f;
        OnStableTimeChanged?.Dispatch(currentStableTime);
        
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
        
        if (currentTimer <= 0)
            LoseLevel();
    }

    //If nuclear is stable for N seconds - win the game
    private void Stabilize()
    {
        currentStableTime += Time.deltaTime;
        OnStableTimeChanged?.Dispatch(currentStableTime);
        
        if (currentStableTime > timeToStabilize)
            FinishLevel();
    }

    private void FinishLevel()
    {
        isLevelFinished = true;
        OnLevelFinished?.Dispatch();
    }

    private void LoseLevel()
    {
        isLevelFinished = true;
        OnLevelLost?.Dispatch();
        nuclearBlast.gameObject.SetActive(true);
        nuclearBlast.Play();
    }
}