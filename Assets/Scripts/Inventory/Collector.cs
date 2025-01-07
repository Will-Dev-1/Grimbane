using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public InventoryMain inventory;
    public playerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        
        if(collectible != null)
        {
            TooltipManager._instance.HideToolTip();
            collectible.Collect(this);
        }
    }
}