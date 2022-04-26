using UnityEngine;
using UnityEngine.Events;
using ScriptableEvents;

public abstract class IEntity : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public IHealth healthRef;
    [SerializeField] public UnityEvent onDeathBehaviour;
    [SerializeField] public UnityEvent onDamaged;
    [SerializeField] public UnityEvent onRespawn;
    [SerializeField] public UnityEvent onFallWater;
    [SerializeField] ScriptableEventVoid onDeath;

    public Vector3 spawnPoint;

    public void Damage(int amount)
    {
        healthRef.Damage(amount);
        onDamaged.Invoke();
        if (healthRef.Health <= 0)
        {
            DeathBehaviour();
        }
    }

    public void Heal(int amount)
    {
        healthRef.Heal(amount); 
    }

    public void ResetHealth()
    {
        healthRef.Reset();
    }

    public void OnFallWater()
    {
        onFallWater.Invoke();
    }
    
    public void DeathBehaviour()
    {
        onDeathBehaviour.Invoke();
        onDeath?.Raise();
    }

    public void Respawn()
    {
        onRespawn.Invoke();
        Spawn(spawnPoint, Quaternion.identity);
    }
    
    public void Spawn(Vector3 worldPos, Quaternion rotation)
    {
        ResetHealth();
        transform.SetPositionAndRotation(worldPos, rotation);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        healthRef = new IHealth(maxHealth);
    }
}