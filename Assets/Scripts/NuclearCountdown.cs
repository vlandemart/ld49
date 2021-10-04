using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NuclearCountdown : MonoBehaviour
{
    
    
    [SerializeField] private Slider slider;
    
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text loseLevelText;
    [SerializeField] private Gradient loseLevelGradient;
    [SerializeField] private TMP_Text finishLevelText;
    [SerializeField] private Gradient finishLevelGradient;
    [SerializeField] private float gradientTime = 3f;
    
    private float finishLevelTime;
    private bool isLevelActive; //i.e not lost or won

    public TMP_Text points_counter;
    public CanvasGroup score_highlight;
    
    [HideInInspector]
    public static int POINTS;

    
    public static int pointsByTime = 10;
    public static int pointsByPechkaHit = 100;
    public static int pointsByAIHit = 100;

    private Coroutine coroutine;
    
    private void Start()
    {
        NuclearConsole.Instance.OnBecameStable.AddListener(HideCountdown);
        NuclearConsole.Instance.OnBecameUnstable.AddListener(ShowCountdown);
        NuclearConsole.Instance.OnCountdownTimerChanged.AddListener(UpdateCountdown);
        NuclearConsole.Instance.OnLevelFinished.AddListener(FinishLevel);
        NuclearConsole.Instance.OnLevelLost.AddListener(LoseLevel);
        
        finishLevelText.gameObject.SetActive(false);

        coroutine = StartCoroutine(addPointsOnTime());
    }

    IEnumerator addPointsOnTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            addPoints(pointsByTime); 
        }
       
    }
    
    IEnumerator addHeatOnTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            addPoints(pointsByTime); 
        }
       
    }

    public void reduceTemperature()
    {
        
    }

    public void addPoints(int amount)
    {
        POINTS += amount;
        if (amount > 10)
        {
            StartCoroutine(scoreHighLight());
        }
    }

   

    IEnumerator scoreHighLight()
    {
        score_highlight.alpha = 1f;
        yield return new WaitForSeconds(0.1f);
        score_highlight.alpha = 0f;

        
    }

    private void Update()
    {
        UpdateFinishTextColor();
        points_counter.text = POINTS.ToString();
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
        //StopCoroutine(coroutine);

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
        StopCoroutine(coroutine);
    }

    private void LoseLevel()
    {
        isLevelActive = false;
        finishLevelTime = Time.time;
        loseLevelText.gameObject.SetActive(true);
        StopCoroutine(coroutine);

    }
}
