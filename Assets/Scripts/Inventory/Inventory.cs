using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Inventory : MonoBehaviour
{
    public static event Action<List<InventoryItem>> OnInventoryChange;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    //Audio Manager
    AudioManager audioManager;

    // Reference to the player script
    public playerMovement playerScript;

    // Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        Item.OnItemCollected += Add;
    }

    private void OnDisable()
    {
        Item.OnItemCollected -= Add;
    }
   
    public void Add(ItemData itemData)
    {
        if (inventory.Count < 6)
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            OnInventoryChange?.Invoke(inventory);
        }
    }

   public void Remove(ItemData itemData, InventoryItem item)
   {
        inventory.Remove(item);
        OnInventoryChange?.Invoke(inventory);
   }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            inventory.RemoveAt(index);
            OnInventoryChange?.Invoke(inventory);

            // Increase player's health by 10 when removing an item
            playerScript.IncreaseHealth(10);
        }
    }

    //Health Potion
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (inventory.Count > 0)
            {
                RemoveAt(0);
                audioManager.PlaySFX(audioManager.potionUsed);
            }
        }
    }
}