using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioClip clickButtonClip;
    [SerializeField] AudioClip winGameClip;

    AudioSource audioSource;
    public static SoundController Instance;
    [SerializeField] public AudioMixer masterMixer;
    [SerializeField] public AudioMixer musicMasterMixer;

    public float SFXVolume = 100;
    public float musicVolume = 100;

    public float defaultVolumeMusic = 50;
    public float defaultVolumeSFX = 50;


    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        audioSource = GetComponent<AudioSource>();

        SFXVolume = PlayerPrefs.GetFloat("SavedMasterVolume") <= 0 ? defaultVolumeSFX : PlayerPrefs.GetFloat("SavedMasterVolume");
        //masterMixer.SetFloat("MasterVolume", Mathf.Log10(SFXVolume/100) * 20f);

        musicVolume = PlayerPrefs.GetFloat("SavedMasterVolumeMusic") <= 0 ? defaultVolumeMusic : PlayerPrefs.GetFloat("SavedMasterVolumeMusic");
        //musicMasterMixer.SetFloat("MasterVolume1", Mathf.Log10(musicVolume/100) * 20f);
    }

    void Update()
    {
        SFXVolume = PlayerPrefs.GetFloat("SavedMasterVolume") <= 0 ? defaultVolumeSFX : PlayerPrefs.GetFloat("SavedMasterVolume");
        musicVolume =PlayerPrefs.GetFloat("SavedMasterVolumeMusic") <= 0 ? defaultVolumeMusic : PlayerPrefs.GetFloat("SavedMasterVolumeMusic");
    }
    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(clickButtonClip);
    }

     public void PLayWinGameSound()
    {
        audioSource.PlayOneShot(winGameClip);
    }
    
}
