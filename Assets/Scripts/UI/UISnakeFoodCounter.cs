using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UISnakeFoodCounter : MonoBehaviour
{
    private TextMeshProUGUI textPrefab;
    private Camera mainCamera;

    private UISettings uiSettings;
    private void Awake()
    {
        mainCamera = Camera.main;
        
        uiSettings = SettingsManager.S.uiSettings;
        textPrefab = uiSettings.snakeFoodCounterTextPrefab;
    }

    private void OnEnable()
    {
        Snake.OnFoodConsumed += SpawnText;
    }

    private void OnDisable()
    {
        Snake.OnFoodConsumed -= SpawnText;
    }

    private void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }

    private void SpawnText(Food food)
    {
        var textObject = Instantiate(textPrefab, transform);
        
        if(food.foodType != Snake.CurrentFoodType && !SnakeBoostController.IsBoostActive)
            textObject.SetText("wrong");
        
        textObject.rectTransform.DOLocalMoveY(uiSettings.foodCounterTextMoveDistance, 
        uiSettings.foodCounterTextTweenTime).SetEase(uiSettings.foodCounterTextEase).OnComplete(() => Destroy(textObject.gameObject));
    }
}
