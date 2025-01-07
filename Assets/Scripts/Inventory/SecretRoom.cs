using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    public GameObject room;
    public Collider2D roomCollider;
    public ItemData secretRoomFinderItem;
    private bool isRoomRevealed = false;

    void Start()
    {
        if (room != null)
        {
            room.SetActive(false);
        }

        if (roomCollider != null)
        {
            roomCollider.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement player = collision.GetComponent<playerMovement>();
            if (player != null && player.IsItemEquipped(secretRoomFinderItem))
            {
                RevealRoom();
            }
        }
    }

    private void RevealRoom()
    {
        if (!isRoomRevealed)
        {
            if (room != null)
            {
                room.SetActive(true);
            }

            if (roomCollider != null)
            {
                roomCollider.enabled = true;
            }

            isRoomRevealed = true;
            Debug.Log("Secret room revealed!");
        }
    }
}