using System.Collections.Generic;
using UnityEngine;

public class NuclearIndicators : MonoBehaviour
{
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private List<GameObject> indicators;
    [SerializeField] private List<AudioClip> indicatorSounds;
    [SerializeField] private AudioSource audioSource;

    private float maxTime;
    private bool[] indicatorStatus;
    
    private void Start()
    {
        NuclearConsole.Instance.OnStableTimeChanged.AddListener(UpdateIndicators);
        maxTime = NuclearConsole.Instance.GetTimeToStabilize();
        indicatorStatus = new bool[indicators.Count];
    }

    private void UpdateIndicators(float currentTime)
    {
        var timePerIndicator = maxTime / indicators.Count;
        for (int i = 0; i < indicators.Count; i++)
        {
            SetIndicator(i, (currentTime >= timePerIndicator * (i + 1)));
        }
    }

    private void SetIndicator(int index, bool status)
    {
        if (indicatorStatus[index] == status)
            return;

        indicatorStatus[index] = status;
        var indicatorRenderer = indicators[index].GetComponent<Renderer>();
        indicatorRenderer.material = status ? enabledMaterial : disabledMaterial;
        if (status == true)
            PlaySound(index);
    }
    
    private void PlaySound(int index)
    {
        if (audioSource == null)
            return;
        
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.clip = indicatorSounds[index];
        audioSource.Play();
    }
}
