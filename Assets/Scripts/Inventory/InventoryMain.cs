using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;
using static playerMovement;

public class InventoryMain : MonoBehaviour, IDataPersistence
{
    public List<Image> weaponSlotUI = new List<Image>();
    public List<Image> itemSlotUI = new List<Image>();
    public List<GameObject> weaponSlotHighlight = new List<GameObject>();

    public GameObject inventoryPanel;
    public Image equippedWeaponUI;
    public Image equippedItemIcon;

    public List<GameObject> weaponsInScene = new List<GameObject>();
    public List<GameObject> itemsInScene = new List<GameObject>();

    public List<ItemData> weaponSlots = new List<ItemData>(3); // Only 3 weapons max
    public List<ItemData> itemSlots = new List<ItemData>(4);   // Only 4 items max

    public Sprite emptySlotSprite;
    public Sprite emptyEquippedSprite;

    private bool isInventoryOpen = false;
    private int equippedWeaponIndex = -1;

    public playerMovement player;

    public ItemData basicSword; // Starting inventory

    // Audio Manager
    AudioManager audioManager;

    // Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

     private void Start()
    {
        InitializeStartingInventory();
        inventoryPanel.SetActive(false);
        UpdateEquippedWeaponUI();
        //ClearHighlights();
        equippedItemIcon.sprite = null;
        equippedItemIcon.enabled = false;
    }

    public void LoadData(GameData data)
    {
        //int itemLoadIndex = 0;

        foreach (KeyValuePair<string, bool> pair in data.itemsCollected)
        {
            if (pair.Value)
            {
                foreach (GameObject weapon in weaponsInScene)
                {
                    if (weapon.GetComponent<Weapons>().GetWeaponID() == pair.Key)
                    {
                        AddWeapon(weapon.GetComponent<Weapons>().weaponData);
                    }
                }

                foreach (GameObject item in itemsInScene)
                {
                    if (item.GetComponent<Items>().GetItemID() == pair.Key)
                    {
                        AddItem(item.GetComponent<Items>().itemData);
                    }
                }
            }

            
        }
    }

    public void SaveData(ref GameData data)
    {
        //no data needed to be saved
    }

