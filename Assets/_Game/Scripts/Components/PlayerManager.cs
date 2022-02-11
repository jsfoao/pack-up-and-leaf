using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    #region References
    PlayerLocomotion locomotion;
    PlayerRolling rolling;
    RigidbodyFloater floater;
    Rigidbody rb;
    PlayerEventHandler events;
    IEntity entity;

    [SerializeField] PlayerJump walkJump;
    [SerializeField] PlayerJump rollJump;
    [SerializeField] LayerMask groundLayer;
    
    public UnityEvent OnLeafPickUp;
    public UnityEvent OnHealthPickUp;

    IInput _input;
    IInput Inputs
    {
        get => _input;
        set { _input = value; }
    }

    UnityEvent<bool> OnGroundWalkStart
    {
        get => events.onGroundStartWalk;
    }

    [SerializeField] CapsuleCollider characterCollider;
    [SerializeField] float unRollCollisionCheckRadiusOffset = 0.3f;
    [SerializeField] float unRollCollisionCheckHeightOffset = 0.2f;
    [SerializeField] MeshRenderer characterMesh;
    [SerializeField] Transform characterTransform; //This is only the transform of the capsule
    #endregion

    public enum State
    {
        Normal,
        Rolling,
    }
    public State state = new State();

    //Flags
    public bool isGrounded;


    #region Methods
    
    public void OnLeafCollision()
    {
        OnLeafPickUp.Invoke();
    }

    public void OnHealthCollision()
    {
        OnHealthPickUp.Invoke();
    }
    public void StartRoll() //Disables the character mesh and collider and replaces it with the rolling one
    {
        characterTransform.gameObject.SetActive(false);
        characterCollider.enabled = false;
        
        rolling.StartRoll();
        rolling.ballObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        isGrounded = false;
        OnGroundWalkStart.Invoke(false);

        //floater.isFloating = false;
    }
    public void StopRoll() //Reenables the character mesh and collider from the rolling one
    {
        

        characterTransform.gameObject.SetActive(true);
        characterCollider.enabled = true;
        rolling.StopRoll();
        OnGroundWalkStart.Invoke(true);
        //floater.isFloating = true;
    }

    public void JumpUpdate()
    {
        walkJump.JumpUpdate(Inputs, isGrounded);
    }



    public void TryRollingJump()
    {
        if (Inputs.Jump)
        {
            //rolling.RollingJump(Inputs.Jump, isGrounded);
        }
        rollJump.JumpUpdate(Inputs, isGrounded);
    }

    public void TryRolling()
    {
        
        if (Inputs.StartRoll)
        {
            SetState(State.Rolling);
        }
    }

    

    #region State Methods
    //Here be methods that handle the update loops for respective states
    //
    public void SetState(State _state)
    {
        switch (_state)
        {
            case State.Normal:
                {
                    state = _state;
                    State_Normal_Enter();
                    break;
                }
            case State.Rolling:
                {
                    state = _state;
                    State_Rolling_Enter();
                    break;
                }
        }
    }

    void State_Normal_Enter()
    {
        StopRoll();
        events.onWalkEnter.Invoke();

    }

    void State_Normal_Update()
    {
        JumpUpdate();
        locomotion.RotatePlayer(Inputs);
        TryRolling();
    }

    void State_Normal_FixedUpdate()
    {
        floater.FloatOnGround(out isGrounded); //Evaluate wether or not I should move the groundCheck to Update instead of FixedUpdate
        locomotion.ApplyMovement(Inputs, isGrounded);
        walkJump.JumpFixedUpdate(Inputs);
        
    }

    void State_Rolling_Enter()
    {
        StartRoll();
        events.onBallEnter.Invoke();
    }

    void State_Rolling_Update()
    {
        TryRollingJump();
        //JumpUpdate();

        
        if (Inputs.StartRoll)
        {
            //Check if unrolling will cause a collision
            Ray ray = new Ray();
            ray.direction = Vector3.up;
            ray.origin = characterCollider.bounds.center;
            Debug.Log(ray.origin);
            if (Physics.SphereCast(ray, characterCollider.radius - unRollCollisionCheckRadiusOffset, characterCollider.height - characterCollider.radius * 2 - unRollCollisionCheckHeightOffset, groundLayer))
            {
                Debug.Log("cant transform here");
                return;
            }
            SetState(State.Normal);
        }
    }

    void State_Rolling_FixedUpdate()
    {
        rolling.ApplyRollingMovement(Inputs.InputDirection);
        rollJump.JumpFixedUpdate(Inputs);
    }
    #endregion

    #endregion

    #region Unity Methods
    private void Awake()
    {
        locomotion = GetComponent<PlayerLocomotion>();
        floater = GetComponent<RigidbodyFloater>();
        rolling = GetComponent<PlayerRolling>();
        //walkJump = GetComponent<PlayerJump>();
        events = GetComponent<PlayerEventHandler>();
        rb = GetComponent<Rigidbody>();
        entity = GetComponent<IEntity>();
        _input = GetComponent<IInput>();
    }
    private void Update()
    {
        if (rb.velocity.y > 0)
        {
            events.onRisingOrFalling.Invoke(true);
        }
        else
        {
            events.onRisingOrFalling.Invoke(false);
        }

        switch(state)
        {
            case State.Normal:
            {
                State_Normal_Update();
                break;
            }
            case State.Rolling:
            {
                State_Rolling_Update();
                break;
            }
        }


    }



    private void FixedUpdate()
    {
        switch(state)
        {
            case State.Normal:
                {
                    State_Normal_FixedUpdate();
                    break;
                }
            case State.Rolling:
                {
                    State_Rolling_FixedUpdate();
                    break;
                }
        }
    }

    public void stat()
    {
        Debug.Log("Die, turn ball");
        SetState(State.Normal);
    }

    private void OnEnable()
    {
        
        entity.onRespawn.AddListener(stat);
        
    }
    private void OnDisable()
    {
        void stat()
        {
            SetState(State.Normal);
        }
        entity.onRespawn.RemoveListener(stat);
    }

    #endregion
}
