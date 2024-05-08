using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] public Slider soundSlider;
    [SerializeField] public AudioMixer masterMixer;

    [SerializeField] public Slider musicSlider;
    [SerializeField] public AudioMixer musicMasterMixer;

    private void Awake()
    {
        
    }

    private void Start()
    {  
        //SetVolume(50);
        soundSlider.value = SoundController.Instance.SFXVolume;
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(SoundController.Instance.SFXVolume/100) * 20f);
        //SetMusic(60)
        musicSlider.value = SoundController.Instance.musicVolume;
        musicMasterMixer.SetFloat("MasterVolume1", Mathf.Log10(SoundController.Instance.musicVolume/100) * 20f);
        
    }

    public void SetVolume(float value)
    {
        if(value < 1)
        {
            value = .001f;
        }
        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedMasterVolume", value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value/100) * 20f);
    }

    public void SetMusic(float value)
    {
        if(value < 1)
        {
            value = .001f;
        }
        //RefreshMusicSlider(value);
        PlayerPrefs.SetFloat("SavedMasterVolumeMusic", value);
        musicMasterMixer.SetFloat("MasterVolume1", Mathf.Log10(value/100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void SetVolumeMusicFromSlider()
    {
        SetMusic(musicSlider.value);
    }

    public void RefreshSlider(float value)
    {
        soundSlider.value = value;
    }
    public void RefreshMusicSlider(float value)
    {
        musicSlider.value = value;
    }
}
