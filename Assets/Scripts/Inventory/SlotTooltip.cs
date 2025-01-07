using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotTooltip : MonoBehaviour
{
    public static TooltipManager _instance;

    [TextArea]
    public string message;
    public string names;
    //private bool isPointerOver = false;
    //private float tooltipDelay = 0.2f;
    //private float timeSinceHover = 0f;

    private void OnMouseEnter()
    {
      TooltipManager._instance.SetAndShowToolTip(message);
      TooltipManager._instance.SetAndShowToolTips(names);
    }

    private void OnMouseExit()
    {
      TooltipManager._instance.HideToolTip();
    }

    private void Update()
    {

      if (!gameObject.activeInHierarchy)
      {
        TooltipManager._instance.HideToolTip();
      }
    }
  
    private void OnDisable()
    {
      if (TooltipManager._instance != null)
      {
        TooltipManager._instance.HideToolTip();
      }
    }
}