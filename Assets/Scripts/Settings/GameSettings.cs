using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "Game Settings", order = 1, menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Range(-1, 120)] public int targetFrameRate = -1;
    [Min(0)] public float timeToCompletelyStopTime = 0.5f;
    [Min(0)] public float restartDelay = 2f;
    
    [Header("Game Speed")]
    [Min(0f)] public float gameSpeed = 10f;
    [Min(0f)] public float gameSpeedGain = 100f;
    [Min(0f)] public float burstGameSpeed = 30f;
    
    [Header("Road Info")]
    [Min(0f)] public float borderOffset = 0.375f;
    [Min(0f)] public float roadWidth = 7.5f;
    
    [Header("Road Generation")]
    [Min(0f)] public float platformSpawnOffset = 50f;
    [Range(-100f, 0f)] public float platformDestructionOffset = -20f;
    [Range(0, 30)] public int roadPartsToNewCheckPoint = 5;
    [Min(0)] public int initialDefaultRoadsCount = 2;
    [Range(0f, 1f)] public float probabilityToSpawnJewelsPlatform = 0.5f; 
    public RoadPart checkPointRoadPartPrefab;
    public RoadPart defaultRoadPartPrefab;
    public List<RoadPart> foodRoadParts;
    public List<RoadPart> jewelsRoadParts;    
    
    [Header("Water Manager")]
    public float minWaterPosZ = -30f;
    [Min(0f)] public float waterPlaneLength = 10f;

    [Header("Jewel")]
    public float jewelRotationSpeed = 10f;

    [Header("Objects Generation")]
    [Min(0f)] public float foodSpawnerRadius = 5f;
    [Min(0f)] public int maxFoodSpawnCount = 5;
    [Min(0)] public int maxWrongSpawnAtteptsCount = 3;
    [Min(0f)] public float minSqrDistanceBetweenFood = 1f;
    [Range(0f, 1f)] public float probabilityToSpawnCurrentFoodType = 0.5f;
    [Range(0f, 1f)] public float probabilityToSpawnJewel = 0.25f;
    public List<FoodType> foodTypes;

    [Header("Object Pool")]
    [Min(0)] public int objectPoolSize = 50;
    public Food foodPrefab;
    public Jewel jewelPrefab;
}
