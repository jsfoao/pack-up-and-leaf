using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAudioManager : MonoBehaviour
{
    public Hashtable localAudioHash;
    public GameObject[] localAudioObjects;
    public  float volumeAtPoint = 0.5f;

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
        AudioSource.PlayClipAtPoint(thisAudioSource.clip, transform.position,volumeAtPoint);
    }
}
