using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    private void FixedUpdate()
    {
        var translation = Vector3.back * (GameManager.CurrentGameSpeed * Time.fixedDeltaTime);
        transform.Translate(translation);
    }
}
