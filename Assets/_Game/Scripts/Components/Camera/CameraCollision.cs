using UnityEngine;

[RequireComponent(typeof(OrbitCamera))]
public class CameraCollision : MonoBehaviour
{
    [SerializeField] private LayerMask camMask;
    [SerializeField, Range(0f, 20f)] private float minRadius = 1f;
    [SerializeField, Range(0f, 5f)] private float threshold = 0.5f;
    private OrbitCamera _orbitCamera;

    private void LateUpdate()
    {
        if (_orbitCamera.Target == null) { return; }
        
        _orbitCamera.CurrentRadius = Mathf.Clamp(_orbitCamera.CurrentRadius, minRadius, 100f);
        
        Vector3 targetPos = _orbitCamera.Target.transform.position;
        Vector3 direction = (transform.position - targetPos).normalized;
        Vector3 relativePos = targetPos + direction * _orbitCamera.Radius;

        if (Physics.Linecast(targetPos, relativePos, out RaycastHit wallHit, camMask))
        {
            float distanceToHit = (relativePos - wallHit.point).magnitude;
            _orbitCamera.CurrentRadius = _orbitCamera.Radius - distanceToHit - threshold;
            // Debug.DrawLine(targetPos, wallHit.point, Color.green);
            // Debug.DrawLine(transform.position, wallHit.point, Color.red);
        }
        else
        {
            // TODO: Fix stuttering on edge cases
            // TODO: Refactor this being called every frame
            _orbitCamera.CurrentRadius = _orbitCamera.Radius;
            // Debug.DrawLine(targetPos, relativePos, Color.blue);
        }
    }

    private void OnValidate()
    {
        if (minRadius < threshold)
        {
            minRadius = threshold;
        }
    }

    private void Start()
    {
        _orbitCamera = GetComponent<OrbitCamera>();
    }
}
