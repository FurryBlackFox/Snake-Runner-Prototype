using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIScreenDimmer : UIWindow
{
    private Image image;
    private Color opaqueColor;
    private Color transparentColor;
    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
        image.enabled = true;
        opaqueColor = image.color;
        transparentColor = opaqueColor;
        transparentColor.a = 0;

        if(enabledOnStart)
            HideOnStart();
        else
            image.color = transparentColor;
  
    }

    public override void Show(float delay = 0f)
    {
        base.Show(delay);
        image.DOColor(opaqueColor, SettingsManager.S.uiSettings.screenDimmerTweenTime).SetDelay(delay).OnComplete(OnShowFinished);
    }

    public override void Hide(float delay = 0f)
    {
        base.Hide(delay);
        image.DOColor(transparentColor, SettingsManager.S.uiSettings.screenDimmerTweenTime).SetDelay(delay).OnComplete(OnHideFinished);
    }

    private void HideOnStart()
    {
        base.Hide();
        image.DOColor(transparentColor, SettingsManager.S.uiSettings.startScreenDimmerTweenTime).SetEase(Ease.Linear)
        .OnComplete(OnHideFinished);
    }


}
