using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    
    private IHealth _healthRef;

    private void Awake()
    {
        _healthRef = new IHealth(maxHealth);
        Debug.Log(_healthRef.Health);
    }
}
