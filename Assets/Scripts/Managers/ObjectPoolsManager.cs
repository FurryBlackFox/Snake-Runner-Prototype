using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game might have freezes caused by GC //Fixed? 
/// </summary>
public class ObjectPoolsManager : MonoBehaviour //TODO: implement for all frequently created objects
{
    public static ObjectPoolMonoBeh<Food> FoodObjectPool { get; private set; }
    public static ObjectPoolMonoBeh<Jewel> JewelObjectPool { get; private set; }

    private GameSettings gameSettings;
    
    private void Awake()
    {
        gameSettings = SettingsManager.S.gameSettings;
        FoodObjectPool = new ObjectPoolMonoBeh<Food>(gameSettings.objectPoolSize, gameSettings.foodPrefab);
        JewelObjectPool = new ObjectPoolMonoBeh<Jewel>(gameSettings.objectPoolSize, gameSettings.jewelPrefab);
    }
}
