using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    Rigidbody rb;
    PlayerManager player;
    PlayerEventHandler events;

    #region Variables
    [Header("JUMP PROPERTIES:")]
    [SerializeField] FloatRef jumpForce;
    [SerializeField] FloatRef lowJumpMultiplier;
    [SerializeField] bool cutOffJump;
    [SerializeField] float jumpBuffer;
    public float currJumpBuffer;

    public bool bufferingJump;

    private  float coyoteTimeCounter = 0f;
    public float coyoteTime;

    #endregion

    #region PROPERTIES

    float JumpForce
    {
        get => jumpForce.Value;
    }
    float FallMultiplier
    {
        get => 1;
    }
    float LowJumpMultiplier
    {
        get => lowJumpMultiplier.Value;
    }
    [SerializeField] float cutJumpPercentage = 0.5f;
    float CutJumpPercentage
    {
        get => cutJumpPercentage;
    }

    float JumpBuffer
    {
        get => jumpBuffer;
    }

    #endregion

    #region METHODS
    public void JumpFixedUpdate(IInput input)
    {
        if (cutOffJump)
        {
            return;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && input.JumpHold)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        
        

    }

    public void JumpUpdate(IInput input, bool isGrounded)
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        currJumpBuffer = currJumpBuffer - Time.deltaTime;
        if (input.Jump)
        {
            currJumpBuffer = JumpBuffer;
        }

        if (input.JumpUp)
        {
            coyoteTimeCounter = 0f;
        }

        if ((currJumpBuffer > 0) && (coyoteTimeCounter > 0))
        {
            coyoteTimeCounter = 0f;
            currJumpBuffer = 0;
            events.onJump.Invoke();
            Vector3 jumpForceVector = Vector3.up * JumpForce;
            Vector3 newVector = rb.velocity;
            newVector.y = JumpForce;

            rb.velocity = newVector;
        }
        if (cutOffJump)
        {
            if (rb.velocity.y > 0 && input.JumpUp)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * CutJumpPercentage, rb.velocity.z);
            }
        }


        

    }


    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerManager>();
        events = GetComponent<PlayerEventHandler>();
    }


    #endregion
}
