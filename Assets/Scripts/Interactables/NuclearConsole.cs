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

    [SerializeField] private float explosionForce = 300f;
    [SerializeField] private int signalsNeeded = 5;
    [SerializeField] private float countdownMaxTime = 30f;
    [SerializeField] private float timeToStabilize = 4f;
    [SerializeField] private ParticleSystem nuclearBlast;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip stableSound;
    [SerializeField] private AudioClip unstableSound;

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
        PlaySound(unstableSound);
    }

    private void MakeStable()
    {
        isUnstable = false;
        OnBecameStable?.Dispatch();
        PlaySound(stableSound);
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
        PlaySound(winSound);
        
        OnLevelFinished?.Dispatch();
    }

    private void LoseLevel()
    {
        isLevelFinished = true;
        nuclearBlast.gameObject.SetActive(true);
        nuclearBlast.Play();
        PlaySound(loseSound);
        Explode();
        
        OnLevelLost?.Dispatch();
    }
    
    private void PlaySound(AudioClip clipToPlay)
    {
        if (audioSource == null || clipToPlay == null)
            return;
        
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

    private void Explode()
    {
        var allRb = FindObjectsOfType<Rigidbody>();
        foreach (var rb in allRb)
        {
            var force = (rb.position - transform.position).normalized * explosionForce;
            rb.AddForce(force);
        }
    }
}