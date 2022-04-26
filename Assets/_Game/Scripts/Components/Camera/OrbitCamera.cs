using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    public CameraState cameraState = CameraState.Follow;
    
    [Header("Follow")] 
    [SerializeField] private float defaultHeight;
    private Vector3 _focusPoint;
    [SerializeField] private Transform target;
    [SerializeField, Range(0f, 5f), Tooltip("Focus radius")] private float focusRadius;
    [SerializeField, Range(0f, 1f)] private float focusCentering = 0.5f;
    [SerializeField, Range(0f, 360f)] private float facingCameraAngle = 100f;
    

    [Header("Orbit")]
    [SerializeField, Tooltip("Camera distance radius")] private float radius;
    [SerializeField, Range(0.1f, 10f), Tooltip("Orbital rotation speed")] private float rotationSpeed;
    private float currentRadius = 5f;
    [SerializeField] private Vector2 orbitAngles;
    [SerializeField, Range(-89f, 89f)] private float minVerticalAngle;
    [SerializeField, Range(-89f, 89f)] private float maxVerticalAngle;
   
    
    private Quaternion _lookRotation;
    private bool isFacingCamera;
    
    public Vector2 Angles
    {
        get => orbitAngles;
        set => orbitAngles = value;
    }
    public float Radius
    {
        get => radius;
        set => radius = value;
    }
    public float CurrentRadius
    {
        get => currentRadius;
        set => currentRadius = value;
    }
    public Transform Target
    {
        get => target;
    }

    #region Helper funcs
    private void UpdateFocusPoint()
    {
        Vector3 targetPoint = target.position;
        if (focusRadius > 0f)
        {
            float distance = (targetPoint - _focusPoint).magnitude;
            float t = 1f;
            if (distance > 0.001f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            _focusPoint = Vector3.Lerp(targetPoint, _focusPoint, t);
        }
        else
        {
            _focusPoint = targetPoint;
        }
    }

    private void ConstrainAngles()
    {
        orbitAngles.y = Mathf.Clamp(orbitAngles.y, minVerticalAngle, maxVerticalAngle);
    }
    
    private void LoopAngle()
    {
        if (orbitAngles.x < 0f)
        {
            orbitAngles.x += 360f;
        }
        else if (orbitAngles.x >= 360f)
        {
            orbitAngles.x -= 360f;
        }
    }

    public void LerpRadius(float newRadius, float speed)
    {
        radius = Mathf.Lerp(radius, newRadius, speed);
    }
    #endregion

    #region States Execution
    private void ExecuteFollow()
    {
        LoopAngle();
        Vector3 direction = target.position - _focusPoint;
        if (direction.magnitude < 0.1f)
        {
            transform.rotation = transform.localRotation;
            return;
        }

        #region Convert angles
        float angle = Vector3.Angle(target.forward, Vector3.forward);
        float relativeAngle;
        if (target.forward.x < 0 && target.forward.z < 0)
        {
            relativeAngle = 360 - angle;
        }
        else if (target.forward.x < 0 && target.forward.z > 0)
        {
            relativeAngle = 360 - angle;
        }
        else
        {
            relativeAngle = angle;
        }
        
        #endregion
        #region Orbit
        // speed for when inside and outside free zone area
        float speed = direction.magnitude < focusRadius - 0.01f ? 0.1f : rotationSpeed;
        
        if (!isFacingCamera)
        {
            orbitAngles.x = Mathf.LerpAngle(orbitAngles.x, relativeAngle, speed * Time.unscaledDeltaTime);
        }

        orbitAngles.y = Mathf.LerpAngle(orbitAngles.y, defaultHeight, speed * Time.unscaledDeltaTime);
        Quaternion newRotation = Quaternion.Euler(new Vector3(orbitAngles.y, orbitAngles.x, 0f));
        _lookRotation = newRotation;
        
        // looking at target
        Vector3 lookDirection = _lookRotation * Vector3.forward;
        Vector3 lookPosition = _focusPoint - lookDirection * currentRadius;
        
        // set pos and rotation
        transform.SetPositionAndRotation(lookPosition, _lookRotation);
        #endregion
    }
    
    private void ExecuteOrbit()
    {
        ConstrainAngles();
        LoopAngle();
        Quaternion newRotation = Quaternion.Euler(new Vector3(orbitAngles.y, orbitAngles.x, 0f));
        _lookRotation = newRotation;
        
        // looking at target
        Vector3 lookDirection = _lookRotation * Vector3.forward;
        Vector3 lookPosition = _focusPoint - lookDirection * currentRadius;
        
        // set pos and rotation
        transform.SetPositionAndRotation(lookPosition, _lookRotation);
    }
    #endregion
    
    private void LateUpdate()
    {
        if (target == null) { return; }
        
        float cameraAngle = Vector3.Angle(target.forward, transform.forward);
        isFacingCamera = cameraAngle > facingCameraAngle;
        
        Debug.DrawLine(target.position, _focusPoint, Color.green);
        Debug.DrawRay(target.position, target.forward, Color.blue);

        UpdateFocusPoint();
        switch (cameraState)
        {
            case CameraState.Follow:
                ExecuteFollow();
                break;
            case CameraState.Orbit:
                ExecuteOrbit();
                break;
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
        currentRadius = radius;
    }

    #region Debugging
    public static void DrawEllipse(Vector3 pos, Vector3 forward, Vector3 up, float radiusX, float radiusY, int segments, Color color, float duration = 0)
    {
        float angle = 0f;
        Quaternion rot = Quaternion.LookRotation(forward, up);
        Vector3 lastPoint = Vector3.zero;
        Vector3 thisPoint = Vector3.zero;
 
        for (int i = 0; i < segments + 1; i++)
        {
            thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
            thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;
 
            if (i > 0)
            {
                Debug.DrawLine(rot * lastPoint + pos, rot * thisPoint + pos, color, duration);
            }
 
            lastPoint = thisPoint;
            angle += 360f / segments;
        }
    }

    private void OnDrawGizmos()
    {
        if (target == null) { return; }
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(target.position, currentRadius);
        Gizmos.DrawWireSphere(_focusPoint, focusRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_focusPoint, 0.1f);
    }
    #endregion
}

public enum CameraState
{
    Follow,
    Orbit
}