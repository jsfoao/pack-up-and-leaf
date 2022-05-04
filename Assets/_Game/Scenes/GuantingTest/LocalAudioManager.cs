using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LocalAudioManager : MonoBehaviour
{
    public Hashtable localAudioHash;
    public GameObject[] localAudioObjects;
    public  float volumeAtPoint = 0.5f;

    [SerializeField] AudioMixer audioMixer;
    private float masterValueFloat;
    private float volumeValueFloat;

    private void Start()
    {
        localAudioHash = new Hashtable();
        foreach (GameObject audioObject in localAudioObjects)
        {
            localAudioHash.Add(audioObject.name, audioObject);
        }
    }

    public void PlaySFX(string name)
    {
        GameObject thisAudioObject = (GameObject)localAudioHash[name];
        AudioSource thisAudioSource = thisAudioObject.GetComponent<AudioSource>();
        thisAudioSource.Play();
    }

    public void PlaySFXAtPoint(string name)
    {
        GameObject thisAudioObject = (GameObject)localAudioHash[name];
        AudioSource thisAudioSource = thisAudioObject.GetComponent<AudioSource>();
        bool masterValueResult = audioMixer.GetFloat("masterVolume", out float masterVolume);
        if (masterValueResult)
        {
           masterValueFloat = (masterVolume+80)*5/4;
        }
        else
        {
            masterValueFloat = 0.0f;
        }
        bool volumValueResult = audioMixer.GetFloat("soundVolume", out float soundValue);
        if (volumValueResult)
        {
            volumeValueFloat = (soundValue + 80) * 5 / 4;
        }
        else
        {
            volumeValueFloat = 0.0f;
        }
        float finalVolumeProduct = volumeAtPoint * masterValueFloat * volumeValueFloat;
        Debug.Log("finalVolume"+finalVolumeProduct);
        AudioSource.PlayClipAtPoint(thisAudioSource.clip, transform.position,finalVolumeProduct);
    }
}
