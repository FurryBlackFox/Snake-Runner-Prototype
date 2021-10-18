using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Food : MonoBehaviour, IPoolableObject, IConsumable
{
    public FoodType foodType;
    private MeshRenderer meshRenderer;
    private Collider cashedCollider;
    private ObjectPoolMonoBeh<Food> assignedPool;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        cashedCollider = GetComponent<Collider>();
    }
    
    public void SetType(FoodType newType)
    {
        foodType = newType;
        meshRenderer.sharedMaterial = newType.material;
    }

    public void AssignToPool<T>(ObjectPool<T> objectPool) where T : IPoolableObject, new()
    {
        assignedPool = objectPool as ObjectPoolMonoBeh<Food>;
    }

    public void ReturnToPool()
    {
        assignedPool.Return(this);
    }

    public void Hide()
    {
        cashedCollider.enabled = false;
        gameObject.SetActive(false);
    }

    public void Reveal()
    {
        cashedCollider.enabled = true;
        gameObject.SetActive(true);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnConsumeStarted()
    {
        cashedCollider.enabled = false;
    }

    public void Destroy()
    {
        Hide();
    }
}
