using System;
using UnityEngine;

[RequireComponent(typeof(OrbitCamera))]
public class CameraCollision : MonoBehaviour
{
    public LayerMask collisionLayer;

    private bool colliding = false;
    private Vector3[] adjustedCameraClipPoints;
    private Vector3[] desiredCameraClipPoints;
    [SerializeField] private float collisionSpaceSize = 3.41f;
    [SerializeField] private float offset;
    [SerializeField] private float smoothTime;
    [SerializeField] private float smoothAfterColliding;
    [SerializeField] private float smoothNoColliding;
    
    
    
    
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
        float x = Mathf.Tan(camera.fieldOfView / collisionSpaceSize) * z;
        float y = x / camera.aspect;

        Vector3 camPosition = _transform.position;
        Vector3 camRight = _transform.right;
        Vector3 camUp = _transform.up;
        Vector3 camForward = _transform.forward;

        // top left
        clipPoints[0] = (camRight * -x + camUp * y + camForward * z) * offset + camPosition;

        // top right
        clipPoints[1] = (camRight * x + camUp * y + camForward * z)* offset + camPosition ;

        // bottom left
        clipPoints[2] = (camRight * -x + camUp * -y + camForward * z)* offset + camPosition;

        // bottom right
        clipPoints[3] = (camRight * x + camUp * -y + camForward * z)* offset + camPosition;
        
        // camera's position
        clipPoints[4] = camPosition - camForward;
        
        // Collision with clip points
        if (CollisionWithClipPoints(clipPoints, _orbitCamera.Target.position))
        {
            float newDistance = GetShortestDistance(clipPoints, _orbitCamera.Target.position) - .5f;
            if (newDistance < _orbitCamera.Radius)
            {
                if (newDistance < 0f)
                {
                    newDistance = 0f;
                }
                
                if (colliding)
                {
                    _minDistance = newDistance;
                    _orbitCamera.CurrentRadius = Mathf.SmoothDamp(_orbitCamera.CurrentRadius, _minDistance, ref _camVelocity, smoothAfterColliding);
                }
                else
                {
                    _minDistance = newDistance;
                    _orbitCamera.CurrentRadius = Mathf.SmoothDamp(_orbitCamera.CurrentRadius, _minDistance, ref _camVelocity, smoothTime);
                }
            }
            colliding = true;
        }
        else
        {
            _orbitCamera.CurrentRadius = Mathf.SmoothDamp(_orbitCamera.CurrentRadius, _orbitCamera.Radius, ref _camVelocity, smoothNoColliding);
            colliding = false;
        }
        
        Debug.Log("Colliding: " + colliding);

        for (int i = 0; i < clipPoints.Length; i++)
        {
            Debug.DrawLine(clipPoints[i], _orbitCamera.Target.position, Color.red);
        }
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

                if (hit.distance < distance)
                {
                    distance = hit.distance;
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
                count++;
                sum += hit.distance;
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
        adjustedCameraClipPoints = new Vector3[5];
        desiredCameraClipPoints = new Vector3[5];
        _minDistance = 0;
    }
}
