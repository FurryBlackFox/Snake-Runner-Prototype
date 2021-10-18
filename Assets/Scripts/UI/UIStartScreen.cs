using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
public class UIStartScreen : UIWindow
{
    [SerializeField] private TextMeshProUGUI text;

    private UISettings uiSettings;

    private Tweener textTweener;
    protected override void Awake()
    {
        base.Awake();

        uiSettings = SettingsManager.S.uiSettings;
        StartScalingText();
    }

    public void StartScalingText()
    {
        textTweener = text.rectTransform.DOPunchScale(Vector3.one * uiSettings.textTweenAdditiveScale, uiSettings.textTweenTime, 
        uiSettings.textTweenVibratio).SetLoops(-1);
    }

    public void StopScalingText()
    {
        textTweener.Rewind();
    }

    public override void Show(float delay = 0)
    {
        base.Show(delay);
        StartScalingText();
        UpdateState(true);
    }

    public override void Hide(float delay = 0)
    {
        base.Hide(delay);
        StopScalingText();
        UpdateState(false);
    }
}
