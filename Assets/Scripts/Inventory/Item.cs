using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Health Potion Script ONLY
public class Item : MonoBehaviour, ICollectible
{
    public static event HandleItemCollected OnItemCollected;
    public delegate void HandleItemCollected(ItemData itemData);
    public ItemData potionData;

    //Audio Manager
    AudioManager audioManager;

    // Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Collect(Collector collector)
    {
        audioManager.PlaySFX(audioManager.potionCollect);
        //Debug.Log("Potion Collected.");
        Destroy(gameObject);
        OnItemCollected?.Invoke(potionData);
    }
}
