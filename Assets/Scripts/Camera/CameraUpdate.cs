using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraUpdate : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 cashedPosition;
    private Quaternion cashedRotation;
    private void Awake()
    {
        cashedPosition = transform.position;
        cashedRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        var newPos = cashedPosition;
        newPos.z = target.position.z + cashedPosition.z;
        transform.position = newPos;
        transform.rotation = cashedRotation;
    }
}
