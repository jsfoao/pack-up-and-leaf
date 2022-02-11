using UnityEngine;

public class Damager : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ApplyOneDamage(Collider collider)
    {
        PlayerEntity entity = collider.GetComponent<PlayerEntity>();
        if (entity == null) { return; }
        entity.Damage(1);
    }

    public void ApplyDeath(Collider collider)
    {
        PlayerEntity entity = collider.GetComponent<PlayerEntity>();
        if (entity == null) { return; }
        entity.Damage(4);
    }

    public void FallOnWater(Collider collider)
    {
        PlayerEntity playerEntity = collider.GetComponent<PlayerEntity>();
        if (playerEntity == null) { return; }
        playerEntity.onFallWater.Invoke();
    }
}
