using System;
using UnityEngine;

public class GroundObject : MonoBehaviour
{
    [SerializeField] private bool groundObject;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(0f, 20f)] private float offset;

    private void OnDrawGizmos()
    {
        if (groundObject)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
                transform.position = new Vector3(transform.position.x, hit.point.y + offset, transform.position.z);
            }
        }
    }
}
