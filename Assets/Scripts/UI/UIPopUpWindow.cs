using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster), typeof(Canvas))]
public class UIPopUpWindow : UIWindow
{
    [SerializeField] private RectTransform popUpWindow;
    [SerializeField] private UIScreenDimmer uiScreenDimmer;
    
    
    private float initialPopUpWindowPosX;
    private UISettings uiSettings;
    protected override void Awake()
    {
        base.Awake();
        
        uiSettings = SettingsManager.S.uiSettings;
        initialPopUpWindowPosX = popUpWindow.localPosition.x;
    }

    public void ShowWithDimmer(float delay = 0)
    {
        Show(delay);
        uiScreenDimmer.Show(delay);
    }
    
    public override void Show(float delay = 0)
    {
        OnShowStarted();
        graphicRaycaster.enabled = true;
        popUpWindow.DOLocalMoveX(0f, uiSettings.popUpWindowTweenTime).SetEase(uiSettings.popUpWindowShowEase).SetDelay
                (uiSettings.screenDimmerTweenTime + delay).OnComplete(OnShowFinished);
    }

    public void HideWithDimmer()
    {
        Hide();
        uiScreenDimmer.Hide(uiSettings.popUpWindowTweenTime);
    }
    
    public override void Hide(float delay = 0f)
    { 
        OnHideStarted();
        popUpWindow.DOLocalMoveX(initialPopUpWindowPosX, uiSettings.popUpWindowTweenTime).SetEase(uiSettings
            .popUpWindowHideEase).OnComplete(OnHideFinished);
    }

}
