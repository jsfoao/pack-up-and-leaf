using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _components;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer[] _renderer;

    public void DisablePlayerInput()
    {
        GetComponent<InputManager>().enabled = false;
    }
    
    public void EnablePlayerInput()
    {
        GetComponent<InputManager>().enabled = true;
    }
    
    public void DisablePlayer()
    {
        FreezeRigidbody();
        DisableCollision();
        DisableRendering();
        DisableComponentArray();
    }

    public void EnablePlayer()
    {
        ResumeRigidbody();
        EnableCollision();
        EnableRendering();
        EnableComponentArray();
    }

    public void DisableComponentArray()
    {
        foreach (var component in _components)
        {
            component.enabled = false;
        }
    }
    
    public void EnableComponentArray()
    {
        foreach (var component in _components)
        {
            component.enabled = true;
        }
    }

    public void FreezeRigidbody()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ResumeRigidbody()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.freezeRotation = true;
    }

    public void DisableCollision()
    {
        _collider.enabled = false;
    }
    
    public void EnableCollision()
    {
        _collider.enabled = true;
    }

    public void DisableRendering()
    {
        foreach (var renderer in _renderer)
        {
            renderer.enabled = false;
        }
    }
    
    public void EnableRendering()
    {
        foreach (var renderer in _renderer)
        {
            renderer.enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<PlayerEntity>().Respawn();
        }
    }
}
