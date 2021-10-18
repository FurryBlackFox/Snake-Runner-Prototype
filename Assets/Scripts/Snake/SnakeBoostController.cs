using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoostController : MonoBehaviour
{
    public static event Action<float> OnBoostValueUpdated;
    public static event Action OnBoostStarted;
    public static event Action OnBoostEnded;

    [SerializeField] private ParticleSystem boostParticles;
    
    public static bool IsBoostActive { get; private set; }
    
    private float currentBoostValue;
    private float targetBoostValue;

  
    private SnakeSettings snakeSettings;

    private void Awake()
    {
        snakeSettings = SettingsManager.S.snakeSettings;
    }

    private void OnEnable()
    {
        Snake.OnJewelConsumed += OnJewelConsumedHandler;
    }

    private void OnDisable()
    {
        Snake.OnJewelConsumed -= OnJewelConsumedHandler;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            OnJewelConsumedHandler();
    }

    private void FixedUpdate()
    {
        UpdateBoostValue();
    }
    
    private void OnJewelConsumedHandler(Jewel jewel = null)
    {
        if(IsBoostActive)
            return;
        
        targetBoostValue = Mathf.Clamp01(currentBoostValue + snakeSettings.boostValuePerJewel);
    }

    private void UpdateBoostValue()
    {
        float deltaValue;

        if (IsBoostActive)
        {
            deltaValue = 1 / snakeSettings.boostDuration;
            if (Mathf.Abs(currentBoostValue) < float.Epsilon)
            {
                EndBoost();
            }
             
        }
        else
        {
            deltaValue = currentBoostValue < targetBoostValue
                ? snakeSettings.boostValueGain
                : snakeSettings.boostValueDecline;
        }
           
        currentBoostValue = Mathf.MoveTowards(currentBoostValue, targetBoostValue, deltaValue * Time.fixedDeltaTime);

        if (Mathf.Abs(currentBoostValue - targetBoostValue) < float.Epsilon)
        {
            targetBoostValue = 0f;
        }
        OnBoostValueUpdated?.Invoke(currentBoostValue);

        if (Mathf.Abs(currentBoostValue - 1f) < float.Epsilon)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        IsBoostActive = true;
        OnBoostStarted?.Invoke();
        boostParticles.Play();
    }

    private void EndBoost()
    {
        IsBoostActive = false;
        OnBoostEnded?.Invoke();
        boostParticles.Stop();
    }

}
