using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    //This is for the inventory slots to be able to equip items
    public bool isWeaponSlot;
    public int slotIndex;
    public ItemData itemData;
    private playerMovement player;
    private InventoryMain inventoryMain;

    private void Start()
    {
        inventoryMain = FindObjectOfType<InventoryMain>();
    }

    public void SetPlayer(playerMovement playerReference)
    {
        player = playerReference;
    }

    public void SetItem(ItemData newItemData)
    {
        itemData = newItemData;
        GetComponent<Image>().sprite = itemData?.icon;
        GetComponent<Image>().enabled = (itemData != null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData != null)
        {
           if (player.IsItemEquipped(itemData))
            {
                UnequipItem();
            }

            else
            {
                EquipItem();
            }
        }

        if (inventoryMain != null)
        {
            if (isWeaponSlot)
            {
                inventoryMain.EquipWeaponFromSlot(slotIndex);
            }
        }
    }

    private void EquipItem()
    {
        if (player != null)
        {
            player.EquipItem(itemData);
            Debug.Log("Equipped: " + itemData.itemName);
        }
    }

    private void UnequipItem()
    {
        if (player != null)
        {
            player.UnequipItem();
            Debug.Log("Unequipped: " + itemData.itemName);
        }
    }
}
