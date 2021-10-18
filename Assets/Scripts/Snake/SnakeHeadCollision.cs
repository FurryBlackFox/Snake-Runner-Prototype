using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeHead), typeof(Rigidbody), typeof(Collider))]
public class SnakeHeadCollision: MonoBehaviour
{
    private Snake snake;
    private SnakeSettings snakeSettings;

    private Collider[] colliders;

    private bool isBoostActive = false;

    private float currentConsumptionRange;
    private float currentConsumptionMaxAngle;
    private int currentLayerMask;
    
    private void Awake()
    {
        snake = GameManager.snake;
        snakeSettings = SettingsManager.S.snakeSettings;
        colliders = new Collider[snakeSettings.collisionCollidersMaxCount];
        
        currentConsumptionRange = snakeSettings.foodConsumptionRange;
        currentConsumptionMaxAngle = snakeSettings.foodConsumptionMaxAngle;
        currentLayerMask = snakeSettings.foodConsumptionLayerMask;
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
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            AssignNewFoodType(other.GetComponent<CheckPoint>().foodType);
        }
        else if (other.CompareTag("Obstacle"))
        {
            OnObstacleCollision(other.GetComponent<Obstacle>());
        }
    }

    private void LateUpdate()
    {
        CheckForConsumables();
    }

    private void OnDrawGizmos()
    {
        if(!snakeSettings)
            return;
        Gizmos.color = Color.red;
        var pos = transform.position;
        var forwardOnSphere = transform.forward * currentConsumptionRange;
        Gizmos.DrawWireSphere(pos, currentConsumptionRange);
        
        Gizmos.DrawLine(pos, pos + Quaternion.Euler(0, currentConsumptionMaxAngle, 0) * forwardOnSphere);
        Gizmos.DrawLine(pos, pos + Quaternion.Euler(0, -currentConsumptionMaxAngle, 0) * forwardOnSphere);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos, snakeSettings.confidentFoodConsumptionRange);
    }

    
    private void OnBoostStartedHandler()
    {
        isBoostActive = true;

        StartCoroutine(SmoothlyUpdateConsumptionRange(snakeSettings.boostConsumptionRange));
        currentConsumptionMaxAngle = snakeSettings.boostConsumptionMaxAngle;
        currentLayerMask = snakeSettings.boostConsumptionLayerMask;
    }

    private void OnBoostEndedHandler()
    {
        isBoostActive = false;

        currentConsumptionRange = snakeSettings.foodConsumptionRange;
        currentConsumptionMaxAngle = snakeSettings.foodConsumptionMaxAngle;
        currentLayerMask = snakeSettings.foodConsumptionLayerMask;
    }
    
    
    private IEnumerator SmoothlyUpdateConsumptionRange(float targetRange)
    {
        while (!Mathf.Approximately(currentConsumptionRange, targetRange))
        {
            currentConsumptionRange = Mathf.MoveTowards(currentConsumptionRange, targetRange, 
                snakeSettings.consumptionRangeGain * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
    
    
    private void AssignNewFoodType(FoodType foodType)
    {
        snake.UpdateFoodType(foodType);
    }

    private void OnObstacleCollision(Obstacle obstacle)
    {
        if(isBoostActive)
            return;
        snake.OnObstacleCollision(obstacle);
        obstacle.OnCollide();
        
    }
    
    private void CheckForConsumables()
    {
        int collidersCount;



        collidersCount =
            Physics.OverlapSphereNonAlloc(transform.position, currentConsumptionRange, colliders, currentLayerMask);
        
        if(collidersCount == 0)
            return;

        for (var i = 0; i < collidersCount; i++)
        {
            var targetCollider = colliders[i];
            var targetPos = targetCollider.transform.position;
            var pos = transform.position;
            if ((pos - targetCollider.transform.position).sqrMagnitude > snakeSettings.confidentFoodConsumptionRange *
                snakeSettings.confidentFoodConsumptionRange)
            {
                var vectorToTarget = targetPos - transform.position;
                var angle= Vector3.Angle(transform.forward, vectorToTarget);

                if(angle > currentConsumptionMaxAngle)
                    continue;
            }

            if(targetCollider.CompareTag("Food"))
                TryToConsumeFood(targetCollider.GetComponent<Food>());
            else if(targetCollider.CompareTag("Jewel"))
                ConsumeJewel(targetCollider.GetComponent<Jewel>());
            else if(targetCollider.CompareTag("Obstacle"))
                ConsumeObstacle(targetCollider.GetComponent<Obstacle>());
        }
    }

    private void TryToConsumeFood(Food food)
    {
        // if(snake.currentFoodType != food.foodType)
        //     return;
        
        snake.ConsumeFood(food);
    }

    private void ConsumeJewel(Jewel jewel)
    {
        snake.ConsumeJewel(jewel);
    }

    private void ConsumeObstacle(Obstacle obstacle)
    {
        snake.ConsumeObstacle(obstacle);
    }
}
