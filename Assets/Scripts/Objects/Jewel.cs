using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Jewel : MonoBehaviour, IPoolableObject, IConsumable
{
    private GameSettings gameSettings;
    private Collider cashedCollider;
    private ObjectPoolMonoBeh<Jewel> assignedPool;
    private void Start()
    {
        gameSettings = SettingsManager.S.gameSettings;
        cashedCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        var rotation = gameSettings.jewelRotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
    }

    public void AssignToPool<T>(ObjectPool<T> objectPool) where T : IPoolableObject, new()
    {
        assignedPool = objectPool as ObjectPoolMonoBeh<Jewel>;
    }

    public void ReturnToPool()
    {
        assignedPool.Return(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Reveal()
    {
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        ReturnToPool();
    }

    
    public void OnConsumeStarted()
    {
        cashedCollider.enabled = false;
    }
    
    public Transform GetTransform()
    {
        return transform;
    }
}
