using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject modelPrefab;
    
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    
    private float _moveT;
    
    private void Update()
    {
        
        _moveT += Time.deltaTime;
        modelPrefab.transform.position = Vector3.Lerp(transform.position + startPos, transform.position + endPos, Mathf.PingPong(_moveT, 1));
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + startPos, .05f);
        Gizmos.DrawSphere(transform.position + endPos, .05f);
        Gizmos.DrawLine(transform.position + startPos, transform.position + endPos);
    }
}
