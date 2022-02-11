using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _bulletRb;
    private OnCollisionPlayer _playerCollision;
    private OnCollisionLayer _layerCollision;

    public void Deactivate()
    {
        _bulletRb.constraints = RigidbodyConstraints.FreezeAll;
        
        // Disable collision behaviour
        if (_playerCollision != null) { _playerCollision.Active = false; }
        if (_layerCollision != null) { _layerCollision.Active = false; }
    }
    
    private void Awake()
    {
        _bulletRb = GetComponent<Rigidbody>();
        _playerCollision = GetComponent<OnCollisionPlayer>();
        _layerCollision = GetComponent<OnCollisionLayer>();
    }
}
