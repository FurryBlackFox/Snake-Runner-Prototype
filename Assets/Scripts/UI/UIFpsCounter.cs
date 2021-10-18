using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIFpsCounter : MonoBehaviour
{
    [SerializeField] private float timePeriod = 0.5f;

    private int currentFrames;
    private float startTime;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(CalculateFps());
    }


    void Update()
    {
        currentFrames++;
    }

    private IEnumerator CalculateFps()
    {
        while (enabled)
        {
            currentFrames = 0;
            startTime = Time.unscaledTime;
            
            yield return new WaitForSecondsRealtime(timePeriod);

            var count = currentFrames / (Time.unscaledTime - startTime);
            text.SetText($"Fps: {count:F1}");
        }
    }
}
