using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private Transform target;
    [SerializeField] private Transform player;

    [Header("Standard Follow")] 
    [SerializeField] private float height;
    [SerializeField] private float freeRadius;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float resetSpeed;

    [Header("Position")]
    [SerializeField] public float radius;
    [SerializeField] private float moveSpeed;

    [Header("Axis Rotation")]
    [SerializeField] public Vector2 angle;
    private Vector2 _relativeAngle;
    [SerializeField] private float orbitSpeed;

    [SerializeField] private AngleRange verticalAngleRange;
    [SerializeField] private AngleRange horizontalAngleRange;
    
    private float _lerpRadius;
    private bool _cameraLerping;
    [SerializeField] private bool _hasInput;
    
    private void ExecuteOrbitCamera()
    {
        // clamping angles
        if (horizontalAngleRange.Lock)
        {
            angle.x = Mathf.Clamp(angle.x, horizontalAngleRange.Min, horizontalAngleRange.Max);
        }
        if (verticalAngleRange.Lock)
        {
            angle.y = Mathf.Clamp(angle.y, verticalAngleRange.Min, verticalAngleRange.Max);
        }
        _lerpRadius = Mathf.Lerp(_lerpRadius, radius, moveSpeed * Time.deltaTime);

        // axis rotation
        _relativeAngle = Vector2.Lerp(_relativeAngle, angle, orbitSpeed * Time.deltaTime);

        Quaternion xRotation = Quaternion.AngleAxis(-_relativeAngle.x, player.transform.up);
        Quaternion yRotation = Quaternion.AngleAxis(-_relativeAngle.y, player.transform.right);

        // calculating new position
        Vector3 horizontalPos = xRotation * Vector3.forward * _lerpRadius;
        Vector3 newPos = horizontalPos;
        
        _transform.position = target.position + newPos + new Vector3(0f, height, 0f);
        
        #region Debugging
        Debug.DrawLine(target.position, target.position + newPos, Color.red);
        #endregion
    }

    private void LateUpdate()
    {
        DrawEllipse(target.position, Vector3.up, Vector3.forward, freeRadius, freeRadius, 20, Color.white);
        Debug.DrawLine(player.position, target.position, Color.red);
        Debug.DrawRay(_transform.position,_transform.forward * 2f, Color.blue);
        
        float distance = (player.position - target.position).magnitude;
        Quaternion playerRotation = Quaternion.LookRotation(player.position - transform.position);

        ExecuteOrbitCamera();
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, turnSpeed * Time.deltaTime);

        // free move zone
        if (_cameraLerping)
        {
            target.position = Vector3.Lerp(target.position, player.position, resetSpeed * Time.deltaTime);
        }
        if (distance >= freeRadius)
        {
            _cameraLerping = true;
            Debug.DrawLine(player.position, target.position, Color.green);
        }
        else if (distance <= 0.1f)
        {
            _cameraLerping = false;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _transform = transform;
        _cameraLerping = false;
    }
    
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
}

[Serializable]
struct AngleRange
{
    public bool Lock;
    public float Min;
    public float Max;
}
