using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public GlobalAudioManager audioManager;
    public AudioMixerSnapshot transitionSnap;
    public AudioMixerSnapshot defaultSnap;

    private void Start()
    {
        audioManager.playSoundTrack("MainMenu");
        audioManager = FindObjectOfType<GlobalAudioManager>();
        defaultSnap.TransitionTo(0.01f);
    }
    public void PlayUIHoverSFX()
    {
        audioManager.playRandomSoundTrack("UIHover");
    }

    public void PlayOnClickSFX()
    {
        audioManager.playRandomSoundTrack("OnClick");
    }
    public void FadeOutMainMenuMusic()
    {
        transitionSnap.TransitionTo(0.8f);
    }
}
