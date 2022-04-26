using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [Header("UI sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;


    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float sliderMultiplier;

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(ChangeMasterSlider);
        musicSlider.onValueChanged.AddListener(ChangeMusicSlider);
        soundSlider.onValueChanged.AddListener(ChangeSoundSlider);
    }

    private static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {

    }

    public void ChangeMasterSlider(float value)
    {

        audioMixer.SetFloat("masterVolume", CalculateVolumeValue(value));
    }

    public void ChangeMusicSlider(float value)
    {
        audioMixer.SetFloat("musicVolume", CalculateVolumeValue(value));
    }

    public void ChangeSoundSlider(float value)
    {
        audioMixer.SetFloat("soundVolume", CalculateVolumeValue(value));
    }

    private float CalculateVolumeValue(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value,0.001f,Mathf.Infinity)) * sliderMultiplier;
    }
}
