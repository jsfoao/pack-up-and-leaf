using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRolling : MonoBehaviour
{

    Rigidbody rb;
    PlayerManager player;
    CustomGravity grav;
    PlayerEventHandler events;

    public GameObject ballObject;
    [SerializeField] SphereCollider characterCollider;

    public bool isRolling;

    [Header("ROLLING PROPERTIES:")]
    [SerializeField] FloatRef rollingMaxSpeed;
    [SerializeField] FloatRef rollingAcceleration;
    [SerializeField] FloatRef rollingMaxAcceleration;
    [SerializeField] FloatRef rollingJumpForce;
    [SerializeField] CurveRef accelFromDot;

    [SerializeField, Min(0.1f)] float ballRadius = 30f;
    
    public bool orthonogalRolling = true;

    [Header("GRAVITY PROPERTIES:")]
    [SerializeField] FloatRef gravityMultiplier;
    [SerializeField] [Range(0,1)] float surfaceAngleTreshold;

    
    Vector3 contOnVelo;
    Vector3 accel2Add;
    Vector3 surfaceNormal;

    #region Properties
    float RollingMaxSpeed
    {
        get => rollingMaxSpeed.Value;
    }
    float RollingAcceleration
    {
        get => rollingAcceleration.Value;
    }

    float RollingMaxAcceleration
    {
        get => rollingMaxAcceleration.Value;
    }

    float RollingJumpForce
    {
        get => rollingJumpForce.Value;
    }

    float GravityMultiplier
    {
        get => gravityMultiplier.Value;
    }


    AnimationCurve AccelFromDot
    {
        get => accelFromDot.Value;
    }

    
    #endregion

    #region Methods
    public void StartRoll()
    {
        isRolling = true;
        ballObject.SetActive(true);
        characterCollider.enabled = true;
    }
    public void StopRoll()
    {
        isRolling = false;
        ballObject.SetActive(false);
        characterCollider.enabled = false;
    }
    public void GroundCheck(out bool _isGrounded)
    {
        _isGrounded = true; //This method currently doesnt do anything yet
    }
    

    public void ApplyRollingMovement(Vector3 _inputDirection) //This has to be in fixed update
    {
        
        Vector3 inputDir = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        Vector3 groundVel = rb.velocity;
        groundVel.y = 0;
        accel2Add = inputDir * RollingAcceleration;

        float turnAccelFactor = Vector3.Dot(accel2Add, groundVel.normalized);
        turnAccelFactor = AccelFromDot.Evaluate(turnAccelFactor);
        accel2Add = accel2Add * turnAccelFactor;
        
        if (player.isGrounded && orthonogalRolling)
        {
            Vector3 surfaceNormalWithoutRotation = surfaceNormal;
            surfaceNormalWithoutRotation.x = 0;
            surfaceNormalWithoutRotation.y = 0;
            accel2Add = Vector3.ProjectOnPlane(accel2Add, surfaceNormal);
        }
        
        /*
        contOnVelo = Vector3.Project(accel2Add, groundVel);

        if (groundVel.magnitude > RollingMaxSpeed)
        {
            accel2Add = accel2Add - (contOnVelo + groundVel);
        }
        */
        
        float accelGroundDot = (Vector3.Dot(accel2Add, groundVel.normalized));
        if (accelGroundDot <= 0)
        {
            accelGroundDot = 0;
        }
        contOnVelo = accelGroundDot * groundVel.normalized;
        
        if (groundVel.magnitude > RollingMaxSpeed)
        {
            accel2Add = accel2Add - contOnVelo;
        }
        
        //Apply force
        rb.AddForce(accel2Add);

        //Custom gravity
        rb.AddForce(grav.ModifiedGravity(GravityMultiplier));


        //Sharp turn feature, can make sharp turns better, even
        /*
        if (groundVel.magnitude > RollingMaxSpeed)
        {
            float velDirMod = Vector3.Dot(groundVel.normalized, inputDir);
            float factor = AccelFromDot.Evaluate(velDirMod);
            accel2Add = accel2Add * factor;
        }
        */

        //Rolling rotation
        Vector3 axisToRotate = Vector3.Cross(rb.velocity.normalized, surfaceNormal);

        float movement = rb.velocity.magnitude * Time.fixedDeltaTime;
        if (movement < 0.001f)
        {
            return;
        }
        float angle = movement * (180f / Mathf.PI) / ballRadius;
        ballObject.transform.rotation = Quaternion.Euler(axisToRotate * -angle) * ballObject.transform.rotation;
        //Quaternion newRot = Quaternion.AngleAxis(angle, axisToRotate);
        //ballObject.transform.localRotation *= newRot;

        events.onRollFast.Invoke(rb.velocity, player.isGrounded);
        
    }

    public void RollingJump(bool _jumpInput, bool _isGrounded) //This has to be in Update
    {
        if (!_isGrounded)
        { return; }
        Vector3 jumpForceVector = Vector3.up * RollingJumpForce;
        rb.velocity += jumpForceVector;
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerManager>();
        grav = GetComponent<CustomGravity>();
        events = GetComponent<PlayerEventHandler>();
    }

    private void Start()
    {
        //ballRadius = characterCollider.radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + accel2Add);
    }

    private void OnCollisionStay(Collision collision)
    {

        if (isRolling && !collision.transform.CompareTag("Slippery"))
        {
            ContactPoint[] contacts = new ContactPoint[5];
            int amount = collision.GetContacts(contacts);

            for (int i = 0; i < amount; i++)
            {
                float am = Vector3.Dot(contacts[i].normal, Vector3.up);
                if (am > surfaceAngleTreshold)
                {
                    player.isGrounded = true;
                    events.onGroundStateChange.Invoke(true);
                    surfaceNormal = contacts[i].normal;

                    
                    events.onRollRedirect.Invoke(contacts[i].point, rb.velocity);
                    return;
                }
                else

                {
                    events.onGroundStateChange.Invoke(false);
                    player.isGrounded = false;
                }
            }
        }
        
        /*
        float am = Vector3.Dot(c.normal, Vector3.up);
        Debug.Log(am);
        if (am > surfaceAngleTreshold)
        {
            player.isGrounded = true;
        }
        else
        {
            player.isGrounded = false;
        }
        */
           
    }
    

    

    private void OnCollisionExit(Collision collision)
    {
        if (isRolling && !collision.transform.CompareTag("Slippery"))
        {
            player.isGrounded = false;
            events.onGroundStateChange.Invoke(false);
        }
        

    }

    
    #endregion
}
