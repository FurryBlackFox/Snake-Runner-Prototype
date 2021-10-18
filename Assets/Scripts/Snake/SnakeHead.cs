using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeHead : SnakeSegment
{
    private Vector3 raycastResult;
    private bool hasTarget;

    private Vector3 hitPointGizmos;

    private float currentHorizontalSpeed = 0f;
    private float cashedDirection = 0f;

    private List<MeshRenderer> childMeshes;

    protected override void Awake()
    {
        base.Awake();
        childMeshes = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SnakeInput.OnInput += OnInputHandler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SnakeInput.OnInput -= OnInputHandler;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = hasTarget ? Color.green : Color.red;
        Gizmos.DrawSphere(hitPointGizmos, 0.25f);
    }

    public override void BlowUp()
    {
        base.BlowUp();
        foreach (var childMesh in childMeshes)
        {
            childMesh.enabled = false;
        }
    }
    
    private void OnInputHandler(float hitXPos)
    {
        raycastResult = new Vector3(hitXPos, transform.position.y, 0);

        hitPointGizmos = raycastResult;
        hasTarget = true;
    }
    

    public override void UpdatePosition()
    {
        var horizontalMovement = 0f;
        if (!hasTarget)
        {
            currentHorizontalSpeed = 0f;
        }
        else
        {
            var difference = raycastResult.x - transform.position.x;
        
            var movementDirection = Mathf.Sign(difference);

            if (Mathf.Abs(cashedDirection - movementDirection) > 0.01f)
                currentHorizontalSpeed = 0f;
        
            cashedDirection = movementDirection;
        
        
            var absDifference = Mathf.Abs(difference);
            if (absDifference <= snakeSettings.minDistanceToRaycastTarget)
            {
                hasTarget = false;
                horizontalMovement = 0f;
            }
            else
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, snakeSettings.snakeHorizontalSpeed,
                    snakeSettings.snakeSpeedGain * Time.fixedDeltaTime);

                var deltaDistance = currentHorizontalSpeed * Time.fixedDeltaTime;
                if (deltaDistance > absDifference)
                {
                    deltaDistance = absDifference;
                    hasTarget = false;
                }
                horizontalMovement = movementDirection * deltaDistance;
            }
        }
        
        var newPos = transform.position;
        newPos.x += horizontalMovement;
        transform.position = newPos;
    }
    
    public override void UpdateRotation()
    {
        if(!hasTarget && transform.rotation == Quaternion.identity)
            return;
        
        var newForwardVector = raycastResult - transform.position;
        var targetRotation = hasTarget
            ? Quaternion.LookRotation(newForwardVector, Vector3.up)
            : Quaternion.identity;
        
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, snakeSettings.maxDegreePerSec * Time.deltaTime);
    }
}
