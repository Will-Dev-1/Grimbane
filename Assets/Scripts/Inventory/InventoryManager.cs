using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(6);

    private void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateInventoryDisplay;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateInventoryDisplay;
    }

    private void Start()
    {
        InitializeInventoryDisplay();
    }

    void InitializeInventoryDisplay()
    {
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform, false);
            InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
            newSlotComponent.SetGrayedOutIcon();
            inventorySlots.Add(newSlotComponent);
        }
    }

    void UpdateInventoryDisplay(List<InventoryItem> inventory)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].SetGrayedOutIcon();
        }

        for (int i = 0; i < inventory.Count && i < inventorySlots.Count; i++)
        {
            inventorySlots[i].SetCollectedIcon();
        }
    }

    //void ResetInventory()
    //{
    //    foreach (Transform childTransform in transform)
    //    {
    //        Destroy(childTransform.gameObject);
    //    }
    //    inventorySlots = new List<InventorySlot>(6);
    //}

    //void DrawInventory(List<InventoryItem> inventory)
    //{
    //    ResetInventory();

    //    for (int i = 0; i < inventorySlots.Capacity; i++)
    //    {
    //        CreateInventorySlot();
    //    }

    //    for (int i = 0; i < inventory.Count; i++)
    //    {
    //        inventorySlots[i].DrawSlot(inventory[i]);
    //    }
    //}

    //void CreateInventorySlot()
    //{
    //    GameObject newSlot = Instantiate(slotPrefab);
    //    newSlot.transform.SetParent(transform, false);

    //    InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
    //    newSlotComponent.ClearSlot();

    //    inventorySlots.Add(newSlotComponent);
    //}
}