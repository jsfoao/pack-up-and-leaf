using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class GlobalAudioManager : MonoBehaviour
{
    private static GlobalAudioManager instance;

    public Sound[] soundTracks;
    [SerializeField] float soundDuration;

    private void Awake()
    {
        Configuration();
    }

    private void Configuration()
    {
        if (!instance)
        {
            instance = this;
            foreach (Sound sound in soundTracks)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
                sound.source.volume = sound.volumn;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.ignoreListenerPause = sound.ignorePause;
            }
        }
        ////else
        ////{
        ////    Destroy(this.gameObject);
        ////    return;
        ////}
        //DontDestroyOnLoad(this.gameObject);
    }

    public void playSoundTrack(string soundTrack)  //play the first sound clip in sound clip list.
    {
        Sound sound = Array.Find(soundTracks, sound => sound.soundTrack == soundTrack);
        if(sound == null) { return; }
        sound.source.clip = sound.clipList[0];
        sound.source.Play();        
    }

    public void playRandomSoundTrack(string soundTrack) //play random sound clip in sound clip list with randomized pitch and volumn 
    {
        Sound sound = Array.Find(soundTracks, sound => sound.soundTrack == soundTrack);
        if (sound == null) { return; }
        int clipRandomIndex = UnityEngine.Random.Range(0, sound.clipList.Length - 1);
        sound.source.clip = sound.clipList[clipRandomIndex];
        if (sound.source.isPlaying) { return; }
        //RandomizeSound(sound);
        sound.source.PlayOneShot(sound.source.clip);
    }

    private static void RandomizeSound(Sound sound)
    {
        sound.source.pitch += UnityEngine.Random.Range(-sound.pitchChangeMultiplier, sound.pitchChangeMultiplier);
        sound.source.volume += UnityEngine.Random.Range(-sound.volumnChangeMultiplier, sound.volumnChangeMultiplier);
    }

    public void StopSoundTrack(string soundTrack)
    {
        Sound sound = Array.Find(soundTracks, sound => sound.soundTrack == soundTrack);
        if (sound == null) { return; }
        sound.source.Stop();
    }

    public void LoopSoundTrack(string soundTrack)
    {
        Sound sound = Array.Find(soundTracks, sound => sound.soundTrack == soundTrack);
        if (sound == null) { return; }
        if (sound.source.isPlaying) { return; }
        sound.source.Play();
    }

    public bool IfSoundIsPlaying(string soundTrack)
    {
        Sound sound = Array.Find(soundTracks, sound => sound.soundTrack == soundTrack);
        if (sound == null) { return false; }
        return sound.source.isPlaying;
    }
}
