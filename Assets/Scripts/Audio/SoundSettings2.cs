using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings2 : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer sfxMixer;
    
    private void Start() 
    {
        SetVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100));
    }

    public void SetVolume(float _value) 
    {
        if (_value < 1) 
        {
            _value = .001f;
        }

        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value);
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(_value / 100) * 20f);
    }
   
    public void SetVolumeFromSlider() 
    {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float _value) 
    {
        soundSlider.value = _value;
    }
    
}
