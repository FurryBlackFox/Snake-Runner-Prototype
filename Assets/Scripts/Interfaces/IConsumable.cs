using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    public Transform GetTransform();

    public void OnConsumeStarted();
    
    public void Destroy();
}
