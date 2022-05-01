using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(OrbitCamera))]
public class CameraCollision : MonoBehaviour
{
    public LayerMask collisionLayer;

    private bool colliding;
    [SerializeField] private float collisionSpaceSize = 3.41f;
    [SerializeField] private float offset;
    [SerializeField] private float smoothTime;
    [SerializeField] private float smoothReset;

    private float _camVelocity;
    private float _minDistance;
    private Camera camera;
    private OrbitCamera _orbitCamera;
    private Transform _transform;
    
    private void Update()
    {
        // Update clip points
        if (!camera) { return; }
        
        Vector3[] clipPoints = new Vector3[5];
        
        // Calculate clip points
        float z = camera.nearClipPlane;
        float x = Mathf.Tan(60f / collisionSpaceSize) * z;
        float y = x / camera.aspect;

        Vector3 camPosition = _transform.position;
        Vector3 camRight = _transform.right;
        Vector3 camUp = _transform.up;
        Vector3 camForward = _transform.forward;

        Vector3 direction = (transform.position - _orbitCamera.Target.position).normalized;
        
        #region Adjusted Clip points
        // top left
        clipPoints[0] = ((camRight * -x + camUp * y) * offset + camForward * z) + direction * _orbitCamera.Radius + _orbitCamera.Target.transform.position;
        
        // top right
        clipPoints[1] = ((camRight * x + camUp * y) * offset + camForward * z) + direction * _orbitCamera.Radius + _orbitCamera.Target.transform.position;
        
        // bottom left
        clipPoints[2] = ((camRight * -x + camUp * -y) * offset + camForward * z) + direction * _orbitCamera.Radius + _orbitCamera.Target.transform.position;
        
        // bottom right
        clipPoints[3] = ((camRight * x + camUp * -y) * offset + camForward * z) + direction * _orbitCamera.Radius + _orbitCamera.Target.transform.position;
        
        // camera's position
        clipPoints[4] = direction * _orbitCamera.Radius + _orbitCamera.Target.transform.position;
        #endregion
        
        // Old code
        // Collision with clip points
        // if (CollisionWithClipPoints(clipPoints, _orbitCamera.Target.position))
        // {
        //     // Shortest collision distance
        //     float newDist = GetShortestDistance(clipPoints, _orbitCamera.Target.position);
        //     newDist = Mathf.Clamp(newDist, 0, _orbitCamera.Radius);
        //     
        //     // Difference between new distance and distance on previous frame
        //     float diff = Mathf.Abs(newDist - _previousFrame);
        //
        //     float currSmoothTime;
        //     if (diff > threshold)
        //     {
        //         _minDistance = newDist;
        //     }
        //     else
        //     {
        //         currSmoothTime = smoothTime;
        //     }
        //
        //     _orbitCamera.CurrentRadius = Mathf.SmoothDamp(_orbitCamera.CurrentRadius, _minDistance, ref _camVelocity, smoothTime);
        //     _previousFrame = _minDistance;
        //     colliding = true;
        // }
        
        if (CollisionWithClipPoints(clipPoints, _orbitCamera.Target.position))
        {
            // Shortest collision distance
            _minDistance = GetShortestDistance(clipPoints, _orbitCamera.Target.position);
            _minDistance = Mathf.Clamp(_minDistance, 0, _orbitCamera.Radius);
            _orbitCamera.CurrentRadius = Mathf.SmoothDamp(_orbitCamera.CurrentRadius, _minDistance, ref _camVelocity, smoothTime);

            colliding = true;
        }
        else
        {
            _minDistance = _orbitCamera.Radius;
            _orbitCamera.CurrentRadius = Mathf.SmoothDamp(_orbitCamera.CurrentRadius, _minDistance, ref _camVelocity, smoothReset);
            colliding = false;
        }


        for (int i = 0; i < clipPoints.Length; i++)
        {
            Debug.DrawLine(clipPoints[i], _orbitCamera.Target.position, Color.red);
        }
        
        Debug.DrawRay(_orbitCamera.Target.position, direction * _minDistance, Color.blue);
    }

    private bool CollisionWithClipPoints(Vector3[] clipPoints, Vector3 targetPosition)
    {
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(targetPosition, clipPoints[i] - targetPosition);
            float distance = Vector3.Distance(clipPoints[i], targetPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, collisionLayer))
            {
                return true;
            }
        }
        return false;
    }

    private float GetShortestDistance(Vector3[] clipPoints, Vector3 targetPosition)
    {
        float distance = -1;
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(targetPosition, clipPoints[i] - targetPosition);
            float dist = Vector3.Distance(clipPoints[i], targetPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, dist, collisionLayer))
            {
                if (distance == -1)
                {
                    distance = hit.distance;
                }

                Vector3 hitVec = clipPoints[i] - targetPosition;
                Vector3 camVec = transform.position - targetPosition;
                float relativeDist = hit.distance * Mathf.Cos(Vector3.Angle(hitVec, camVec) * Mathf.Deg2Rad);
                
                if (relativeDist < distance)
                {
                    distance = relativeDist;
                }

            }
        }
        return distance;
    }

    private float GetAveragedDistance(Vector3[] clipPoints, Vector3 targetPosition)
    {
        float distance = -1;
        float sum = 0;
        float count = 0;
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(targetPosition, clipPoints[i] - targetPosition);
            float dist = Vector3.Distance(clipPoints[i], targetPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, dist, collisionLayer))
            {
                Vector3 hitVec = clipPoints[i] - targetPosition;
                Vector3 camVec = transform.position - targetPosition;
                float relativeDist = hit.distance * Mathf.Cos(Vector3.Angle(hitVec, camVec) * Mathf.Deg2Rad);
                
                count++;
                sum += relativeDist;
            }
        }
        distance = sum / count;
        return distance;
    }
    private void Awake()
    {
        camera = GetComponent<Camera>();
        _orbitCamera = GetComponent<OrbitCamera>();
        _transform = transform;
    }

    private void Start()
    {
        _minDistance = _orbitCamera.Radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
