using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using ScriptableEvents;
using System;

public class TestAudio : MonoBehaviour
{
    private GlobalAudioManager audioManager;
    private bool playFootsteps = false;
    private bool ballState = false;
    private bool onGround = true;

    [Header("AUDIO CONFIGURATION")]
    [SerializeField] GameObject player;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerSnapshot unpauseSnap;
    
    //[SerializeField] float rvCurveMidway=0.8f;
    //[SerializeField] float rvMaxTreshold = 21f;
    //[SerializeField] float stepnessVolumeCurve = 0.02f;
    //[SerializeField] float rvMultiplier = 30f;

    [Header("ROLLING SOUND PROPERTIES:")]
    [SerializeField] AnimationCurve rollingSoundVolumeCurve;
    [SerializeField] float maxRollingSpeedCap;
    [SerializeField] AnimationCurve rollingSoundPitchCurve;
    [SerializeField] float maxRollingPitchSpeedCap;


    private void Awake()
    {
        audioManager = FindObjectOfType<GlobalAudioManager>().GetComponent<GlobalAudioManager>();
    }

    void Start()
    {
        audioManager.playSoundTrack("Ambience");
        audioManager.playSoundTrack("Music");
        AudioConfiguration();
    }
    private void Update()
    {
        if (Time.timeScale == 0) { return; }
        if(playFootsteps)
        {
            audioManager.playRandomSoundTrack("Footsteps");
        }
        UpdateRollingVolume();
    }
    
    public void AudioConfiguration()
    {
        audioManager.StopSoundTrack("MainMenu");
        unpauseSnap.TransitionTo(0.01f);
        //Addlistener to event system
        player = GameObject.FindGameObjectWithTag("Player"); //Protip: Since this is on the same object as PlayerEventHandler, you can just do GetComponent<PlayerEventHandler> with no 'player.' :)
        PlayerEventHandler playerEventHandler = player.GetComponent<PlayerEventHandler>();
        playerEventHandler.onGroundStartWalk.AddListener(PlayFootstepsSFX);
        playerEventHandler.onGroundStartSkid.AddListener(PlaySkidSFX);
        playerEventHandler.onGroundStateChange.AddListener(PlayerOnGround);
        playerEventHandler.onBallEnter.AddListener(EnterBallSFX);
        playerEventHandler.onWalkEnter.AddListener(ExitBallSFX);
        playerEventHandler.onGroundStateChange.AddListener(PlayerOnGround);
        playerEventHandler.onLand.AddListener(PlayLandingSFX);
        playerEventHandler.onJump.AddListener(PlayJumpSFX);
        //on damage event
        PlayerEntity playerEntity = player.GetComponent<PlayerEntity>();
        playerEntity.onDamaged.AddListener(PlayerDamagedSFX);
        playerEntity.onFallWater.AddListener(PlayDropWtaerSFX);
    }

    public void PlayJumpSFX()
    {
        audioManager.playRandomSoundTrack("Jump");
    }

    public void PlayLandingSFX()
    {
        audioManager.playRandomSoundTrack("Land");
    }

    public void PlaySkidSFX(bool trueOrFalse)
    {
        if(trueOrFalse)
        {
            audioManager.playRandomSoundTrack("Skid");
        }
        else
        {
            return;
        }
    }

    public void PlayFootstepsSFX(bool trueOrFalse)
    {
        if (trueOrFalse)
        {
            playFootsteps = true;
        }
        else
        {
            playFootsteps = false;
        }
    }

    public void PlayLeafPickupSFX()
    {
        audioManager.playSoundTrack("LeafPickup");
    }

    public void PlayerDamagedSFX()
    {
        audioManager.playRandomSoundTrack("Damaged");
    }

    private void UpdateRollingVolume()
    {
        float ballVelocity = player.GetComponent<Rigidbody>().velocity.magnitude;

        //float rvPercentage = ballVelocity / rvMaxTreshold;
        //float rvCurve = 1f / Mathf.Pow((1f + Mathf.Exp(1)), (-stepnessVolumeCurve * (rvPercentage - rvCurveMidway)));
        //1f / Mathf.Pow((1f + Mathf.Exp(1)), (-stepnessVolumeCurve * (ballVelocity - rvLowTreshold)));//S shape logictic curve between ballVelocity and rollingVolume
        //float rollingVolume = Mathf.Clamp( Mathf.Log10(Mathf.Clamp(rvCurve, 0.001f, Mathf.Infinity)) * rvMultiplier , -60f,0);

        //Handle percentage from velocity
        float volumePercentage = ballVelocity / maxRollingSpeedCap;
        volumePercentage = Mathf.Clamp01(volumePercentage);

        float pitchPercentage = ballVelocity / maxRollingPitchSpeedCap;
        pitchPercentage = Mathf.Clamp01(pitchPercentage);
        
        float rollingVolume = rollingSoundVolumeCurve.Evaluate(volumePercentage);
        float rollingPitch = rollingSoundPitchCurve.Evaluate(pitchPercentage);

        
        audioMixer.SetFloat("rollingVolume", rollingVolume);
        audioMixer.SetFloat("rollingPitch", rollingPitch);
    }

    public void EnterBallSFX()
    {
        audioManager.playSoundTrack("Switch");
        audioManager.playSoundTrack("Rolling_highSpeed");
        ballState = true;
    }

    public void ExitBallSFX()
    {
        audioManager.playSoundTrack("Switch");
        audioManager.StopSoundTrack("Rolling_highSpeed");
        audioManager.StopSoundTrack("Rolling_Ground");
        ballState = false;
    }
    public void PlayerOnGround(bool trueOrFalse)
    {
        if(trueOrFalse)
        {
            onGround = true;
            if(ballState)
            {
                audioManager.playSoundTrack("Rolling_Ground");
            }
        }
        else
        {
            onGround = false;
            audioManager.StopSoundTrack("Rolling_Ground"); 
        }
    }
    public void PlayDropWtaerSFX()
    {
        Debug.Log("AAAAAAA");
        audioManager.playSoundTrack("DropWater");
    }


}
