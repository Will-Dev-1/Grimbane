using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private playerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(playerMovement playerRef)
    {
        player = playerRef;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
       if (player == null) return;
       
       if (coll.gameObject.CompareTag("Enemy"))
        {
            if (coll.gameObject.TryGetComponent<enemyBase>(out enemyBase currentEnemy))
            {
                currentEnemy.takeDmg(player.getDmg());

                if (player.IsLifestealEnabled())
                {
                    player.lifeSteal();
                }
            }
        }

        else if (coll.gameObject.tag.Contains("Boss"))
        {
            if (!coll.gameObject.tag.Contains("Attack"))
            {
                coll.gameObject.GetComponent<Boss>().takeDmg(player.getDmg());

                if (player.IsLifestealEnabled())
                {
                    player.lifeSteal();
                }
            }

            else
            {
                coll.gameObject.GetComponentInParent<Boss>().takeDmg(player.getDmg() / 2);

                if (player.IsLifestealEnabled())
                {
                    player.lifeSteal();
                }
            }
        }
    }
}