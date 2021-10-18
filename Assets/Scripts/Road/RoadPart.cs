using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPart : MonoBehaviour
{
    public float length;

    private void Awake()
    {
        if (length <= 0.0001f)
        {
            Debug.LogError($"CHECK {gameObject.name} LENGTH");
            Application.Quit();
        }
    }
}
