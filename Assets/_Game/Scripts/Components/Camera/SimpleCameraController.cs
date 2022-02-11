using System;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float angle;
    
    [SerializeField] private Transform target;

    void Update()
    {
        // handle rotation
        Vector3 targetDirection = target.position - transform.position;
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        // handle position
        float xPos = distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 newPosition = target.position + new Vector3(xPos, height, zPos);
        transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }
}
