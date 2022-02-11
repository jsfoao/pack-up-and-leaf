using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;



public class PlayerLocomotion : MonoBehaviour
{

    //This component handles walking and jumping while in the non-rolling state

    //public UnityEvent<bool> onStartGroundWalk;

    Rigidbody rb;
    PlayerManager player;
    CustomGravity grav;
    PlayerEventHandler events;

    [SerializeField] Transform forwardTransform;
    
    [Header("GROUNDED MOVEMENT PROPERTIES:")]
    [SerializeField] FloatRef maxSpeed;
    [SerializeField] FloatRef acceleration;
    [SerializeField] FloatRef maxAccelerationForce;
    [SerializeField] FloatRef deacceleration;
    [SerializeField] CurveRef accelFactorFromDot;
    [SerializeField] [Range(-1, 0)] float skidTreshold;

    [Header("AERIAL MOVEMENT PROPERTIES:")]
    
    [SerializeField] FloatRef airMaxSpeed;
    [SerializeField] FloatRef airAcceleration;
    [SerializeField] FloatRef airDeacceleration;
    [SerializeField] CurveRef airAccelFactorFromDot;

    [Header("GRAVITY PROPERTIES:")]
    [SerializeField] FloatRef gravityMultiplier;

    bool isWalking;
    bool isSkidding;

    #region PROPERTIES



    public float MaxSpeed
    {
        get => maxSpeed.Value;
    }

    [SerializeField] float rotationSpeed = 15;
    public float RotationSpeed
    {
        get => rotationSpeed;
    }

    public float Acceleration
    {
        get => acceleration.Value;
    }

    public float Deacceleration
    {
        get => deacceleration.Value;
    }

    public float MaxAccelerationForce
    {
        get => maxAccelerationForce.Value;
    }

    public AnimationCurve AccelFactorFromDot
    {
        get => accelFactorFromDot.Value;
    }

    public float AirMaxSpeed
    {
        get => airMaxSpeed.Value;
    }

    public float AirAcceleration
    {
        get => airAcceleration.Value;
    }

    public float AirDeacceleration
    {
        get => airDeacceleration.Value;
    }
    public float GravityMultiplier
    {
        get => gravityMultiplier.Value;
    }

    public AnimationCurve AirAccelFactorFromDot
    {
        get => airAccelFactorFromDot.Value;
    }

    public UnityEvent<bool> OnGroundWalkStart
    {
        get => events.onGroundStartWalk;
    }

    public UnityEvent<bool> OnGroundSkidStart
    {
        get => events.onGroundStartSkid;
    }

    

    #endregion

    #region Methods
    public void ApplyMovement(IInput inputs, bool _isGrounded) //This has to be in FixedUpdate
    {
        #region Some ugly, old code
        /*
        //Accelerate the player 
        Vector3 groundVel = rb.velocity;
        groundVel.y = 0;

        Vector3 goalVel = new Vector3(_inputDirection.x, 0, _inputDirection.y);


        float velDot = Vector3.Dot(goalVel, m_goalVel.normalized);
        float accel = Acceleration * AccelFactorFromDot.Evaluate(velDot); //Get factor from dot
        
        goalVel.Normalize();
        goalVel = goalVel * MaxSpeed;
        m_goalVel = Vector3.MoveTowards(m_goalVel, goalVel, accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (m_goalVel - groundVel) / Time.fixedDeltaTime;

        //Make the dot allow for sharper turns
        float maxaccel = MaxAccelerationForce * AccelFactorFromDot.Evaluate(velDot);
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxaccel);
    


        rb.AddForce(neededAccel, ForceMode.Acceleration);
        
        
        Vector3 inputDir = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        Vector3 groundVel = rb.velocity;
        groundVel.y = 0;
        accel2Add = inputDir * RollingAcceleration;
        
        contOnVelo = Vector3.Project(accel2Add, groundVel);

        if (groundVel.magnitude > RollingMaxSpeed)
        {
            accel2Add = accel2Add - (contOnVelo + groundVel);
        }
        
        */
        #endregion
        

        float accel = _isGrounded ? Acceleration : AirAcceleration;
        float maxspeed = _isGrounded ? MaxSpeed : AirMaxSpeed;
        float deaccel = _isGrounded ? Deacceleration : AirDeacceleration;

        Vector3 accel2Add;
        Vector3 contOnVelo;

        Vector3 inputDir = new Vector3(inputs.InputDirection.x, 0, inputs.InputDirection.y);
        Vector3 groundVel = rb.velocity;
        groundVel.y = 0;
        accel2Add = inputDir * accel;

        float accelMultiplier = Vector3.Dot(accel2Add.normalized, groundVel.normalized);
        //Debug.Log(accelMultiplier);
        accel2Add = accel2Add * AccelFactorFromDot.Evaluate(accelMultiplier);
        if (accelMultiplier < skidTreshold && player.isGrounded)
        {
            if (!isSkidding)
            {
                OnGroundSkidStart.Invoke(true);
                events.onStartSkidding.Invoke();
            }
            
            isSkidding = true;
        }
        else
        {
            isSkidding = false;
            OnGroundSkidStart.Invoke(false);
        }

        
        if (inputDir != Vector3.zero && player.isGrounded)
        {
            if (!isWalking)
            {
                OnGroundWalkStart.Invoke(true);
                events.onStartWalking.Invoke();
            }
            isWalking = true;
            
        }
        else
        {
            isWalking = false;
            OnGroundWalkStart.Invoke(false);
        }

        //

        float accelGroundDot = (Vector3.Dot(accel2Add, groundVel.normalized));
        if (accelGroundDot <= 0)
        {
            accelGroundDot = 0;
        }
        contOnVelo = accelGroundDot * groundVel.normalized;

        if (groundVel.magnitude > maxspeed)
        {
            accel2Add = accel2Add - contOnVelo;
        }

        //Apply force
        rb.AddForce(accel2Add);

        //Friction
        Vector3 deaccelVector = Vector3.MoveTowards(groundVel, Vector3.zero, deaccel * Time.fixedDeltaTime);
        deaccelVector.y = rb.velocity.y;

        rb.velocity = deaccelVector;

        //Custom gravity
        rb.AddForce(grav.ModifiedGravity(GravityMultiplier));

        //Rotate player
        Vector3 targetDirection = new Vector3(inputs.InputDirection.x, 0, inputs.InputDirection.y);

        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        float rotationSpeed = RotationSpeed * AccelFactorFromDot.Evaluate(accelMultiplier);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        forwardTransform.rotation = targetRotation;
        transform.rotation = playerRotation;



    }

    public void RotatePlayer(IInput input) //This has to be in Update
    {
        
    }
    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grav = GetComponent<CustomGravity>();
        player = GetComponent<PlayerManager>();
        events = GetComponent<PlayerEventHandler>();

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + m_goalVel);
    }

    #endregion




}
