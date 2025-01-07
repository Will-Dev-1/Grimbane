using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Tilemaps;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> itemsCollected;
    //public List<Tile> discoveredTiles;


    public GameData()
    {
        playerPosition = new Vector3(-341, 23, 0);
        itemsCollected = new SerializableDictionary<string, bool>();
        //discoveredTiles = new List<Tile>();
    }
}
