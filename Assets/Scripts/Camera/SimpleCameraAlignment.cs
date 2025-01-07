using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleCameraAlignment : MonoBehaviour//, IDataPersistence
{
    [Header("Camera Alignment")]
    [SerializeField] Grid grid;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] GameObject background;
    [SerializeField] Tile[] tiles;


    [Header("Room Indication")]
    [SerializeField] Tilemap playerPosTilemap;
    [SerializeField] Tilemap mapTilemap;
    [SerializeField] Tilemap fogMapTilemap;
    [SerializeField] GameObject playerDot;


    [Header("Map Camera Adjustment")]
    [SerializeField] GameObject mapCamera;
    [SerializeField] float maxMapY = -14f;
    [SerializeField] float minMapY = -20f;
    [SerializeField] float maxMapX = 32f;
    [SerializeField] float minMapX = 10f;

    public static bool MapOn = false;
    public GameObject MapPanel;

    public List<Tile> discoveredTiles = new List<Tile>();

    //public GameObject SecretRoom;

    // Start is called before the first frame update
    void Start()
    {
        MapPanel.SetActive(false);
        PlayerPrefs.SetInt("map_active", 0);
        MapOn = false;
    }

    /*public void LoadData(GameData data)
    {
        this.discoveredTiles = data.discoveredTiles;
        Vector3Int cellPosition = grid.WorldToCell(transform.position);

        foreach(Tile tile in this.discoveredTiles)
        {
            fogMapTilemap.SetTile(cellPosition, null);
        }
    }*/ 

    /*public void SaveData(ref GameData data)
    {
        data.discoveredTiles = this.discoveredTiles;
    }*/

    // Update is called once per frame
    void Update()
    {
        Vector3Int cellPosition = grid.WorldToCell(player.transform.position);
        //discoveredTiles.Add(fogMapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)));
        fogMapTilemap.SetTile(cellPosition + new Vector3Int(11,-19,0), null);
        //Debug.Log(mapTilemap.HasTile(cellPosition + new Vector3Int(11,-19,0)));
        //Debug.Log(mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)));

        if (PlayerPrefs.GetInt("map_active") == 0)
        {
            if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[0])
            {
                //Single Size Room
                mainCamera.transform.position = grid.GetCellCenterWorld(cellPosition);
                background.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[16])
            {
                //Single Size Room
                mainCamera.transform.position = grid.GetCellCenterWorld(cellPosition);
                background.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11,-19,0)) == tiles[1])
            {
                //Top left Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[17])
            {
                //Top left Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[2]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[18])
            {
                //Large Room top center
                Vector3 Pos = player.transform.position;
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[18])
            {
                //Large Room top center
                Vector3 Pos = player.transform.position;
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[3]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[19])
            {
                //Top right Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[19])
            {
                //Top right Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[4]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[20])
            {
                //Large Room left
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[20])
            {
                //Large Room left
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[5]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[21])
            {
                //Large Room Center
                Vector3 Pos = player.transform.position;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[21])
            {
                //Large Room Center
                Vector3 Pos = player.transform.position;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[6]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[22])
            {
                //Large Room Right
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[22])
            {
                //Large Room Right
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[7]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[23])
            {
                //Bottom left Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[23])
            {
                //Bottom left Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[8]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[24])
            {
                //Large Room Bottom Center
                Vector3 Pos = player.transform.position;
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[24])
            {
                //Large Room Bottom Center
                Vector3 Pos = player.transform.position;
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[9]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[25])
            {
                //Bottom right Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[25])
            {
                //Bottom right Corner
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[10]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[26])
            {
                //vertical corridor top
                Vector3 Pos = player.transform.position;
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.x = grid.GetCellCenterWorld(cellPosition).x;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[26])
            {
                //vertical corridor top
                Vector3 Pos = player.transform.position;
                if (Pos.y > grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.x = grid.GetCellCenterWorld(cellPosition).x;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[11]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[27])
            {
                //vertical corridor center
                Vector3 Pos = player.transform.position;
                Pos.x = grid.GetCellCenterWorld(cellPosition).x;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[27])
            {
                //vertical corridor center
                Vector3 Pos = player.transform.position;
                Pos.x = grid.GetCellCenterWorld(cellPosition).x;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[12]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[28])
            {
                //vertical corridor bottom
                Vector3 Pos = player.transform.position;
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.x = grid.GetCellCenterWorld(cellPosition).x;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[28])
            {
                //vertical corridor bottom
                Vector3 Pos = player.transform.position;
                if (Pos.y < grid.GetCellCenterWorld(cellPosition).y) { Pos.y = grid.GetCellCenterWorld(cellPosition).y; }
                Pos.x = grid.GetCellCenterWorld(cellPosition).x;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[13])
            {
                //horizontal corridor left
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.y = grid.GetCellCenterWorld(cellPosition).y;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[29])
            {
                //horizontal corridor left
                Vector3 Pos = player.transform.position;
                if (Pos.x < grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.y = grid.GetCellCenterWorld(cellPosition).y;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y+2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[14]) //|| mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[30])
            {
                //horizontal corridor center
                Vector3 Pos = player.transform.position;
                Pos.y = grid.GetCellCenterWorld(cellPosition).y;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[30])
            {
                //horizontal corridor center
                Vector3 Pos = player.transform.position;
                Pos.y = grid.GetCellCenterWorld(cellPosition).y;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile(cellPosition + new Vector3Int(11, -19, 0)) == tiles[15])
            {
                //horizontal corridor right
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.y = grid.GetCellCenterWorld(cellPosition).y;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y + 2);
            }
            else if (mapTilemap.GetTile<Tile>(cellPosition + new Vector3Int(11, -19, 0)) == tiles[31])
            {
                //horizontal corridor right
                Vector3 Pos = player.transform.position;
                if (Pos.x > grid.GetCellCenterWorld(cellPosition).x) { Pos.x = grid.GetCellCenterWorld(cellPosition).x; }
                Pos.y = grid.GetCellCenterWorld(cellPosition).y;
                Pos.z = grid.GetCellCenterWorld(cellPosition).z;
                mainCamera.transform.position = Pos;
                background.transform.position = new Vector3(Pos.x, Pos.y+2);
            }
        }
        else
        {
            playerDot.transform.position = playerPosTilemap.GetCellCenterWorld(cellPosition);
        }

        //Map Activation
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapOn)
            {
                MapPanel.SetActive(false);
                PlayerPrefs.SetInt("map_active", 0);
                //Debug.Log(PlayerPrefs.GetInt("map_active"));
                MapOn = !MapOn;
                Debug.Log("MapOn is set to " + MapOn);
                Time.timeScale = 1f;
                AudioListener.volume = 1;


            }
            else
            {
                MapPanel.SetActive(true);
                PlayerPrefs.SetInt("map_active", 1);
                //Debug.Log(PlayerPrefs.GetInt("map_active"));
                MapOn = !MapOn;
                Debug.Log("MapOn is set to " + MapOn);
                Time.timeScale = 0f;
                AudioListener.volume = 0;
            }
        }
        //Map Camera Movement
        if (MapOn)
        {
            Vector3 mapCamPos = mapCamera.transform.position;
            Debug.Log("Cam Position: " + mapCamPos);

            if (Input.GetKey(KeyCode.W))
            {
                Vector3 moveUp = new Vector3(0, 0.0625f, 0);
                float upDelta = mapCamPos.y + moveUp.y;
                if(upDelta > maxMapY)
                {
                    mapCamPos = new Vector3(mapCamPos.x, maxMapY, mapCamPos.z);
                }
                else
                {
                    mapCamPos = mapCamPos + moveUp;
                }
                mapCamera.transform.position = mapCamPos;
                Debug.Log("Current Camera Position: " + mapCamPos);
            }

            if (Input.GetKey(KeyCode.S))
            {
                Vector3 moveDown = new Vector3(0, -0.0625f, 0);
                float downDelta = mapCamPos.y + moveDown.y;
                if(downDelta < minMapY)
                {
                    mapCamPos = new Vector3(mapCamPos.x, minMapY, mapCamPos.z);
                }
                else
                {
                    mapCamPos = mapCamPos + moveDown;
                }
                mapCamera.transform.position = mapCamPos;
                Debug.Log("Current Camera Position: " + mapCamPos);
            }

            if (Input.GetKey(KeyCode.A))
            {
                Vector3 moveLeft = new Vector3(-0.0625f, 0, 0);
                float leftDelta = mapCamPos.x + moveLeft.x;
                if (leftDelta < minMapX)
                {
                    mapCamPos = new Vector3(minMapX, mapCamPos.y, mapCamPos.z);
                }
                else
                {
                    mapCamPos = mapCamPos + moveLeft;
                }
                mapCamera.transform.position = mapCamPos;
                Debug.Log("Current Camera Position: " + mapCamPos);
            }

            if (Input.GetKey(KeyCode.D))
            {
                Vector3 moveRight = new Vector3(0.0625f, 0, 0);
                float rightDelta = mapCamPos.x + moveRight.x;
                if (rightDelta > maxMapX)
                {
                    mapCamPos = new Vector3(maxMapX, mapCamPos.y, mapCamPos.z);
                }
                else
                {
                    mapCamPos = mapCamPos + moveRight;
                }
                mapCamera.transform.position = mapCamPos;
                Debug.Log("Current Camera Position: " + mapCamPos);
            }
        }
    }
}
