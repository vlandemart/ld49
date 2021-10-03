using System.Collections.Generic;
using UnityEngine;

public class NuclearIndicators : MonoBehaviour
{
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private List<GameObject> indicators;

    private float maxTime;
    
    private void Start()
    {
        NuclearConsole.Instance.OnStableTimeChanged.AddListener(UpdateIndicators);
        maxTime = NuclearConsole.Instance.GetTimeToStabilize();
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
        var indicatorRenderer = indicators[index].GetComponent<Renderer>();
        indicatorRenderer.material = status ? enabledMaterial : disabledMaterial;
    }
}
