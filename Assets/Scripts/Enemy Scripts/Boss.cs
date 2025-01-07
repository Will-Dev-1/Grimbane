using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour, IDamageable
{
    //Outside forces
    public Transform[] projectileSpawn;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject projectile;
    public GameObject playerObj;
    public Transform currentSpawn;

    public GameObject[] spawnableEnemies = new GameObject[2];

    Animator LFist;
    Animator RFist;

    //enemy stats
    float enemyHealth = 100.0f;
    float attackDelay = 1.0f;
    float attackSpeed = 4.0f;
    float attackDmg = 8.0f;
    int numberOfSpawn = 0;

    //Probs
    float[,] Probs = new float[5, 2];
    
    //Audio Manager
    AudioManager audioManager;

    //Health Bar
    public Slider healthSlider; // Enemy Health Bar
    public Slider easeSlider; // Damage Indicator
    public float maxHealth = 100f;
    private float lerpSpeed = 0.05f;
    

    enum BossState
    {
        Idle,
        Slam,
        SlamSlide,
        Shoot,
        Spawn,
        Start,
        Death
    }

    BossState currentState = BossState.SlamSlide;

    //Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        LFist = leftHand.GetComponent<Animator>();
        RFist = rightHand.GetComponent<Animator>();
    }

    void chooseNextState()
    {
        idleSet();
        float choiceProb = Random.Range(0.0f, 1.0f);

        BossState nextState = BossState.Idle;

        float sumChance = 0.0f;

        for (int i = 0; i < 5;i++)
        {
            sumChance += Probs[i,1];
            if(choiceProb <= sumChance)
            {
                nextState = (BossState)i;
                updateProbs(i);
                break;
            }
        }

        string lastStateString = currentState.ToString() + "State";

        string nextStateString = nextState.ToString() + "State";

        currentState = nextState;

        StopCoroutine(lastStateString);

        StartCoroutine(nextStateString);
    }

    void chooseStartState()
    {
        StartCoroutine("StartState");
    }

    void chooseSelectState(string state)
    {
        StopCoroutine(currentState.ToString() + "State");
        currentState = (BossState)System.Enum.Parse(typeof(BossState), state);
        StartCoroutine(state + "State");
    }

    void chooseDeathState()
    {
        StopCoroutine(currentState.ToString() + "State");
        StartCoroutine("DeathState");
    }

    IEnumerator StartState()
    {
        //beginning animation if we want

        yield return null;
        chooseNextState();
    }

    IEnumerator DeathState()
    {
        Destroy(this.gameObject);

        yield return null;

    }

    IEnumerator IdleState()
    {
        float timer = Random.Range(attackDelay, attackDelay * 3.0f);

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        yield return null;

        chooseNextState();
    }

    IEnumerator SpawnState()
    {
        float enemyChance = Random.Range(0.0f, 1.0f);

        if(enemyChance < .75f)
        {
            GameObject newEnemy = Instantiate(spawnableEnemies[0], projectileSpawn[2].position, projectileSpawn[2].rotation);

            newEnemy.GetComponent<enemyMove>().setWaypoints(playerObj.transform, playerObj.transform);
            newEnemy.GetComponent<enemyMove>().setPlayerObj(playerObj);

            newEnemy.GetComponent<enemyMove>().isBossSpawn(this.gameObject);
        }
        else
        {
            GameObject newEnemy = Instantiate(spawnableEnemies[1], projectileSpawn[2].position, projectileSpawn[2].rotation);

            newEnemy.GetComponent<enemyTankMove>().setWaypoints(playerObj.transform, playerObj.transform);
            newEnemy.GetComponent<enemyTankMove>().setPlayerObj(playerObj);

            newEnemy.GetComponent<enemyTankMove>().isBossSpawn(this.gameObject);
        }

        numberOfSpawn++;

        float timer = Random.Range(attackDelay, attackDelay * 3.0f);

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        yield return null;

        chooseNextState();
    }

    IEnumerator ShootState()
    {
        int spawnSpot = Random.Range(0, 2);
        currentSpawn = projectileSpawn[spawnSpot];
        float timer = 4.0f;
        float shotTimer = 0.0f;

        if(spawnSpot == 0)
        {
            LFist.SetBool("isIdle", false);
            LFist.SetBool("isShooting", true);
        }
        else
        {
            RFist.SetBool("isIdle", false);
            RFist.SetBool("isShooting", true);
        }

        for (int i = 0; i < 4; i++)
        {
            Shoot();

            timer = .5f;
            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

             shotTimer = Random.Range(attackDelay, attackDelay * 3.0f);

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            yield return null;
        }

        if(spawnSpot == 0)
        {
            LFist.SetBool("isShooting", false);
        }
        else
        {
            RFist.SetBool("isShooting", false);
        }

        idleSet();
        yield return null;

        chooseNextState();
    }

    IEnumerator SlamSlideState()
    {
        GameObject attackingHand;
        int handSpawn = 0;
        float timer = 0.0f;
        if (playerObj.transform.position.x < transform.position.x)
        {
            attackingHand = leftHand;
            handSpawn = 0;
            LFist.SetBool("isSlamming", true);
            LFist.SetBool("isIdle", false);
        }
        else
        {
            attackingHand = rightHand;
            handSpawn = 1;
            RFist.SetBool("isSlamming", true);
            RFist.SetBool("isIdle", false);
        }
        
        attackingHand.GetComponent<BossWeapon>().setAttacking(true);


        while (Mathf.Abs(attackingHand.transform.position.x - playerObj.transform.position.x) > .5f)
        {
            attackingHand.transform.Translate(new Vector3(Mathf.Sign(playerObj.transform.position.x - attackingHand.transform.position.x), 0, 0) * Time.deltaTime * attackSpeed);
            yield return null;
        }

        timer = .4f;

        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        while (attackingHand.transform.position.y > 22.9f)
        {
            attackingHand.transform.Translate(Vector3.down * Time.deltaTime * attackSpeed * 4.5f);
            yield return null;
        }

        timer = 1.5f;

        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        while (Mathf.Abs(attackingHand.transform.position.x - transform.position.x) > .5F)
        {
            attackingHand.transform.position = Vector2.MoveTowards(attackingHand.transform.position, new Vector2(transform.position.x, attackingHand.transform.position.y), attackSpeed * Time.deltaTime * 4);
            yield return null;
        }

        attackingHand.GetComponent<BossWeapon>().setAttacking(false);

        timer = 1.0f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        while (attackingHand.transform.position != projectileSpawn[handSpawn].position)
        {
            attackingHand.transform.position = Vector2.MoveTowards(attackingHand.transform.position, projectileSpawn[handSpawn].position, attackSpeed * Time.deltaTime * 2);
            yield return null;
        }

        if(handSpawn == 0)
        {
            LFist.SetBool("isSlamming", false);
        }
        else
        {
            RFist.SetBool("isSlamming", false);
        }

        idleSet();

        yield return null;

        chooseNextState();
    }

    IEnumerator SlamState()
    {
        GameObject attackingHand;
        int handSpawn = 0;
        float timer = 0.0f;

        if(playerObj.transform.position.x < transform.position.x)
        {
            attackingHand = leftHand;
            handSpawn = 0;
            LFist.SetBool("isSlamming", true);
            LFist.SetBool("isIdle", false);
        }
        else
        {
            attackingHand = rightHand;
            handSpawn = 1;
            RFist.SetBool("isSlamming", true);
            RFist.SetBool("isIdle", false);
        }

        attackingHand.GetComponent<BossWeapon>().setAttacking(true);
        audioManager.PlaySFX(audioManager.bossAttack);


        while(Mathf.Abs(attackingHand.transform.position.x - playerObj.transform.position.x) > .5f)
        {
            attackingHand.transform.Translate(new Vector3(Mathf.Sign(playerObj.transform.position.x - attackingHand.transform.position.x), 0, 0) * Time.deltaTime * attackSpeed);
            yield return null;
        }

        while(attackingHand.transform.position.y > 22.9f)
        {
            attackingHand.transform.Translate(Vector3.down * Time.deltaTime * attackSpeed * 4.5f);
            yield return null;
        }

        attackingHand.GetComponent<BossWeapon>().setAttacking(false);

        timer = 1.5f;

        while(timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        while(attackingHand.transform.position != projectileSpawn[handSpawn].position)
        {
            attackingHand.transform.position = Vector2.MoveTowards(attackingHand.transform.position, projectileSpawn[handSpawn].position, attackSpeed * Time.deltaTime * 2);
            yield return null;
        }

        if (handSpawn == 0)
        {
            LFist.SetBool("isSlamming", false);
        }
        else
        {
            RFist.SetBool("isSlamming", false);
        }

        idleSet();
        yield return null;

        chooseNextState();
    }

    void updateProbs()
    {
        float sum = 0;
        for (int i = 0; i < 5; i++)
        {
            Probs[i, 0] = 1.0f;
            sum += Probs[i, 1];
        }

        for(int i = 0;i < 5; i++)
        {
            Probs[i,1] = Probs[i,0] / sum;
        }
    }

    void updateProbs(int newState)
    {
        float sum = 0;

        for (int i = 0;i < 5; i++)
        {
            if(i != newState && i != 4)
            {
                Probs[i, 0] = Probs[i,0] + .5f;
                sum += Probs[i,0];
            }
            else if (i == newState)
            {
                if (Probs[i, 0] - .5 < 1.0f)
                {
                    Probs[i, 0] = 1.0f;
                }
                else
                {
                    Probs[i, 0] = Probs[i, 0] - .5f;
                }
                sum += Probs[i, 0];
            }
            else if(i == 4)
            {
                if(numberOfSpawn >= 4)
                {
                    Probs[i, 0] = 0.0f;
                }
                else
                {
                    Probs[i, 0] = (1.0f/(numberOfSpawn + 1));
                }
                sum += Probs[i,0];
            }
        }

        for (int i = 0; i < 5; i++)
        {
            Probs[i, 1] = Probs[i, 0] / sum;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        updateProbs();
        enemyHealth = maxHealth;
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
        
        if(enemyHealth <= 0) { 
            chooseDeathState();
            audioManager.PlaySFX(audioManager.bossDeath);
            SceneManager.LoadScene(3);
        }

        if(enemyHealth  <= 50)
        {
            attackDelay = .25f;

            attackSpeed = 8.0f;

            attackDmg = 12.0f;
        }
    }

    void idleSet()
    {
        LFist.SetBool("isIdle", true);
        RFist.SetBool("isIdle", true);
    }

    public void takeDmg(float dmg)
    {
        audioManager.PlaySFX(audioManager.bossHit);
        enemyHealth -= dmg;
    }

    public float getDmg()
    {
        return attackDmg;
    }

    //method for having the boss use a projectile attack
    public void Shoot()
    {
        audioManager.PlaySFX(audioManager.bossShoot);
        GameObject shotProjectile;

        shotProjectile = Instantiate(projectile, currentSpawn.position, currentSpawn.rotation) as GameObject;
        shotProjectile.GetComponent<ProjectileProperties>().Aim(playerObj.transform);

        Debug.Log("Pew pew");
    }

    public void spawnDeath()
    {
        numberOfSpawn -= 1;
    }

    public void startBoss()
    {
        StartCoroutine(currentState.ToString() + "State");
        audioManager.ChangeMusic(2);
    }
}
