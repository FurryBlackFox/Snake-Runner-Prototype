using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIObjectCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image objectImage;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private bool enableTextTween = true;
    [SerializeField] private bool enableImageTween = true;
    
    
    private UISettings uiSettings;
    private int currentCount;


    private Tweener imageTweener;
    private Tweener textTweener;
    private void Awake()
    {
        uiSettings = SettingsManager.S.uiSettings;
        countText.SetText($"{currentCount}");
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    protected abstract void SubscribeToEvents();
    protected abstract void UnsubscribeFromEvents();

    protected virtual void OnObjectCollected(IConsumable obj = null)
    {
        currentCount++;
        if (currentCount == 100)
        {
            var size = countText.rectTransform.sizeDelta;
            size.x *= 1.25f;
            countText.rectTransform.sizeDelta = size;
        }
        
        countText.SetText($"{currentCount}");

        if (enableImageTween)
        {
            imageTweener?.Complete();
            imageTweener = objectImage.rectTransform.DOPunchScale(Vector3.one * uiSettings.counterTweenAdditiveScale,
                uiSettings.counterTweenTime);
        }

        if (enableTextTween)
        {
            textTweener?.Complete();
            textTweener = countText.rectTransform.DOPunchScale(Vector3.one * uiSettings.counterTweenAdditiveScale,
                uiSettings.counterTweenTime);
        }
        
        if(particles)
            particles.Play();
    }
}
