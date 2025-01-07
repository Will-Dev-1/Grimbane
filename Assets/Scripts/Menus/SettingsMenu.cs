using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
   //For vsync and resolution settings
    public Toggle fullscreenTog, vsyncTog;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;
    public Text resolutionLabel;
    //public TMP_Text resolutionLabel2;

    //For audio settings
    //public AudioMixer masterMixer;
    //public Slider musicSlider, sfxSlider;
    //public TMP_Text musicLabel, sfxLabel;

    // masterSlider masterLabel

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                UpdateResLabel();
            }
        }

        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;
            UpdateResLabel();
        }

        //float vol = 0f;

       // masterMixer.GetFloat("MasterVolume", out vol);
       // masterSlider.value = vol;

        //masterMixer.GetFloat("MusicVolume", out vol);
        //musicSlider.value = vol;

        //masterMixer.GetFloat("SFXVolume", out vol);
        //sfxSlider.value = vol;

       // masterLabel.text = Mathf.RoundToInt(masterSlider.value + 90).ToString();
        //musicLabel.text = Mathf.RoundToInt(musicSlider.value + 90).ToString();
        //sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 90).ToString();
    }

    void Update()
    {

    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        //Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    //public void SetMasterVolume()
    //{
    //    masterLabel.text = Mathf.RoundToInt(masterSlider.value + 90).ToString();
    //    masterMixer.SetFloat("MasterVolume", masterSlider.value);
    //    PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    //}

    //public void SetMusicVolume()
    //{
    //    musicLabel.text = Mathf.RoundToInt(musicSlider.value + 90).ToString();
    //    masterMixer.SetFloat("MusicVolume", musicSlider.value);
     //   PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    //}

    //public void SetSFXVolume()
    //{
    //    sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 90).ToString();
     //   masterMixer.SetFloat("SFXVolume", sfxSlider.value);
     //   PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    //}

}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
