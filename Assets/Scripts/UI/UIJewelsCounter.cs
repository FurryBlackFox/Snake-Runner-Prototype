using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJewelsCounter : UIObjectCounter
{
    protected override void SubscribeToEvents()
    {
        Snake.OnJewelConsumed += OnObjectCollected;
    }

    protected override void UnsubscribeFromEvents()
    {
        Snake.OnJewelConsumed -= OnObjectCollected;
    }
}
