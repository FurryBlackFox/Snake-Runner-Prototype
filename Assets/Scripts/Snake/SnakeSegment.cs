using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnakeSegment : MonoBehaviour
{
    [SerializeField] private ParticleSystem blowUpParticles;
    [SerializeField] protected MeshRenderer meshRenderer;
    public MarkerManager markerManager;

    protected bool isMoving;

    protected SnakeSettings snakeSettings;
    private Vector3 cashedPos;
    
    
    protected virtual void Awake()
    {
        markerManager = new MarkerManager();
        snakeSettings = SettingsManager.S.snakeSettings;
        OnUpdateFoodTypeHandler(Snake.CurrentFoodType);
    }

    protected virtual void OnEnable()
    {
        Snake.OnUpdateFoodType += OnUpdateFoodTypeHandler;
    }

    protected virtual void OnDisable()
    {
        Snake.OnUpdateFoodType -= OnUpdateFoodTypeHandler;
    }

    protected void OnUpdateFoodTypeHandler(FoodType foodType)
    {
        if(!foodType)
            return;
        meshRenderer.sharedMaterial = foodType.material;
    }

    public void UpdateTransform()
    {
        if(cashedPos != Vector3.zero)
            transform.position = cashedPos;

        
        UpdatePosition();
        UpdateRotation();
        
        AddMarker();
        
        isMoving = (cashedPos - transform.position).sqrMagnitude > Mathf.Epsilon;
        cashedPos = transform.position;
    }

    public abstract void UpdatePosition();
    
    public abstract void UpdateRotation();


    public virtual void BlowUp()
    {
        if(!blowUpParticles.isPlaying)
            blowUpParticles.Play();
        meshRenderer.enabled = false;
    }

    public void MeshUpdates()
    {
        if (isMoving)
        {
            meshRenderer.transform.localPosition = Vector3.zero;
            return;
        }
        
        if(snakeSettings.enableSine)
            CalculateSinOffset();
        
    }

    public void CalculateSinOffset()
    {
        var pos = transform.localPosition;
        var absZ = Mathf.Abs(pos.z);
        var wavePeriod = 2 * Mathf.PI / snakeSettings.sinWavePeriod;
        var amplitudeMult = Mathf.Clamp01(absZ / snakeSettings.distanceToMaxWaveAmplitude);
        var sinOffset = amplitudeMult * snakeSettings.sinWaveAmplitude *
                        Mathf.Sin(wavePeriod * (absZ - Time.time * snakeSettings.sinWaveSpeed));
        
        meshRenderer.transform.localPosition = Vector3.MoveTowards(meshRenderer.transform.localPosition, 
            new Vector3(sinOffset, 0, 0), snakeSettings.meshDeltaSpeed * Time.fixedDeltaTime);
    }


    public void AddMarker()
    {
        if (markerManager.Count >= snakeSettings.maxMarkersPerSegment)
        {
            markerManager.Dequeue();
        }
        var marker = new Marker(transform.position, transform.rotation);
        markerManager.Enqueue(marker);
    }
    


}
