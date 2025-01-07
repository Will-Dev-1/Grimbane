using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor;

public class enemyMove : enemyBase, IDamageable
{
    //Outside Forces
    public Transform weaponSpawn;
    public GameObject attackObj;
    public GameObject playerObj;
    public GameObject bossObj;

    Animator enemyAnime;

    //Targets
    public Transform moveTarget;
    public Transform[] waypoints = new Transform[2];
    int currentWaypoint = 0;

    //Sprite Comps
    Rigidbody2D rb;
    //AudioSource attackSound;

    //Enemy Stats
    float enemyWeightMod = 2.0f;
    float enemyHealth = 20f;
    float attackDelay = 1.5f;
    float attackLength = .3f;
    float attackDmg = 4.0f;

    //Timers
    float attackTime = -1.0f;

    //Detection Bools
    bool seesPlayer = false;
    bool isHostile = true;
    bool bossSpawn = false;
    bool isMoving = true;
    bool isAttacking = false;

    //Audio Manager
    AudioManager audioManager;

    //Health Bar
    public Slider healthSlider; // Enemy Health Bar
    public Slider easeSlider; // Damage Indicator
    public float maxHealth = 20f;
    private float lerpSpeed = 0.05f;


    private DamageFlash damageFlash;

    //Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        enemyAnime = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = maxHealth;
        //healthSlider.maxValue = maxHealth;

        damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        // Health Bar
        if(healthSlider.value != enemyHealth)
        {
            healthSlider.value = enemyHealth;
        }
        if(healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, enemyHealth, lerpSpeed);
        }

        if(enemyHealth <= 0)
        {
            if(bossSpawn)
            {
                bossObj.GetComponent<Boss>().spawnDeath();
            }
            audioManager.PlaySFX(audioManager.enemyDeath);
            Destroy(gameObject);
        }
         
        if(Mathf.Abs(transform.position.x - waypoints[currentWaypoint].position.x) < .2f)
        {
            updateCurrentWaypoint();
        }

        if(isHostile && seesPlayer)
        {
            moveTarget = playerObj.transform;
        }
        else
        {
            moveTarget = waypoints[currentWaypoint];
        }

        if (transform.position.x < moveTarget.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        transform.Translate(new Vector3(4/enemyWeightMod, 0, 0) * Time.deltaTime);

        if (seesPlayer && (Mathf.Abs(transform.position.x - playerObj.transform.position.x) < 1.3f) && (Time.time >= attackTime + attackDelay))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
        updateAnimator();
    }

    private void FixedUpdate()
    {
        if(isAttacking)
        {
            enemyAttack();
        }
    }

    public override void takeDmg(float dmg)
    {
        enemyHealth -= dmg;
        audioManager.PlaySFX(audioManager.enemyHit);

        damageFlash.CallDamageFlash();
    }

    void updateAnimator()
    {
        enemyAnime.SetBool("isMoving", isMoving);
        enemyAnime.SetBool("isAttacking", isAttacking);
    }

    public override float getDmg()
    {
        return attackDmg;
    }

    void enemyAttack()
    {
        GameObject enemyWeapon = Instantiate(attackObj, weaponSpawn);
        attackTime = Time.time;

        audioManager.PlaySFX(audioManager.enemyAttack);
        //attackSound.Play();

        Destroy(enemyWeapon, attackLength);
    }

    void updateCurrentWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    public void isBossSpawn(GameObject boss)
    {
        bossObj = boss;

        bossSpawn = true;
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

    public override void playerSight(bool sight)
    {
        seesPlayer = sight;
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

    }

}
