using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public Vector3 ModifiedGravity(float _multiplier) //Returns the custom gravity for this fixedUpdate
    {
        //Custom gravity
        Vector3 addGravity = -Physics.gravity;
        addGravity += Physics.gravity * _multiplier;
        return addGravity;
    }
}
