using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnakeInput : MonoBehaviour
{
    public static event Action<float> OnInput;
    
    private Camera cashedCamera;
    private SnakeSettings snakeSettings;
    private GameSettings gameSettings;
    
    private void Awake()
    {
        cashedCamera = Camera.main;
        snakeSettings = SettingsManager.S.snakeSettings;
        gameSettings = SettingsManager.S.gameSettings;

        GameManager.snakeInput = this;
    }
    
    private void OnEnable()
    {
        SnakeBoostController.OnBoostStarted += OnBoostStartedHandler;
    }

    private void OnDisable()
    {
        SnakeBoostController.OnBoostStarted -= OnBoostStartedHandler;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            CheckInput();
    }

    private void CheckInput()
    {
        if(!Snake.inputEnabled)
            return;
        
        
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        var screenRay = cashedCamera.ScreenPointToRay(Input.mousePosition);
        var value = 0f;
        if (Physics.Raycast(screenRay, out var hitInfo, snakeSettings.maxRaycastDistance,
            snakeSettings.targetRaycastLayers))
        {
            value = hitInfo.point.x;
        }
        else
        {
            var sign = Mathf.Sign(Input.mousePosition.x - 0.5f * Screen.width);
            var posOnRoad = 0.5f * gameSettings.roadWidth * sign;
            value = posOnRoad;
        }

        var maxPos = 0.5f * gameSettings.roadWidth - gameSettings.borderOffset;
        
        value = Mathf.Clamp(value, -maxPos, maxPos);

        OnInput?.Invoke(value);
    }
    
    private void OnBoostStartedHandler()
    {
        OnInput?.Invoke(0f);
    }

}
