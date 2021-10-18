using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] private List<Transform> waterTransforms;
    
    private GameSettings gameSettings;
    private Transform waterPrefab;
    private void Awake()
    {
        gameSettings = SettingsManager.S.gameSettings;
    }

    private void Update()
    {
        var transition = Vector3.back * (GameManager.CurrentGameSpeed * Time.deltaTime);
        for (var i = 0; i < waterTransforms.Count; i++)
        {
            var water = waterTransforms[i];
            water.Translate(transition);
            var waterPos = water.transform.position;
            if (water.transform.position.z < gameSettings.minWaterPosZ)
            {
                waterPos.z += waterTransforms.Count * water.localScale.z * gameSettings.waterPlaneLength;
                water.transform.position = waterPos;
            }
        }
    }
}
