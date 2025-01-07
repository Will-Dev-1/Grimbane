using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class Weapons : MonoBehaviour, ICollectible, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate Id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public ItemData weaponData;

    private bool collected;

    //Audio Manager
    AudioManager audioManager;

    // Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
       weaponData.id = id;
        weaponData.isWeapon = true;
        weaponData.wepDmgMod = this.gameObject.GetComponent<WeaponBase>().getWepDmg();
        weaponData.wepSpdMod = this.gameObject.GetComponent<WeaponBase>().getWepSpd();
    }

    private void Update()
    {
        collected = weaponData.collected;
    }

    public void Collect(Collector collector)
    {
        audioManager.PlaySFX(audioManager.potionCollect);
        //Debug.Log("Potion Collected.");
        collector.inventory.AddWeapon(weaponData);
        weaponData.collected = true;
        collected = weaponData.collected;
        Destroy(gameObject);
        //OnItemCollected?.Invoke(potionData);
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

    public string GetWeaponID()
    {
        return id;
    }

    public bool GetCollectedValue()
    {
        return collected;
    }
        
}
