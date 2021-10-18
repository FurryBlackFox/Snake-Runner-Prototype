using System;    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoost : MonoBehaviour
{
    [SerializeField] private Image fillerImage;
    [SerializeField] private ParticleSystem boostParticles;
    private void OnEnable()
    {
        SnakeBoostController.OnBoostValueUpdated += UpdateValue;
        SnakeBoostController.OnBoostStarted += OnBoostStartedHandler;
        SnakeBoostController.OnBoostEnded += OnBoostEndedHandler;
    }

    private void OnDisable()
    {
        SnakeBoostController.OnBoostValueUpdated -= UpdateValue;
        SnakeBoostController.OnBoostStarted -= OnBoostStartedHandler;
        SnakeBoostController.OnBoostEnded -= OnBoostEndedHandler;
    }

    private void UpdateValue(float newValue)
    {
        fillerImage.fillAmount = newValue;
    }
    
    private void OnBoostStartedHandler()
    {
        boostParticles.Play();
    }

    private void OnBoostEndedHandler()
    {
        boostParticles.Stop();
    }
}
