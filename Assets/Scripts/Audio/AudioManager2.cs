using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager2 : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioMixer masterMixer;

    public static AudioManager audioManager;

    private void Start() 
    {
       // if(PlayerPrefs.HasKey("MasterVolume"))
       // {
       //     masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
       // }

        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            masterMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }

        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            masterMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }
}
}