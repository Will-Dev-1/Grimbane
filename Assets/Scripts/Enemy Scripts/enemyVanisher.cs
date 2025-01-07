using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class enemyVanisher : enemyBase, IDamageable
{
    //Outside Forces
    public Transform projectileSpawn;
    public GameObject playerObj;
    public GameObject projectile;

    Animator vanisherAnim;

    //Targets
    public Transform moveTarget;
    public Transform[] waypoints = new Transform[2];
    int currentWaypoint = 0;

    //Sprite Comps
    Rigidbody2D rb;
    //AudioSource attackSound;

    //Enemy Stats
    float enemyHealth = 1.0f;
    float attackDelay = 1.5f;
    float attackDmg = 16.0f;
    float visibleLength = 3.0f;
    //Timers

    float visibleTime = -1.0f;

    //Detection Bools
    bool seesPlayer = false;
    bool isHostile = true;
    bool isInvisible = false;
    bool attacked = true;

    //Audio Manager
    AudioManager audioManager;

    //Health Bar
    public Slider healthSlider; // Enemy Health Bar
    public Slider easeSlider; // Damage Indicator
    public float maxHealth = 1.0f;
    private float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        vanisherAnim = gameObject.GetComponent<Animator>();

        healthSlider.maxValue = maxHealth;
        easeSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Health Bar
        if (healthSlider.value != enemyHealth)
        {
            healthSlider.value = enemyHealth;
        }
        if (healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, enemyHealth, lerpSpeed);
        }

        if (enemyHealth <= 0)
        {
            audioManager.PlaySFX(audioManager.spiderDeath);
            Destroy(gameObject);
        }

        if (Mathf.Abs(transform.position.x - waypoints[currentWaypoint].position.x) < .2f)
        {
            updateCurrentWaypoint();
        }

        if (isHostile && seesPlayer)
        {
            moveTarget = playerObj.transform;
        }
        else
        {
            moveTarget = waypoints[currentWaypoint];
        }

        if(isInvisible)
        {
            moveEnemy();
        }
        else
        {
            if ((Time.time > (visibleTime + attackDelay)) && !attacked)
            {
                enemyAttack();
            }
            else if(Time.time > (visibleTime + visibleLength))
            {
                setInvisible();
            }
        }

        updateAnimator();
    }

    void moveEnemy()
    {
        if (seesPlayer)
        {
            if (Vector3.Distance(new Vector3(playerObj.transform.position.x, 0, 0), new Vector3(transform.position.x, 0, 0)) < 3.0f)
            {
                transform.Translate(new Vector3(-1 * Mathf.Sign(playerObj.transform.position.x - transform.position.x), Mathf.Sign(playerObj.transform.position.y - transform.position.y), 0) * Time.deltaTime);
            }
            else if (Mathf.Abs(transform.position.x - playerObj.transform.position.x) > 4.0f)
            {
                transform.Translate(new Vector3(Mathf.Sign(playerObj.transform.position.x - transform.position.x), Mathf.Sign(playerObj.transform.position.y - transform.position.y), 0) * Time.deltaTime);
            }
            else if (Mathf.Abs(transform.position.y - playerObj.transform.position.y) > .5f)
            {
                transform.Translate(new Vector3(0, Mathf.Sign(playerObj.transform.position.y - transform.position.y), 0) * Time.deltaTime);
            }
            else
            {
                setVisible();
            }
        }
        else
        {
            transform.Translate(new Vector3(Mathf.Sign(moveTarget.transform.position.x - transform.position.x), Mathf.Sign(moveTarget.transform.position.y - transform.position.y), 0) * Time.deltaTime);
        }
    }
    void enemyAttack()
    {
        audioManager.PlaySFX(audioManager.vanisherAttack);
        attacked = true;
        GameObject vanisherShot;

        //currently set to throw a projectile directly aimed at the player
        vanisherShot = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation) as GameObject;
        vanisherShot.GetComponent<ProjectileProperties>().Aim(playerObj.transform);

        Debug.Log("Sneak attack!");

    }

    void setVisible()
    {
        audioManager.PlaySFX(audioManager.vanisherAppear);
        isInvisible = false;

        attacked = false;

        if (playerObj.transform.position.x > transform.position.x)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        this.gameObject.transform.GetComponentInChildren<Canvas>().enabled = true;

        visibleTime = Time.time;
    }

    void setInvisible()
    {
        isInvisible = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        this.gameObject.transform.GetComponentInChildren<Canvas>().enabled = false;
    }

    void updateAnimator()
    {

    }

    public override void takeDmg(float dmg)
    {
        enemyHealth -= dmg;
        audioManager.PlaySFX(audioManager.spiderHit);

    }

    public override float getDmg()
    {
        return attackDmg;
    }

    public override void playerSight(bool sight)
    {
        seesPlayer = sight;
    }

    public override void setPlayerObj(GameObject Obj)
    {
        playerObj = Obj;
    }

    public override void setWaypoints(Transform wp1, Transform wp2)
    {
        waypoints[0] = wp1;
        waypoints[1] = wp2;
    }

    public void setHostile(bool aggro)
    {
        isHostile = aggro;
    }

    void updateCurrentWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }
}
