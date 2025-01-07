using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    //setting up the damage value for the projectile and a timer + duration for how long it lasts
    public int Damage;
    public float timer;
    public float duration;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 5 * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            Destroy(this.gameObject);
        }
    }

    //aims the projectile in a direction
    public void Aim(Transform playerPos)
    {
        target = playerPos;
        Vector3 targetVector = target.position - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg - 90.0f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }

    //meant to inflict damage whenever the projectile comes into contact with something
    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Player":
                {
                    Debug.Log("Ow.");
                    col.gameObject.GetComponent<playerMovement>().takeDmg((float)Damage);
                    Destroy(this.gameObject);
                    break;
                }
            case "BossAttack":
                {
                    break;
                }
            case "Platform":
                {
                    Destroy(this.gameObject);
                    break;
                }
                //destroys the projectile if it comes into contact with anything that doesn't take damage
            default:
                {
                    break;
                }
        }
    }
}
