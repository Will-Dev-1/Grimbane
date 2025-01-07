using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
     public Image icon;
     private ItemData currentItem;
     private Tooltips tooltipComponent;
     public Sprite grayedOutIcon;
     public Sprite collectedIcon;
     //public TextMeshProUGUI labelText;
     //public TextMeshProUGUI stackSizeText;

     private void Awake()
     {
          tooltipComponent = GetComponent<Tooltips>();
     }

     public void SetGrayedOutIcon()
     {
          icon.enabled = true;
          icon.sprite = grayedOutIcon;
     }

    public void SetCollectedIcon()
     {
          icon.enabled = true;
          icon.sprite = collectedIcon;
     }

     public void DrawSlot(InventoryItem item)
     {
          if(item == null)
          {
               ClearSlot();
               return;
          }

          currentItem = item.itemData;
          icon.enabled = true;
          icon.sprite = item.itemData.icon;

          if (tooltipComponent != null)
          {
               tooltipComponent.message = item.itemData.description; // Set item description
               tooltipComponent.names = item.itemData.itemName;      // Set item name
          }
          //labelText.enabled = true;
          //stackSizeText.enabled = true;
          //labelText.text = item.itemData.displayName;
          //stackSizeText.text = item.stackSize.ToString();
     }

     public void ClearSlot()
     {
          icon.enabled = false;
          currentItem = null;
          //labelText.enabled = false;
          //stackSizeText.enabled = false;

          if (tooltipComponent != null)
          {
               tooltipComponent.message = string.Empty;
               tooltipComponent.names = string.Empty;
          }
     }
}
