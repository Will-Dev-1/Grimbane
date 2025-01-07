using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  private bool isPointerOver = false;
  private Coroutine showTooltipCoroutine;
  public static TooltipManager _instance;

  [TextArea]
  public string message;
  public string names;

  private void Start()
  {
    TooltipManager._instance.HideToolTip();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    //Debug.Log("Pointer entered: " + names);
    isPointerOver = true;

    if (showTooltipCoroutine == null)
    {
      showTooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    //Debug.Log("Pointer exited: " + names);
    isPointerOver = false;

    if (showTooltipCoroutine != null)
    {
      StopCoroutine(showTooltipCoroutine);
      showTooltipCoroutine = null;
    }

    TooltipManager._instance.HideToolTip();
  }

  private IEnumerator ShowTooltipWithDelay()
  {
    //Debug.Log("Starting tooltip delay...");
    yield return new WaitForSeconds(0.2f);

    if (isPointerOver)
    {
      //Debug.Log("Showing tooltip: " + names);
      TooltipManager._instance.SetAndShowToolTip(message);
      TooltipManager._instance.SetAndShowToolTips(names);
    }

    showTooltipCoroutine = null;
  }

  private void OnDisable()
  {
    if (TooltipManager._instance != null)
    {
        TooltipManager._instance.HideToolTip();
    }
  }
}