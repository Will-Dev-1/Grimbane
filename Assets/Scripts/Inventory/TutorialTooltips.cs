using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TutorialTooltips : MonoBehaviour
{
  public static TooltipManager _instance;
  public GameObject player;
  public GameObject board;
  public bool ShowGUI = false;

  private void Awake()
  {
    board.gameObject.SetActive(false);
  }

  private void Start()
  {
    
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.tag == "Player")
    {
      ShowGUI = true;
      board.gameObject.SetActive(true);
    }
  }

  void OnTriggerExit2D(Collider2D collision)
  {
    if(collision.tag == "Player")
    {
      ShowGUI = false;
      board.gameObject.SetActive(false);
    }
  }
}