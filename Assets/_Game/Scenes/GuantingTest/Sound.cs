using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string soundTrack;
    public AudioClip[] clipList;
    public AudioMixerGroup audioMixerGroup;
    [Range(-3f,3f)]
    public float pitch = 1f;
    [Range(0f,1f)]
    public float volumn=0.8f;
    [Range(0.01f, 0.5f)]
    public float volumnChangeMultiplier=0.2f;
    [Range(0.01f, 0.5f)]
    public float pitchChangeMultiplier = 0.2f;
    public bool loop = false;
    public bool ignorePause = false;

    [HideInInspector]
    public AudioSource source;
}
