using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoadSpawnType
{
    Default,
    CheckPoint,
    RandomFromList
}

public class RoadManager : MonoBehaviour
{
    public bool spawnOnlyDefault = false;
    
    private List<RoadPart> roadParts;
    private GameSettings gameSettings;

    private int roadPartsSinceCheckPoint = 0;

    private bool isUpdatingInFixed = true;
    private void Awake()
    {
        roadParts = new List<RoadPart>();
    }

    private void OnEnable()
    {
        GameManager.OnPlayerDeath += OnPlayerDeathHandler;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerDeath -= OnPlayerDeathHandler;
    }

    private void Start()
    {
        gameSettings = SettingsManager.S.gameSettings;
        InitialSpawnRoadParts();
    }

    
    
    private void FixedUpdate()
    {
        if(isUpdatingInFixed)
            TranslateObjects(Time.fixedDeltaTime);
    }

    private void Update()
    {
        if(!isUpdatingInFixed)
            TranslateObjects(Time.deltaTime);
    }


    private void OnPlayerDeathHandler()
    {
        isUpdatingInFixed = false;
    }
    

    private void TranslateObjects(float deltaTime)
    {
                
        var translation = Vector3.back * (GameManager.CurrentGameSpeed * deltaTime);

        RoadPart partToDestroy = null;
        foreach (var roadPart in roadParts)
        {
            roadPart.transform.Translate(translation, Space.World);
            if (roadPart.transform.position.z <= gameSettings.platformDestructionOffset)
            {
                partToDestroy = roadPart;
            }
        }
        
        if(partToDestroy)
            DestroyRoadPart(partToDestroy);
        
        CheckForSpawnNeed();
    }

    private void InitialSpawnRoadParts()
    {
        var currentPos = new Vector3(0f, transform.position.y,
            gameSettings.platformDestructionOffset);

        var currentDefaultRoadsCount = 0;
        while (currentPos.z <= gameSettings.platformSpawnOffset)
        {
            var currentType = RoadSpawnType.Default;
            if (currentDefaultRoadsCount > gameSettings.initialDefaultRoadsCount)
            {
                currentType = currentDefaultRoadsCount == gameSettings.initialDefaultRoadsCount + 1
                    ? RoadSpawnType.CheckPoint
                    : RoadSpawnType.RandomFromList;
            }
            
            SpawnRoadPart(ref currentPos, currentType);
            currentDefaultRoadsCount++;
        }
    }

    private void CheckForSpawnNeed()
    {
        var currentZ = roadParts[roadParts.Count - 1].transform.position.z;
        if(currentZ >= gameSettings.platformSpawnOffset)
            return;

        var currentPos = new Vector3(0f, transform.position.y, currentZ);
        
        SpawnRoadPart(ref currentPos, RoadSpawnType.RandomFromList);
    }

    private void SpawnRoadPart(ref Vector3 currentPos, RoadSpawnType prefferedSpawnType)
    {
        RoadPart newRoadPrefab;
        
        roadPartsSinceCheckPoint++;

        if (prefferedSpawnType == RoadSpawnType.CheckPoint)
        {
            roadPartsSinceCheckPoint = 0;
        }
        else if (roadPartsSinceCheckPoint >= gameSettings.roadPartsToNewCheckPoint)
        {
            roadPartsSinceCheckPoint = 0;
            prefferedSpawnType = RoadSpawnType.CheckPoint;
        }

        if (spawnOnlyDefault)
            prefferedSpawnType = RoadSpawnType.Default;
        
        switch (prefferedSpawnType)
        {
            case RoadSpawnType.Default:
                newRoadPrefab = gameSettings.defaultRoadPartPrefab;
                break;
            case RoadSpawnType.CheckPoint:
                newRoadPrefab = gameSettings.checkPointRoadPartPrefab;
                break;
            case RoadSpawnType.RandomFromList:
                var isJewels = Random.value < gameSettings.probabilityToSpawnJewelsPlatform;
                var targetList = isJewels ? gameSettings.jewelsRoadParts : gameSettings.foodRoadParts;
                var randomIndex = Random.Range(0, targetList.Count);
                newRoadPrefab = targetList[randomIndex];
                break;
            default:
                return;
        }

        if(roadParts.Count > 0)
            currentPos.z += 0.5f * roadParts[roadParts.Count - 1].length;
        
        currentPos.z += 0.5f * newRoadPrefab.length;
        var newRoadPart = Instantiate(newRoadPrefab, transform);
        newRoadPart.transform.position = currentPos;
        roadParts.Add(newRoadPart);
    }

    private void DestroyRoadPart(RoadPart roadPart)
    {
        roadParts.Remove(roadPart);
        Destroy(roadPart.gameObject);
    }

}
