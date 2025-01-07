using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class enemyFlyerMove : enemyBase, IDamageable
{
    //Outside Forces
    public Transform weaponSpawn;
    public GameObject attackObj;
    public GameObject playerObj;

    Animator ambusherAnim;

    //Targets
    public Transform moveTarget;
    public Transform[] waypoints = new Transform[2];
    int currentWaypoint = 0;

    //Sprite Comps
    Rigidbody2D rb;
    //AudioSource attackSound;

    //Enemy Stats
    float enemyHealth = 10f;
    float attackDelay = 1.0f;
    float attackLength = .3f;
    float attackDmg = 16.0f;

    //Timers
    float attackTime = -1.0f;

    //Detection Bools
    bool seesPlayer = false;
    bool isHostile = true;
    bool isMoving = true;

    //Audio Manager
    AudioManager audioManager;

    //Health Bar
    public Slider healthSlider; // Enemy Health Bar
    public Slider easeSlider; // Damage Indicator
    public float maxHealth = 10f;
    private float lerpSpeed = 0.05f;

    //Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        ambusherAnim = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = maxHealth;
        //attackSound = GetComponent<AudioSource>();
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

        Vector3 targetVector = moveTarget.position - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg - 90.0f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

        transform.Translate(Vector3.up * 2 * Time.deltaTime);

        if (seesPlayer && (Mathf.Abs(transform.position.x - playerObj.transform.position.x) < 1.8f) && (Time.time >= attackTime + attackDelay))
        {
            enemyAttack();
        }
        updateAnimator();
    }
    public override void takeDmg(float dmg)
    {
        enemyHealth -= dmg;
        audioManager.PlaySFX(audioManager.spiderHit);

    }

    void updateAnimator()
    {
        ambusherAnim.SetBool("isMoving", isMoving);
        //enemyAnime.SetBool("isAttacking", isAttacking);
    }
    public override float getDmg()
    {
        return attackDmg;
    }

    void enemyAttack()
    {
        GameObject enemyWeapon = Instantiate(attackObj, weaponSpawn);
        attackTime = Time.time;

        audioManager.PlaySFX(audioManager.spiderAttack);
        //attackSound.Play();

        Destroy(enemyWeapon, attackLength);
    }

    void updateCurrentWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
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
