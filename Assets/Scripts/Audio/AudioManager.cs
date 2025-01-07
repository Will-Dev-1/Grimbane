using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioMixer masterMixer;
    //[SerializeField] Slider soundSlider;

    [SerializeField] Slider soundSlider;
    [SerializeField] Slider soundSlider2;

    [Header("Music")]
    public AudioClip startMusic;
    public AudioClip nexusMusic;
    public AudioClip gameMusic;
    public AudioClip gameMusic2;
    public AudioClip hallMusic;
    [Header("Player")]
    public AudioClip playerWalk;
    public AudioClip playerRun;
    public AudioClip playerAttack;
    public AudioClip playerHit;
    public AudioClip playerDeath;
    public AudioClip playerDash;
    public AudioClip playerJump;
    [Header("Enemy")]
    public AudioClip enemyAttack;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip spiderAttack;
    public AudioClip spiderHit;
    public AudioClip spiderDeath;
    public AudioClip bruteAttack;
    public AudioClip bruteHit;
    public AudioClip bruteDeath;
    public AudioClip vanisherAppear;
    public AudioClip vanisherAttack;
    [Header("Boss")]
    public AudioClip bossAttack;
    public AudioClip bossShoot;
    public AudioClip bossHit;
    public AudioClip bossDeath;
    public AudioClip bossVictory;
    [Header("Potion")]
    public AudioClip potionCollect;
    public AudioClip potionUsed;
    [Header("Menu")]
    public AudioClip gamePaused;
    public AudioClip gameUnpaused;
    public AudioClip buttonClicked;
    public AudioClip gameSaved;
    [Header("Inventory")]
    public AudioClip inventoryOpened;
    public AudioClip weaponEquipped;
    public AudioClip rangedAttack;
    public AudioClip itemEquipped;
    public AudioClip itemUnequipped;

    public static AudioManager audioManager;

    private void Start() 
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        }

        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            masterMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }

        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            masterMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }

        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        SetVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 100));
        SetVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100));
        musicSource.clip = startMusic;
        musicSource.Play();
        audioManager = this;
    }

    public void SetVolume(float _value) 
    {
        if (_value < 1) 
        {
            _value = .001f;
        }

        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedMusicVolume", _value);
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetVolume2(float _value2) 
    {
        if (_value2 < 1) 
        {
            _value2 = .001f;
        }

        RefreshSlider2(_value2);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value2);
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(_value2 / 100) * 20f);
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime, int musicID)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        switch(musicID)
        {
            case 0:
                audioSource.clip = startMusic;
                break;
            case 1:
                audioSource.clip = nexusMusic;
                break;
            case 2:
                audioSource.clip = gameMusic;
                break;
            case 3:
                audioSource.clip = gameMusic2;
                break;
            case 4:
                audioSource.clip = hallMusic;
                break;
            default:
                audioSource.clip = startMusic;
                break;
        }
        audioSource.Play();
    }

    public void ChangeMusic(int id)
    {
        StartCoroutine(FadeOut(musicSource, 0.3f, id));
    }

    public void SetVolumeFromSlider() 
    {
        SetVolume(soundSlider.value);
    }

    public void SetVolumeFromSlider2() 
    {
        SetVolume2(soundSlider2.value);
    }

    public void RefreshSlider(float _value) 
    {
        soundSlider.value = _value;
    }

     public void RefreshSlider2(float _value2) 
    {
        soundSlider2.value = _value2;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}

