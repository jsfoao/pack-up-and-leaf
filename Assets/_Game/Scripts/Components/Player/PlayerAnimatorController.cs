using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] Animator anim;
    PlayerEventHandler events;

    private void Awake()
    {
        events = GetComponent<PlayerEventHandler>();
    }

    public void StartWalkAnim(bool startOrStop)
    {
        anim.SetBool("isRunning", startOrStop);
    }

    public void SetGroundBool(bool isGrounded)
    {
        anim.SetBool("isGrounded", isGrounded);
    }

    public void SetRisingOrFalling(bool risingOrFalling)
    {
        anim.SetBool("risingOrFalling", risingOrFalling);
    }

    public void SetState(string stateName)
    {
        anim.Play(stateName, 0, 0);
    }

    public void SetStateWithDelay(string stateName)
    {
        anim.Play(stateName, 0, -3);
    }

    void StartSkidAnimation()
    {

        anim.Play("Skid");
    }

    /*
    private void OnEnable()
    {
        events.onStartSkidding.AddListener(StartSkidAnimation);
    }

    private void OnDisable()
    {
        events.onStartSkidding.RemoveListener(StartSkidAnimation);
    }
    */
}
