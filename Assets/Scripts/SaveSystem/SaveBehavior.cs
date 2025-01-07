using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SaveBehavior : MonoBehaviour
{

    public GameObject player;
    public GameObject totem;

    private Animator totemAnimator;

    [Header("GUI")]
    public bool ShowGUI = false;
    public GameObject saveButton;

    //Audio Manager
    AudioManager audioManager;

    //Gets the relavent PlayerPrefs for location and assings it to variables
    private void Awake()
    {
        saveButton.gameObject.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        totemAnimator = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShowGUI && Input.GetKeyDown(KeyCode.E))
        {
            audioManager.PlaySFX(audioManager.gameSaved);
            totemAnimator.Play("SaveTotemSparkle");
            Debug.Log("Input Detected.");
            player.GetComponent<playerMovement>().healthPoints = player.GetComponent<playerMovement>().maxHealth;
            DataPersistenceManager.instance.SaveGame();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ShowGUI = true;
            saveButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ShowGUI = false;
            saveButton.gameObject.SetActive(false);
        }
    }

}
