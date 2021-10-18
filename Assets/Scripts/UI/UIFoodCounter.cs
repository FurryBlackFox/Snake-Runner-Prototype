using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFoodCounter : UIObjectCounter
{
    protected override void SubscribeToEvents()
    {
        Snake.OnRightFoodConsumed += OnObjectCollected;
    }

    protected override void UnsubscribeFromEvents()
    {
        Snake.OnRightFoodConsumed -= OnObjectCollected;
    }

    protected override void OnObjectCollected(IConsumable obj = null)
    {
        var foodObj = (Food) obj;
        base.OnObjectCollected(obj);
    }
}
