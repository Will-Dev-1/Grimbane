using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;

    public AudioSource PauseSound;
    public AudioSource UnPauseSound;
    public AudioSource OptionSelectedSound;

    void Start() 
    {
        
        Time.timeScale = 1f;
        
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(Paused) 
            {
                Play();
            }
            else 
            {
                Stop();
            }
        }

    }

     public void PlaySelectSound()
    {
        OptionSelectedSound.Play();
    }

    void Stop() 
    {
        PauseSound.Play();
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Play() 
    {
        UnPauseSound.Play();
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void MainMenuButton() 
    {
        SceneManager.LoadScene(0);
    }

}
