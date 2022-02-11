using UnityEngine;
using UnityEngine.Events;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifetime;
    
    [SerializeField] private LayerMask blockMask;
    [SerializeField, Range(1f, 100f)] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float force;
    
    [SerializeField] private bool shootEnabled;

    private Transform target;
    private Transform _transform;
    private Vector3 _direction;

    private Color _rangeColor;
    private float _currentTime;

    [SerializeField] private UnityEvent onShoot;
    

    
    
    private void FollowTarget()
    {
        Quaternion _lookRotation = Quaternion.LookRotation(_direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, force * Time.deltaTime);
    }

    private void Shoot()
    {
        if (!shootEnabled) { return; }
        onShoot.Invoke();
        Vector3 shootDirection = _transform.forward;
        GameObject instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        instance.transform.forward = shootDirection;
        instance.GetComponent<Rigidbody>().AddForce(shootDirection * force, ForceMode.Impulse);
        Destroy(instance, bulletLifetime);
    }
    private void ShootTimer()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            Shoot();
            _currentTime = fireRate;
        }
    }
    
    private bool InRange()
    {
        return _direction.magnitude <= range;
    }
    private bool BlockedRaycast()
    {
        return Physics.Linecast(_transform.position, target.position, blockMask);
    }
    
    private void Update()
    {      
        // TODO fix checking for null every frame
        if (target == null) { return; }
        DebugRange();
        DebugDirection();
        
        
        _direction = target.position - _transform.position;
        if (InRange() && !BlockedRaycast())
        {
            FollowTarget();
            ShootTimer();
        }
        else
        {
            // Resetting fireRate
            if (_currentTime < fireRate)
            {
                _currentTime = fireRate;
            }
        }
        
        Debug.DrawRay(_transform.position, _transform.forward * 2f, Color.blue);
    }
    
    #region Debugging
    private void DebugRange()
    {
        _rangeColor = InRange() ? Color.green : Color.red;
    }

    private void DebugDirection()
    {
        Color rayColor;
        if (BlockedRaycast())
        {
            rayColor = Color.red;
        }
        else
        {
            rayColor = Color.green;
        }
        Debug.DrawRay(_transform.position, _direction, rayColor);
    }
    #endregion

    private void Start()
    {
        _transform = transform;
        PlayerEntity entity = FindObjectOfType<PlayerEntity>();
        if (entity != null)
        {
            target = entity.transform;
        }

        _currentTime = fireRate;
    }

    private void OnValidate()
    {
        _rangeColor = Color.red;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _rangeColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
