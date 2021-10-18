using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New UI Settings", order = 3, menuName = "Scriptable Objects/UI Settings")]
public class UISettings : ScriptableObject
{
    [Header("Menu Items")]
    [Min(0f)] public float screenDimmerTweenTime = 0.5f;
    [Min(0f)] public float startScreenDimmerTweenTime = 1f;
    [Min(0f)] public float popUpWindowTweenTime = 0.5f;
    public Ease popUpWindowShowEase = Ease.OutElastic;
    public Ease popUpWindowHideEase = Ease.InOutBack;

    [Header("Object Counters")]
    [Min(0f)] public float counterTweenTime = 0.25f;
    [Min(0f)] public float counterTweenAdditiveScale = 0.25f;
    
    [Header("Snake's Counter")]
    public TextMeshProUGUI snakeFoodCounterTextPrefab;
    [Min(0f)] public float foodCounterTextMoveDistance = 2f;
    [Min(0f)] public float foodCounterTextTweenTime = 1f;
    public Ease foodCounterTextEase = Ease.InQuad;

    [Header("Start Screen")] 
    [Min(0f)] public float textTweenAdditiveScale = 0.1f;
    [Min(0f)] public float textTweenTime = 0.5f;
    [Min(0)] public int textTweenVibratio = 1;
}
