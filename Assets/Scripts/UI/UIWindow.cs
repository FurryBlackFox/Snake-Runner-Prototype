using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{
    [SerializeField] protected UnityEvent onShowStarted;
    [SerializeField] protected UnityEvent onShowFinished;
    [SerializeField] protected UnityEvent onHideStarted;
    [SerializeField] protected UnityEvent onHideFinished;
    [SerializeField] protected bool enabledOnStart = false;
    protected Canvas canvas;
    protected GraphicRaycaster graphicRaycaster;


    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        UpdateState(enabledOnStart);
    }

    public virtual void Show(float delay = 0f)
    {
        OnShowStarted();
    }

    public virtual void Hide(float delay = 0f)
    {
        OnHideStarted();
    }

    protected void UpdateState(bool state)
    {
        UpdateCanvasState(state);
        UpdateRaycasterState(state);
    }

    protected void UpdateCanvasState(bool state)
    {
        if (canvas)
            canvas.enabled = state;
    }

    protected void UpdateRaycasterState(bool state)
    {
        if (graphicRaycaster)
            graphicRaycaster.enabled = state;   
    }

    protected void OnHideStarted()
    {
        UpdateRaycasterState(false);
        onHideStarted?.Invoke();
    }

    protected void OnHideFinished()
    {
        UpdateCanvasState(false);
        onHideFinished?.Invoke();
    }

    protected void OnShowStarted()
    {
        UpdateCanvasState(true);
        onShowStarted?.Invoke();
    }

    protected void OnShowFinished()
    {
        UpdateRaycasterState(true);
        onShowFinished?.Invoke();

    }
}
