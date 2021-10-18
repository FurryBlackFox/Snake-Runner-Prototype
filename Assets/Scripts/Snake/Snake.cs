using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public static event Action<FoodType> OnUpdateFoodType;
    public static event Action<Food> OnFoodConsumed;
    public static event Action<Food> OnRightFoodConsumed;
    public static event Action<Jewel> OnJewelConsumed;

    public static FoodType CurrentFoodType { get; private set; }
    public static bool inputEnabled = true;

    private SnakeSettings snakeSettings;
    private bool isMoving = true;
    public  bool isBoostActive = false;
    
    [SerializeField]
    private List<SnakeSegment> snakeSegments;

    private SnakeHead snakeHead;

    #region DefaultEvents

    private void Awake()
    {
        snakeSettings = SettingsManager.S.snakeSettings;
        CurrentFoodType = null;
        
        GameManager.snake = this;
    }

    private void OnEnable()
    {
        SnakeBoostController.OnBoostStarted += OnBoostStartedHandler;
        SnakeBoostController.OnBoostEnded += OnBoostEndedHandler;
    }

    private void OnDisable()
    {
        SnakeBoostController.OnBoostStarted -= OnBoostStartedHandler;
        SnakeBoostController.OnBoostEnded -= OnBoostEndedHandler;
    }

    private void Start()
    {
        snakeHead = (SnakeHead) snakeSegments[0];
    }

    private void FixedUpdate()
    {
        if(!isMoving)
            return;
        foreach (var snakeSegment in snakeSegments)
        {
            snakeSegment.UpdateTransform();
            snakeSegment.MeshUpdates();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            SpawnNewSegment();
    }


    #endregion
    
    #region Consumption

    public void UpdateFoodType(FoodType newFoodType)
    {
        CurrentFoodType = newFoodType;
        OnUpdateFoodType?.Invoke(newFoodType);
    }

    public void ConsumeFood(Food food)
    {
        if (food.foodType != CurrentFoodType && !isBoostActive)
            StartCoroutine(OnWrongFoodConsumed());
        else
        {
            OnRightFoodConsumed?.Invoke(food);
            SpawnNewSegment();
        }
        
        OnFoodConsumed?.Invoke(food);
        
        StartCoroutine(UpdateConsumable(food));
    }

    private IEnumerator OnWrongFoodConsumed()
    {
        yield return new WaitForSeconds(snakeSettings.timeToConsume * 0.5f);
        OnPlayerDeath();
    }

    public void ConsumeJewel(Jewel jewel)
    {
        OnJewelConsumed?.Invoke(jewel);
        StartCoroutine(UpdateConsumable(jewel));
    }

    public void ConsumeObstacle(Obstacle obstacle)
    {
        StartCoroutine(UpdateConsumable(obstacle));
    }

    private IEnumerator UpdateConsumable(IConsumable consumable)
    {
        consumable.OnConsumeStarted();
        var targetTransform = consumable.GetTransform();
        var timer = 0f;
        var waitForFixed = new WaitForFixedUpdate();
        var currentScale = targetTransform.localScale;
        var currentPos = targetTransform.position;
        var minScale = Vector3.one * snakeSettings.minConsumableScale;
        while (timer <= snakeSettings.timeToConsume)
        {
            var t = timer / snakeSettings.timeToConsume;
            currentScale = Vector3.Lerp(currentScale, minScale, t);
            targetTransform.localScale = currentScale;
            currentPos = Vector3.Lerp(currentPos, snakeHead.transform.position, t);
            targetTransform.position = currentPos;

            yield return waitForFixed;
            timer += Time.fixedDeltaTime;
            
            if(!isMoving)
                break;
        }
        
        consumable.Destroy();
    }

    private void SpawnNewSegment()
    {
        if(snakeSegments.Count > snakeSettings.snakeMaxSegments)
            return;
        
        var parent = snakeSegments[snakeSegments.Count - 1];
        var parentTransform = parent.transform;
        var spawnPosition = parentTransform.position - parentTransform.forward * snakeSettings.spawnOffset;

        var newSegment = Instantiate(snakeSettings.snakeSegmentPrefab, spawnPosition, parentTransform.rotation, transform);
        snakeSegments.Add(newSegment);
        newSegment.parent = parent;
    }

    #endregion

    #region Boost

    private void OnBoostStartedHandler()
    {
        inputEnabled = false;
        isBoostActive = true;
    }

    private void OnBoostEndedHandler()
    {
        inputEnabled = true;
        isBoostActive = false;
    }

    #endregion
    
    #region Collision

    public void OnObstacleCollision(Obstacle obstacle)
    {
        OnPlayerDeath();
    }

    #endregion

    #region Death
    
    public void OnPlayerDeath()
    {
        GameManager.S.OnPlayerDeathHandler();
        StartCoroutine(BlowUpSnake());
        isMoving = false;
    }

  

    private IEnumerator BlowUpSnake()
    {
        var waitForSeconds = new WaitForSecondsRealtime(snakeSettings.snakeBlowUpDelta);
        for (var i = 0; i < snakeSegments.Count; i++) 
        {
            snakeSegments[i].BlowUp();
            yield return waitForSeconds;
        }
    }

    #endregion
    
  
    
  
  
}
