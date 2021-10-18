using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// markers generate quite a lot of garbage per frame.
/// need more time for optimization.
/// current variants: returning back to struct or finish object pooling
/// </summary>
public class Marker : IPoolableObject 
{
    public Vector3 position;
    public Quaternion rotation;

    private ObjectPool<Marker> assignedPool;

    // ~Marker() //Still dont know how to stable return object to the pool
    // {
    //     assignedPool?.Return(this);
    //     Debug.Log(assignedPool);
    // }
    
    public void Subscribe()
    {
        GameManager.OnUpdatePosition += UpdatePosition;
    }

    public void Unsubscribe()
    {
        GameManager.OnUpdatePosition -= UpdatePosition;
    }
    
    private void UpdatePosition(float deltaSpeed)
    {
        position.z += deltaSpeed;
    }


    public void AssignToPool<T>(ObjectPool<T> objectPool) where T : IPoolableObject, new()
    {
        assignedPool = objectPool as ObjectPool<Marker>;
    }

    public void ReturnToPool()
    {
        assignedPool.Return(this);
    }

    public void Hide()
    {
    }

    public void Reveal()
    {
    }
}
