using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Killzone : MonoBehaviour
{
    [SerializeField] private Vector3 size;
    private PlayerEntity targetEntity;

    private void OnTriggerEnter(Collider other)
    {
        if (targetEntity == null) { return; }
        
        if (other.gameObject == targetEntity.gameObject)
        {
            targetEntity.DeathBehaviour();
        }
    }

    private void Start()
    {
        targetEntity = FindObjectOfType<PlayerEntity>();
    }

    private void OnValidate()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
