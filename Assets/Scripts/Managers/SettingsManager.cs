using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager S;
    
    public SnakeSettings snakeSettings;
    public GameSettings gameSettings;
    public UISettings uiSettings;


    private void Awake()
    {
        if (!S)
            S = this;
    }
}
