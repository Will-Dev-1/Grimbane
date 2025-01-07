using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;
    public GameObject tooltipPanel;
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameComponent;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            _instance = this;
        }
    }
    
    void Start()
    {
        Cursor.visible = true;
        tooltipPanel.SetActive(false);
    }

    void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.x += 15;
            mousePosition.y -= 15;
            tooltipPanel.transform.position = mousePosition;
        }
    }

    public void SetAndShowToolTip(string message)
    {
        if (!tooltipPanel.activeSelf)
        {
            //Debug.Log("Activating tooltip with message: " + message);
            tooltipPanel.SetActive(true);
        }

        textComponent.text = message;
    }

    public void SetAndShowToolTips(string names)
    {
        if (!tooltipPanel.activeSelf) // Only activate if not already active
        {
            //Debug.Log("Activating tooltip with names: " + names);
            tooltipPanel.SetActive(true);
        }

        nameComponent.text = names;
    }

    public void HideToolTip()
    {
        if (tooltipPanel != null && tooltipPanel.activeSelf)
        {
            //Debug.Log("Hiding tooltip...");
            tooltipPanel.SetActive(false);
            textComponent.text = string.Empty;
            nameComponent.text = string.Empty;
        }
    }

    public void HideTooltipExternally()
    {
        HideToolTip();
    }
}