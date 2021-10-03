using System;
using TMPro;
using UnityEngine;

public class NuclearCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text loseLevelText;
    [SerializeField] private Gradient loseLevelGradient;
    [SerializeField] private TMP_Text finishLevelText;
    [SerializeField] private Gradient finishLevelGradient;
    [SerializeField] private float gradientTime = 3f;
    
    private float finishLevelTime;
    private bool isLevelActive; //i.e not lost or won
    
    private void Start()
    {
        NuclearConsole.Instance.OnBecameStable.AddListener(HideCountdown);
        NuclearConsole.Instance.OnBecameUnstable.AddListener(ShowCountdown);
        NuclearConsole.Instance.OnCountdownTimerChanged.AddListener(UpdateCountdown);
        NuclearConsole.Instance.OnLevelFinished.AddListener(FinishLevel);
        NuclearConsole.Instance.OnLevelLost.AddListener(LoseLevel);
        
        finishLevelText.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateFinishTextColor();
    }

    private void UpdateFinishTextColor()
    {
        if (finishLevelTime <= 0)
            return;

        var time = (Time.time - finishLevelTime) % gradientTime;
        finishLevelText.color = finishLevelGradient.Evaluate(time);
        loseLevelText.color = loseLevelGradient.Evaluate(time);
    }

    private void ShowCountdown()
    {
        if (!isLevelActive)
            return;
        
        countdownText.gameObject.SetActive(true);
    }

    private void HideCountdown()
    {
        if (!isLevelActive)
            return;
        
        countdownText.gameObject.SetActive(false);
    }

    private void UpdateCountdown(float time)
    {
        var minutes = (int)time / 60;
        var seconds = (int)time % 60;
        var fraction = time % 1 * 1000;
        var timerText = $"{minutes:00}:{seconds:00}:{fraction:000}";
        countdownText.text = timerText;
    }

    private void FinishLevel()
    {
        isLevelActive = false;
        finishLevelTime = Time.time;
        finishLevelText.gameObject.SetActive(true);
    }

    private void LoseLevel()
    {
        isLevelActive = false;
        finishLevelTime = Time.time;
        loseLevelText.gameObject.SetActive(true);
    }
}
