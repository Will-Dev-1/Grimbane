using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAttacking(bool att)
    {
        isAttacking = att;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player" && isAttacking)
        {
            coll.gameObject.GetComponent<playerMovement>().takeDmg(GetComponentInParent<Boss>().getDmg());
        }
    }
}
