using System;
using TMPro;
using UnityEngine;

public class NuclearCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    private void Start()
    {
        NuclearConsole.Instance.OnBecameStable.AddListener(HideTimer);
        NuclearConsole.Instance.OnBecameUnstable.AddListener(ShowTimer);
        NuclearConsole.Instance.OnCountdownTimerChanged.AddListener(UpdateTimer);
    }

    private void ShowTimer()
    {
        countdownText.gameObject.SetActive(true);
    }

    private void HideTimer()
    {
        countdownText.gameObject.SetActive(false);
    }

    private void UpdateTimer(float time)
    {
        var minutes = (int)time / 60;
        var seconds = (int)time % 60;
        var fraction = time % 1 * 1000;
        var timerText = $"{minutes:00}:{seconds:00}:{fraction:000}";
        countdownText.text = timerText;
    }
}
