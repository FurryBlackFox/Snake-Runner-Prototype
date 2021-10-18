using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class Obstacle : MonoBehaviour, IConsumable
{
    [SerializeField] private ParticleSystem blowUpParticles;


    private MeshRenderer meshRenderer;
    private Collider cashedCollider;
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        cashedCollider = GetComponent<Collider>();
    }

    public void OnCollide()
    {
        cashedCollider.enabled = false;
        meshRenderer.enabled = false;
        
        if(!blowUpParticles.isPlaying)
            blowUpParticles.Play();
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
        Destroy(gameObject);
    }
}