    private void Update()
    {
        // Tab or I key to toggle the inventory
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // 1, 2, 3 keys to equip weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0); // Equip weapon in slot 1
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1); // Equip weapon in slot 2
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2); // Equip weapon in slot 3
        }
    }

    private void InitializeStartingInventory()
    {
        if (basicSword != null)
        {
            AddWeapon(basicSword);
            EquipWeapon(0); 
            HighlightEquippedWeaponSlot(0);
        }
    }

    public void ToggleInventory()
    {
        audioManager.PlaySFX(audioManager.inventoryOpened);
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        if (!isInventoryOpen)
        {
            TooltipManager._instance.HideToolTip();
        }
    }

    public void AddWeapon(ItemData weapon)
    {
        if (weaponSlots.Count < 3)
        {
            TooltipManager._instance.HideToolTip();
            weaponSlots.Add(weapon);
            UpdateWeaponUI();
            Debug.Log("Weapon added to inventory: " + weapon.itemName);
        }

        else
        {
            Debug.Log("Weapon inventory is full.");
        }
    }

    public void AddItem(ItemData item)
    {
        if (itemSlots.Count < 4)
        {
            TooltipManager._instance.HideToolTip();
            itemSlots.Add(item);
            UpdateItemUI();
            Debug.Log("Item added to inventory: " + item.itemName);
        }

        else
        {
            Debug.Log("Item inventory is full.");
        }
    }

    public void EquipWeapon(int slotIndex)
    {
        if (slotIndex == 0 || (slotIndex >= 0 && slotIndex < weaponSlots.Count && weaponSlots[slotIndex] != null))
        {
            audioManager.PlaySFX(audioManager.weaponEquipped);
            equippedWeaponIndex = slotIndex;
            player.modAttackStats(weaponSlots[slotIndex].wepDmgMod, weaponSlots[slotIndex].wepSpdMod);
            UpdateEquippedWeaponUI();
            HighlightEquippedWeaponSlot(slotIndex);
            UpdateWeaponUI();
            Debug.Log("Equipped weapon: " + weaponSlots[slotIndex].itemName);
        }

        else
        {
            //equippedWeaponIndex = -1;
            //UpdateEquippedWeaponUI();
            Debug.Log("No weapon equipped.");
        }

        UpdateEquippedWeaponUI();
        UpdateWeaponUI();
    }

    void UpdateEquippedWeaponUI()
    {
        if (equippedWeaponIndex >= 0 && equippedWeaponIndex < weaponSlots.Count)
        {
            equippedWeaponUI.sprite = weaponSlots[equippedWeaponIndex].icon;
        }

        else
        {
            equippedWeaponUI.sprite = emptyEquippedSprite;
        }
    }

    void UpdateWeaponUI()
    {
        ClearHighlights();

        for (int i = 0; i < weaponSlotUI.Count; i++)
        {
            if (i < weaponSlots.Count)
            {
                weaponSlotUI[i].sprite = weaponSlots[i].icon;
                Tooltips tooltipComponent = weaponSlotUI[i].GetComponent<Tooltips>();

                if (tooltipComponent == null)
                {
                    tooltipComponent = weaponSlotUI[i].gameObject.AddComponent<Tooltips>();
                }

                if (i == equippedWeaponIndex && weaponSlotHighlight[i] != null)
                {
                    weaponSlotHighlight[i].SetActive(true);
                }

                tooltipComponent.message = weaponSlots[i].description;
                tooltipComponent.names = weaponSlots[i].itemName;
            }

            else
            {
                weaponSlotUI[i].sprite = emptySlotSprite;
                Tooltips tooltipComponent = weaponSlotUI[i].GetComponent<Tooltips>();

                if (tooltipComponent != null)
                {
                    tooltipComponent.message = string.Empty;
                    tooltipComponent.names = string.Empty;
                }
            }
        }
    }

    public void HighlightEquippedWeaponSlot(int slotIndex)
    {
        ClearHighlights();

        if (slotIndex >= 0 && slotIndex < weaponSlotHighlight.Count)
        {
            weaponSlotHighlight[slotIndex].SetActive(true);
        }
    }

    void ClearHighlights()
    {
        foreach (var highlight in weaponSlotHighlight)
        {
            if (highlight != null)
            {
                highlight.SetActive(false);
            }
        }
    }

    void UpdateItemUI()
    {
        for (int i = 0; i < itemSlotUI.Count; i++)
        {

            if (i < itemSlots.Count)
            {
                itemSlotUI[i].sprite = itemSlots[i].icon;
                itemSlotUI[i].enabled = true;

                Tooltips tooltipComponent = itemSlotUI[i].GetComponent<Tooltips>();
                if (tooltipComponent == null)
                {
                    tooltipComponent = itemSlotUI[i].gameObject.AddComponent<Tooltips>();
                }

                tooltipComponent.message = itemSlots[i].description;
                tooltipComponent.names = itemSlots[i].itemName;

                EquipSlot slot = itemSlotUI[i].GetComponent<EquipSlot>();
                if (slot == null)
                {
                    slot = itemSlotUI[i].gameObject.AddComponent<EquipSlot>();
                }

                slot.SetItem(itemSlots[i]);
                slot.SetPlayer(player);
            }

            else
            {
                itemSlotUI[i].sprite = null;
                itemSlotUI[i].enabled = false;
                itemSlotUI[i].sprite = emptySlotSprite;

                Tooltips tooltipComponent = itemSlotUI[i].GetComponent<Tooltips>();
                if (tooltipComponent != null)
                {
                    tooltipComponent.message = string.Empty;
                    tooltipComponent.names = string.Empty;
                }
            }
        }
    }

    public void EquipWeaponInUI(ItemData equippedWeapon)
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (weaponSlots[i] == equippedWeapon)
            {
                weaponSlotHighlight[i].SetActive(true);
            }

            else
            {
                weaponSlotHighlight[i].SetActive(false);
            }
        }
    }

    public void UpdateEquippedWeaponUI(ItemData equippedWeapon)
    {
        if (equippedWeapon != null)
        {
            equippedWeaponUI.sprite = equippedWeapon.icon;
            equippedWeaponUI.enabled = true;
        }

        else
        {
            equippedWeaponUI.sprite = emptyEquippedSprite;
            equippedWeaponUI.enabled = true;
        }
    }

    public void EquipWeaponFromSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < weaponSlots.Count)
        {
            if (slotIndex == equippedWeaponIndex)
            {
                if (slotIndex != 0)
                {
                    UnequipWeapon();
                }   
            }

            else
            {
                EquipWeapon(slotIndex);
            }
        }
    }

    private void UnequipWeapon()
    {
        equippedWeaponIndex = 0;
        UpdateEquippedWeaponUI();
        UpdateWeaponUI();
        player.modAttackStats(weaponSlots[0].wepDmgMod, weaponSlots[0].wepSpdMod);
        Debug.Log("Weapon unequipped; reverted to basic sword.");
    }
}