using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Items : MonoBehaviour, ICollectible, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate Id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public ItemData itemData;
    private bool collected;
    public playerMovement player;

    //Audio Manager
    AudioManager audioManager;

    // Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
       //itemData.id = id;
    }

    private void Update()
    {
        collected = itemData.collected;
        itemData.id = id;
    }

    public void Collect(Collector collector)
    {
        audioManager.PlaySFX(audioManager.potionCollect);
        //Debug.Log("Potion Collected.");
        collector.inventory.AddItem(itemData);
        itemData.collected = true;
        collected = itemData.collected;
        EquipItem(collector.player);
        TooltipManager._instance.HideToolTip();
        Destroy(gameObject);
        //OnItemCollected?.Invoke(potionData);
    }

    private void EquipItem(playerMovement player)
    {
        player.EquipItem(itemData);
    }

    public void LoadData(GameData data)
    {
        data.itemsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.itemsCollected.ContainsKey(id))
        {
            data.itemsCollected.Remove(id);
        }
        data.itemsCollected.Add(id, collected);
    }

    public string GetItemID()
    {
        return id;
    }

    public bool GetCollectedValue()
    {
        return collected;
    }
}