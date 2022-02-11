using UnityEngine;

public class IHealth
{
    public int MaxHealth;
    public int Health;

    public IHealth(int maxHealth)
    {
        MaxHealth = Mathf.Max(1, maxHealth);
        Health = MaxHealth;
    }
    
    public void Damage(int amount)
    {
        int newHealth = Health - amount;
        Health = Mathf.Max(0, newHealth);
    }

    public void Heal(int amount)
    {
        int newHealth = Health + amount;
        Health = Mathf.Min(MaxHealth, newHealth);
    }

    public void Set(int amount)
    {
        Health = amount;
    }

    public void Reset()
    {
        Health = MaxHealth;
    }
}
