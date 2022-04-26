using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Vector3Var playerPos;
    
    private Transform _transform;
    private bool active;
    [SerializeField] private float range;
    [SerializeField] private float animSpeed;
    

    private void Update()
    {
        float distance = (_transform.position - playerPos.Value).magnitude;
        if (distance <= range && active) { active = false; }
        
        if (!active)
        {
            _transform.position = Vector3.Lerp(_transform.position, playerPos.Value, animSpeed * Time.deltaTime);
        }
        if (distance <= 0.8f)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _transform = transform;
        active = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
