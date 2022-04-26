using System;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionPlayer : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerCollision onPlayerEnter;
    [SerializeField] private PlayerCollision onPlayerExit;
    [SerializeField, Range(0f, 100f)] private float speed;

    public bool Active
    {
        get => active;
        set => active = value;
    }
    
    private void OnEnter(Collider other)
    {
        if (onPlayerEnter.debug)
        {
            Debug.Log($"{gameObject.name} => {other.gameObject.name}\n");
        }
        onPlayerEnter.Raise(other);
        
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        Rigidbody playerRb = other.GetComponent<Rigidbody>();
        if (playerManager == null || playerRb == null ) { return; }
        
        switch (playerManager.state)
        {
            case PlayerManager.State.Rolling when playerRb.velocity.magnitude > speed:
                onPlayerEnter.RaiseOnSpeed(other);
                break;
            case PlayerManager.State.Rolling:
                onPlayerEnter.RaiseOnBall(other);
                break;
            case PlayerManager.State.Normal:
                onPlayerEnter.RaiseOnWalk(other);
                break;
        }
    }
    
    private void OnExit(Collider other)
    {
        if (onPlayerExit.debug)
        {
            Debug.Log($"{gameObject.name} => {other.gameObject.name}\n");
        }
        onPlayerExit.Raise(other);
        
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        Rigidbody playerRb = other.GetComponent<Rigidbody>();
        if (playerManager == null || playerRb == null ) { return; }
        
        switch (playerManager.state)
        {
            case PlayerManager.State.Rolling when playerRb.velocity.magnitude > speed:
                onPlayerExit.RaiseOnSpeed(other);
                break;
            case PlayerManager.State.Rolling:
                onPlayerExit.RaiseOnBall(other);
                break;
            case PlayerManager.State.Normal:
                onPlayerExit.RaiseOnWalk(other);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active) { return; }
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;
        OnEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!active) { return; }
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;
        OnExit(other);
    }
}


[Serializable]
public class PlayerCollision : LayerCollision
{
    public UnityEvent<Collider> onBallCollision;
    public UnityEvent<Collider> onWalkCollision;
    public UnityEvent<Collider> onSpeedCollision;

    public void RaiseOnBall(Collider collider)
    {
        onBallCollision.Invoke(collider);
    }
    
    public void RaiseOnWalk(Collider collider)
    { 
        onWalkCollision.Invoke(collider);
    }

    public void RaiseOnSpeed(Collider collider)
    {
        onSpeedCollision.Invoke(collider);
    }
}