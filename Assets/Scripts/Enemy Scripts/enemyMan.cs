using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMan : MonoBehaviour
{
    //Containers
    public List<GameObject> enemyList = new List<GameObject>();
    public Transform[] enemyWaypoints = new Transform[20];
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach(GameObject enemy in enemyList)
        {
            if (enemy.TryGetComponent<enemyBase>(out enemyBase currentEnemy))
            {
                currentEnemy.setWaypoints(enemyWaypoints[2 * i], enemyWaypoints[2 * i + 1]);
                currentEnemy.setPlayerObj(playerObj);
            }

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
