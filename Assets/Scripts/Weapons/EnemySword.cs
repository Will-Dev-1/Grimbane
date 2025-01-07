using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag.Contains("Player"))
        {
            if (transform.parent.parent.TryGetComponent<enemyBase>(out enemyBase enemy))
            {
                coll.gameObject.GetComponent<playerMovement>().takeDmg(enemy.getDmg());
            }
        }
    }
}
