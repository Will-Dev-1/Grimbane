using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button loadGameButton;

    public void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
    }


    public void StartButton()
   {
        //Scene unload = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
        //SceneManager.UnloadSceneAsync(unload);
   }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void NewGameButton()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Area 1");
    }

    public void LoadGameButton()
    {
        //DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync("Area 1");
    }

    //public void RestartButton()
    //{
    //    SceneManager.LoadScene(1);
    //}

    //public void MenuButton()
    //{
    //    SceneManager.LoadScene(0);
    //}
}
