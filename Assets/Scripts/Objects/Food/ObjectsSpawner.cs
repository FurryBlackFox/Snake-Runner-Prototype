using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnerType
{
    Food,
    Jewel
}

public class ObjectsSpawner : MonoBehaviour
{
    public SpawnerType spawnerType;
    public bool spawnOnlyCurrentType = false;

    private List<Transform> spawnedObjectsList;
    private GameSettings gameSettings;

    private FoodType spawningFoodType;

    private void Awake()
    {
        spawnedObjectsList = new List<Transform>();
    }

    private void Start()
    {
        gameSettings = SettingsManager.S.gameSettings;

        if (spawnerType == SpawnerType.Food)
        {
            spawningFoodType = spawnOnlyCurrentType ? GameManager.currentFoodTypeSpawning : DefineRandomType();
            SpawnFood();
        }
        else
        {
            SpawnJewel();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = spawnOnlyCurrentType ? Color.green : Color.blue;
        Gizmos.DrawSphere(transform.position, 0.1f);
        if(!gameSettings)
            return;
        Gizmos.DrawWireSphere(transform.position, gameSettings.foodSpawnerRadius);
    }


    private FoodType DefineRandomType()
    {
        
        var currentFoodProbability = Random.value;
        if(currentFoodProbability <= gameSettings.probabilityToSpawnCurrentFoodType)
            return GameManager.currentFoodTypeSpawning;
        

        var randomIndex = Random.Range(0, gameSettings.foodTypes.Count);
        var candidateType = gameSettings.foodTypes[randomIndex];
        if (GameManager.currentFoodTypeSpawning == candidateType)
        {
            var newIndex = (randomIndex + 1) % gameSettings.foodTypes.Count;
            candidateType = gameSettings.foodTypes[newIndex];
        }

        return candidateType;
        
    }
    
    private void SpawnFood() 
    {
        var currentWrongSpawnAttempts = 0;
        var yPos = transform.localPosition.y;
        var iterationsCount = gameSettings.maxFoodSpawnCount;
        for (int i = 0; i < iterationsCount; i++)
        {
            if(currentWrongSpawnAttempts >= gameSettings.maxWrongSpawnAtteptsCount)
                break;
            
            
            var randomPos = Random.insideUnitCircle * gameSettings.foodSpawnerRadius;
            var newFoodPos = new Vector3(randomPos.x, yPos, randomPos.y);
            var isGood = true;
            foreach (var food in spawnedObjectsList)
            {
                var sqrDistance = (food.localPosition - newFoodPos).sqrMagnitude;
                if (sqrDistance < gameSettings.minSqrDistanceBetweenFood)
                {
                    isGood = false;
                    currentWrongSpawnAttempts++;
                    iterationsCount++;
                    break;
                }
            }
            
            if(!isGood)
                continue;
            
            var instanciatedFood = ObjectPoolsManager.FoodObjectPool.Get();
            instanciatedFood.transform.parent = transform;
            instanciatedFood.transform.localPosition = newFoodPos;
            instanciatedFood.SetType(spawningFoodType);

            spawnedObjectsList.Add(instanciatedFood.transform);
        }
        
        spawnedObjectsList.Clear();
    }

    private void SpawnJewel()
    {
        if (Random.value > gameSettings.probabilityToSpawnJewel)
            return;
        
        var spawnedJewel = ObjectPoolsManager.JewelObjectPool.Get();
        spawnedJewel.transform.parent = transform;
        spawnedJewel.transform.position = transform.position;
    }
    

    private void OnDestroy()
    {
        ReturnObjects();
    }

    private void ReturnObjects()
    {
        var objects = GetComponentsInChildren<IPoolableObject>();
        foreach (var obj in objects)
        {
            obj.ReturnToPool();
        }
    }
}
