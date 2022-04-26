using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDive : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float diveStrength;
    [SerializeField] float diveCooldown;
    [SerializeField] float upward;

    float DiveStrength
    {
        get => diveStrength;
    }

    float DiveCooldown
    {
        get => diveCooldown;
    }

    private void Update()
    {
        TryDive();
        Debug.Log(rb.velocity.magnitude);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TryDive()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dive();
        }
    }

    void Dive()
    {
        Vector3 diveVelocity = transform.forward * diveStrength;
        
        //rb.AddForce(diveVelocity, ForceMode.VelocityChange);

        Vector3 newVel = diveVelocity;
        newVel.y = upward;

        rb.velocity = newVel;

    }
}
