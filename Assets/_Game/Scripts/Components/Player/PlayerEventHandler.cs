using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using ScriptableEvents;

public class PlayerEventHandler : MonoBehaviour
{
    ///This component handles all things centralized player events, so other components can subscribe to this
    
    public UnityEvent<bool> onGroundStartWalk; //When the player starts inputting movement while on the ground, is called in PlayerLocomotion
    public UnityEvent<bool> onGroundStartSkid; //When the player starts or stops skidding, is called in PlayerLocomotion
    public UnityEvent<bool> onGroundStateChange; //Wether or not you went grounded or undergrounded, is called in RigidbodyFloater and PlayerRolling
    public UnityEvent<bool> onRisingOrFalling; //Wether or not we're rising or falling, is called in PlayerManager
    public UnityEvent<bool> onEnterWalkState; //When you switch from any state to walking. Is called in PlayerManager

    public UnityEvent onStartWalking; //When you start walking, called in PlayerLocomotion
    public UnityEvent onStartSkidding; //When you skid, called in PlayerLocomotion
    public UnityEvent onLand; //When you land. On the ground, just like onGroundStateChange but just a bool. Is called in RigidbodyFloater
    public UnityEvent onJump; //Whenever you jump. Is called in PlayerJump
    public UnityEvent onBallEnter; //Whenever you start ball state. Is called in PlayerManager
    public UnityEvent onWalkEnter; //Whenever you start walk state. Is called in PlayerManager

    public Action<Vector3, Vector3> onRollRedirect; //The position where you collide when you're grounded while rolling and the velocity. Is called in PlayerRolling
    public Action<Vector3, bool> onRollFast; //The velocity and wether or not you're grounded. Is called in PlayerRolling, in update, not sure if that's good or not...

    //public ScriptableEventVoid onDeath; //When the player dies, is called in IEntity
}
