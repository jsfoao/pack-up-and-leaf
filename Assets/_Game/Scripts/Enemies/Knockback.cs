using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    [SerializeField] private bool reverse;
    
    private void ApplyKnockback(Collider collider, Vector2 force, bool reverse)
    {
        Rigidbody otherRb = collider.GetComponent<Rigidbody>();
        if (otherRb == null) { return; }

        int multiplier = reverse ? -1 : 1;

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 thisDirection;
        thisDirection = rb == null ? Vector3.zero : rb.velocity;
        
        Vector3 otherDirection = new Vector3(otherRb.velocity.x, 0f, otherRb.velocity.z);
        Vector3 totalDirection = (thisDirection + otherDirection).normalized;
        Vector3 forceToApply = multiplier * totalDirection * force.x + Vector3.up * force.y;

        otherRb.velocity = Vector3.zero;
        otherRb.AddForce(forceToApply);
    }

    public void ApplyKnockforward(Collider collider)
    {
        ApplyKnockback(collider, force, false);
    }
    
    public void ApplyKnockback(Collider collider)
    {
        ApplyKnockback(collider, force, true);
    }
}
