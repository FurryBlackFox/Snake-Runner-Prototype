using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game might have freezes caused by GC
/// </summary>
public class ObjectPoolsManager : MonoBehaviour //TODO: implement for all frequently created objects
{
    public static ObjectPoolMonoBeh<Food> FoodObjectPool { get; private set; }
    public static ObjectPoolMonoBeh<Jewel> JewelObjectPool { get; private set; }
    public static ObjectPool<Marker> MarkerObjectPool { get; private set; }

    private GameSettings gameSettings;
    
    private void Awake()
    {
        gameSettings = SettingsManager.S.gameSettings;
        MarkerObjectPool = new ObjectPool<Marker>(5 * gameSettings.objectPoolSize); //seems like a good idea but a crooked implementation  //TODO: fix
        FoodObjectPool = new ObjectPoolMonoBeh<Food>(gameSettings.objectPoolSize, gameSettings.foodPrefab);
        JewelObjectPool = new ObjectPoolMonoBeh<Jewel>(gameSettings.objectPoolSize, gameSettings.jewelPrefab);
    }
}
