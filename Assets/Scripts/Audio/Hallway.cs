using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Hallway : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject player;

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D Coll)
    {
        while(Coll.gameObject.tag.Contains("Hallway"))
        {
            audioManager.ChangeMusic(4);
        }
    }

}
