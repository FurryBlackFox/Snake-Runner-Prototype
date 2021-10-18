using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Snake Settings", order = 2, menuName = "Scriptable Objects/Snake Settings")]
public class SnakeSettings : ScriptableObject
{
    [Header("VFX")]
    [Min(0f)] public float snakeBlowUpDelta = 0.25f;
    
    [Header("Segments Spawn")]
    public SnakeBodyPart snakeSegmentPrefab;
    public float spawnOffset = 2f;
    [Range(5, 30)] public int snakeMaxSegments = 10;
    
    [Header("Input")]
    public LayerMask targetRaycastLayers;
    [Min(0f)] public float maxRaycastDistance = 50f;
    [Min(0f)] public float minDistanceToRaycastTarget = 0.001f;
    
    [Header("Movement")]
    [Min(0f)]public float maxDegreePerSec = 360f;
    [Min(0f)] public float snakeHorizontalSpeed = 15f;
    [Min(0f)] public float snakeSpeedGain = 5f;
    [Min(10)] public int maxMarkersPerSegment = 10;


    [Header("Additional Sine Wave")]
    public bool enableSine = true;
    [Min(0f)] public float sinWavePeriod = 10f;
    [Min(0f)] public float sinWaveAmplitude = 1f;
    [Min(0f)] public float sinWaveSpeed = 1f;
    [Min(0f)] public float distanceToMaxWaveAmplitude = 2f;
    [Min(0f)] public float meshDeltaSpeed = 1f;

    [Header("Default Object Detection")]
    [Min(10)] public int collisionCollidersMaxCount = 30;
    [Min(0f)] public float foodConsumptionRange = 2f;
    [Min(0f)] public float foodConsumptionMaxAngle = 30f;
    [Min(0f)] public float confidentFoodConsumptionRange = 1;
    public LayerMask foodConsumptionLayerMask;
    [Min(0f)] public float timeToConsume = 0.5f;
    [Min(0f)] public float minConsumableScale = 0.25f;
    
    [Header("Boost Mode Object Detection")]
    [Min(0f)] public float boostConsumptionRange = 30f;
    [Min(0f)] public float boostConsumptionMaxAngle = 45f;
    public LayerMask boostConsumptionLayerMask;
    [Min(0f)] public float consumptionRangeGain = 10f;

    [Header("Boost Settings")]
    [Range(0f, 1f)] public float boostValuePerJewel = 0.35f;
    [Min(0f)] public float boostValueGain = 0.5f;
    [Min(0f)] public float boostValueDecline = 0.1f;
    [Min(0f)] public float boostDuration = 5f;
}
