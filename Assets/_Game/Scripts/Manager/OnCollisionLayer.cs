using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionLayer : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private List<LayerCollision> layerCollisions;
    
    public bool Active
    {
        get => active;
        set => active = value;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!active) { return; }
        EvaluateCollision(other);
    }

    private void EvaluateCollision(Collider other)
    {
        foreach (var collision in layerCollisions)
        {
            if (((1 << other.gameObject.layer) & collision.LayerMask) == 0) continue;
            if (collision.debug)
            {
                Debug.Log($"{gameObject.name} => {other.gameObject.name}");
            }
            collision.Raise(other);
        }
    }
}

[Serializable]
public class LayerCollision
{
    public bool debug;
    public LayerMask LayerMask;
    public UnityEvent<Collider> OnCollision;

    public void Raise(Collider collider)
    {
        OnCollision.Invoke(collider);
    }
}
