using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public GlobalAudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<GlobalAudioManager>();
    }
    public void PlayUIHoverSFX()
    {
        audioManager.playRandomSoundTrack("UIHover");
    }

    public void PlayOnClickSFX()
    {
        audioManager.playRandomSoundTrack("OnClick");
    }
}
