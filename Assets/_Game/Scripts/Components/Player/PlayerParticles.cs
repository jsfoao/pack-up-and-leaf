using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using ScriptableEvents;

public class PlayerParticles : MonoBehaviour
{
    PlayerEventHandler events;

    //Somehow I didnt notice that these are called 'SFX' and not 'VFX'. I'm dumb
    [SerializeField] ParticleSystem landing_SFX;
    [SerializeField] ParticleSystem sprint_SFX;
    [SerializeField] ParticleSystem skid_SFX;
    [SerializeField] ParticleSystem leaf_SFX;
    [SerializeField] ParticleSystem jump_SFX;
    [SerializeField] ParticleSystem rollFast_SFX;
    [SerializeField] float rollFastTreshold = 10f;
    [SerializeField] ParticleSystem morph_VFX;
    bool fastRolling;


    [Header("EVENT REFERENCES:")]
    [SerializeField] ScriptableEventVoid onLeafPickup;




    
    public void PlayLandingParticle()
    {
        landing_SFX.Play();
    }

    public void PlayLeafPickupParticle()
    {
        leaf_SFX.Play();
    }

    public void PlayJumpParticle()
    {
        jump_SFX.Play();
    }

    public void PlayMorphParticle()
    {
        morph_VFX.Play();
    }

    public void EnableSprintParticle(bool trueOrFalse)
    {
        if (trueOrFalse)
        {
            sprint_SFX.Play();
        }
        else
        {
            sprint_SFX.Stop();
        }
    }

    public void EnableSkidParticle(bool trueOrFalse)
    {
        if (trueOrFalse)
        {
            skid_SFX.Play();
        }
        else
        {
            skid_SFX.Stop();
        }
    }

    private void EnableRollFastParticle(Vector3 vel, bool isGrounded)
    {
        if (vel.magnitude > rollFastTreshold && isGrounded)
        {
            if (!fastRolling)
            {
                rollFast_SFX.Play();
            }
            fastRolling = true;
            
        }
        else
        {
            if (fastRolling)
            {
                rollFast_SFX.Stop();
            }
            fastRolling = false;
            
        }
        
    }

    public void DisableRollFastParticle()
    {
        fastRolling = false;
        rollFast_SFX.Stop();

    }
    //Not exactly proud of what's going on here but it works.
    public void RedirectRollFastParticle(Vector3 colPoint, Vector3 vel)
    {
        
        rollFast_SFX.transform.position = colPoint;
        if (vel == Vector3.zero)
        {
            vel = new Vector3(0, 0, 1);
            rollFast_SFX.Stop();
        }
        Quaternion rot = Quaternion.LookRotation(vel.normalized);
        rollFast_SFX.transform.rotation = rot;

        
    }

    private void Awake()
    {
        events = GetComponent<PlayerEventHandler>();
    }

    private void OnEnable()
    {
        onLeafPickup.Register(PlayLeafPickupParticle);

        events.onRollRedirect += RedirectRollFastParticle;
        events.onRollFast += EnableRollFastParticle;
        
    }

    private void OnDisable()
    {
        onLeafPickup.Unregister(PlayLeafPickupParticle);

        events.onRollRedirect -= RedirectRollFastParticle;
        events.onRollFast -= EnableRollFastParticle;

    }
}
