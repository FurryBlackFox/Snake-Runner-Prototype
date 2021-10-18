using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private List<Renderer> meshesToUpdate;
    [SerializeField] private List<ParticleSystemRenderer> particleRenderers;
        
        
    public FoodType foodType;
    private Collider cashedCollider;
    private void Awake()
    {
        cashedCollider = GetComponent<Collider>();
        if (cashedCollider.isTrigger == false)
        {
            Debug.Log($"plz check trigger in {name} ");
            cashedCollider.isTrigger = true;
        }
    }

    private void Start()
    {
        var randomIndex = Random.Range(0, SettingsManager.S.gameSettings.foodTypes.Count);
        foodType = SettingsManager.S.gameSettings.foodTypes[randomIndex];

        foreach (var meshRenderer in meshesToUpdate)
        {
            meshRenderer.sharedMaterial = foodType.material;
        }
        
        foreach (var particleRenderer in particleRenderers)
        {
            particleRenderer.material.color = foodType.material.color;
        }

        GameManager.currentFoodTypeSpawning = foodType;
    }
}
