using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidbodyFloater : MonoBehaviour
{
    //This component handles the soft pushing off the ground while in the non-rolling state
    
    //References
    Rigidbody rb;
    PlayerEventHandler events;

    
    [Header("FLOAT PROPERTIES:")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] FloatRef rayLength;
    [SerializeField] FloatRef floatHeight;
    [SerializeField] FloatRef rideFloatStrength;
    [SerializeField] FloatRef rideFloatDamper;
    [SerializeField] float disableJumpDuration;

    public bool isGrounded;
    bool disabledFloat;


    #region PROPERTIES
    float RayLength
    {
        get => rayLength.Value;
    }
    float FloatHeight
    {
        get => floatHeight.Value;
    }
    float RideFloatStrength
    {
        get => rideFloatStrength.Value;
    }
    float RideFloatDamper
    {
        get => rideFloatDamper.Value;
    }

    UnityEvent OnLand
    {
        get => events.onLand;
    }

    UnityEvent<bool> OnGroundStateChanged
    {
        get => events.onGroundStateChange;
    }
    #endregion

    #region Methods

    IEnumerator DisabledJumpCoroutine()
    {
        disabledFloat = true;
        yield return new WaitForSeconds(disableJumpDuration);
        disabledFloat = false;
    }
    public void FloatOnGround(out bool _isGrounded)
    {
        /*
        if (!isFloating)
        {
            _isGrounded = true;
            return;
        }
        */
        
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayLength, groundMask)) //Raycasts into the floor
        {
            OnGroundStateChanged.Invoke(true);
            if (!isGrounded)
            {
                OnLand.Invoke();
                
                
            }
            _isGrounded = true;
            isGrounded = true;
            Vector3 vel = rb.velocity;
            Vector3 rayDir = transform.TransformDirection(Vector3.down);

            Vector3 otherVel = Vector3.zero;

            float rayDirVel = Vector3.Dot(Vector3.down, vel);
            float otherDirVel = Vector3.Dot(Vector3.down, otherVel);

            float relVel = rayDirVel - otherDirVel; //This might be useful if we want to make the player stick better as you run up ramps

            float x = hit.distance - FloatHeight;
            float springForce = (x * RideFloatStrength) - (vel.y * RideFloatDamper);

            if (!disabledFloat)
            {
                rb.AddForce(Vector3.down * springForce * Time.deltaTime, ForceMode.Acceleration);
            }
            

            /*
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(rayDir * -springForce, hit.point);
            }
            */
        }
        else
        {
            OnGroundStateChanged.Invoke(false);
            if (isGrounded)
            {
                
            }
            _isGrounded = false;
            isGrounded = false;
        }
    }

    public void DisableFloating()
    {
        StartCoroutine(DisabledJumpCoroutine());
    }
    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        events = GetComponent<PlayerEventHandler>();
    }





    #endregion
}
