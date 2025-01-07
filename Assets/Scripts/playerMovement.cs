using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class playerMovement : MonoBehaviour, IDataPersistence
{
    //Outside Forces
    public Transform weaponSpawn;
    public GameObject weaponObj;
    public Transform raySpawn;

    Animator playerMove;

    //Audio Manager
    AudioManager audioManager;
    public AudioSource footstepsSound;

    //Sprite Comps
    Rigidbody2D rb;
    Vector2 movement;

    //Player Stats
    float playerWeightMod = 4.0f;
    public float healthPoints;// = 20;
    float dashDelay = 1.0f;
    float dashLength = .2f;
    float attackDelay = .5f;
    float attackLength = .3f;
    float attackDmg = 4.0f;
    float fallLength = 2.0f;

    float wepMod = 0.0f;
    float ringMod = 1.0f;
    float DmgMod = 4.0f;
    float lengthMod = .3f;
    float delayMod = .5f;

    //Health Bar
    public Slider healthSlider; // Player Health Bar
    public Slider easeSlider; // Damage Indicator
    public float maxHealth = 100f;
    private float lerpSpeed = 0.05f;

    //setting up enums for different weapons
    public enum Weapon
    {
        Sword
    }

    //Timers
    float dashTime = -1.0f;
    float permTime = 0.0f;
    float permLength = 0.5f;
    float attackTime = -1.0f;
    float fallTime;

    //Movement Bools
    bool isMoving = false;
    bool isJumping = false;
    bool isDashing = false;
    bool isPerming = false;
    bool isClimbing = false;
    bool isAttacking = false;
    bool isRight = false;

    //Detection Bools
    bool isGrounded = false;
    bool isLadder = false;
    bool permPlat = false;
    bool isFalling = false;

    //Save Test Data
    //public int attackCount = 0;
    //public TMP_Text attackText;

    //Equipping Items
    public Image equippedItemIcon;
    public GameObject rangedProjectilePrefab;
    public GameObject defaultProjectilePrefab;
    private GameObject defaultRangedProjectilePrefab;
    private ItemData equippedItem;
    private float originalDamage;
    public bool isLifestealEnabled;
    public bool berserkerRingEquipped;

    // Audio
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

       playerMove = gameObject.GetComponent<Animator>();

    }

    private void updateDmg()
    {
        DmgMod = (attackDmg + wepMod) * ringMod;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);

        rb = GetComponent<Rigidbody2D>();
        healthPoints = maxHealth;
        originalDamage = attackDmg;
        isLifestealEnabled = true;

        defaultRangedProjectilePrefab = defaultProjectilePrefab;
        rangedProjectilePrefab = defaultRangedProjectilePrefab;

        //Equipped Item Icon
        equippedItemIcon.sprite = null;
        equippedItemIcon.enabled = false;

        if (rangedProjectilePrefab == null && defaultProjectilePrefab != null)
        {
            rangedProjectilePrefab = defaultProjectilePrefab;
        }
    }

    //Load Data Test
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    //Save Data Test
    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(raySpawn.position, Vector2.down, 0);

        if (hit)
        {
            if (hit.collider.gameObject.tag.Contains("Platform"))
            {
                isGrounded = true;

                if (isFalling)
                {
                    isFalling = false;

                    if (Time.time >= fallTime + fallLength)
                    {
                        this.takeDmg(2 * Mathf.Pow(Time.time - fallTime, 2.0f));
                    }
                }
            }
        }
        else
        {
            isGrounded = false;

            if(!isFalling)
            {
                isFalling = true;
                fallTime = Time.time;
            }
        }

        // Ranged Attack Code
        if (equippedItem != null && equippedItem.isRangedAttack)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PerformRangedAttack();
                audioManager.PlaySFX(audioManager.rangedAttack);
            }
        }
    
        // Health Bar Code
        if(healthSlider.value != healthPoints)
        {
            healthSlider.value = healthPoints;
        }
        if(healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, healthPoints, lerpSpeed);
            //easeSlider.value = Mathf.Clamp(easeSlider.value, 0, healthPoints);
        }
        
        if(healthPoints <= 0)
        {
            //audioManager.PlaySFX(audioManager.playerDeath);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(2);
        }

        movement = new Vector2(Input.GetAxis("Horizontal"), 0);

        if(movement.x > 0)
        {
            isRight = true;
            isMoving = true;
        }
        else if(movement.x < 0)
        {
            isRight = false;
            isMoving = true;
        }
        else if(Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x) >= .005f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Footstep SFX
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && (isGrounded)){
            footstepsSound.enabled = true;
        }
        else
        {
            footstepsSound.enabled = false;
        }

        if (Input.GetKeyDown("space") && (isGrounded || isClimbing))
        {
            isJumping = true;
            isClimbing = false;
            audioManager.PlaySFX(audioManager.playerJump);
            footstepsSound.enabled = false;
        }

        if(Input.GetKey(KeyCode.W) && isLadder)
        {
            movement = new Vector2(movement.x, 1.0f);
            isClimbing = true;
            isMoving = true;
        }
        else if(Input.GetKey(KeyCode.S) && isLadder)
        {
            movement = new Vector2(movement.x, -1.0f);
            isClimbing = true;
            isMoving = true;
        }

        if(Input.GetKey(KeyCode.S) && permPlat)
        {
            isPerming = true;
        }
        else if(!permPlat && Time.time >= permTime + permLength)
        {
            gameObject.layer = 11;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && Time.time >= (dashTime + dashDelay))
        {
            audioManager.PlaySFX(audioManager.playerDash);
            isDashing = true;
            dashTime = Time.time;
        }
        else if(Time.time >= dashTime + dashLength)
        {
            isDashing = false;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= attackTime + delayMod)
        {
            isAttacking = true;
        }

        updateAnimator();

        //Update attack text for testing purposes
        //attackText.text = "Attacks: " + attackCount;
    }

    void FixedUpdate()
    {
        movePlayer(movement);

        if (isDashing)
        {
            rb.AddForce(new Vector2(Mathf.Sign(transform.eulerAngles.y - 180) * -400, 0));
        }

        if (isJumping)
        {
            isJumping = false;
            rb.AddForce(new Vector2(0, 400));
            playerMove.Play("Jump");
        }

        if (isAttacking)
        {
            playerAttack();
            isAttacking = false;
        }

    }

    public void modAttackStats(float Dmod, float Smod)
    {
        wepMod = Dmod;
        lengthMod = attackLength * Smod;
        delayMod = attackDelay * Smod;

        updateDmg();
    }

    //Equipping Items
    public void EquipItem(ItemData itemData)
    {
        audioManager.PlaySFX(audioManager.itemEquipped);

        ResetItemEffects();
        equippedItem = itemData;

        if (equippedItemIcon != null && itemData != null && itemData.icon != null)
        {
            equippedItemIcon.sprite = itemData.icon;
            equippedItemIcon.enabled = true;
        }

        else
        {
            equippedItemIcon.sprite = null;
            equippedItemIcon.enabled = false;
        }

        if (itemData != null)
        {
            Debug.Log("Equipping item: " + itemData.itemName);

            switch (itemData.itemName)
            {
                case "Basic Sword":
                ApplyBasicSword();
                break;

                case "Phantom Dash":
                ApplyPhantomDash();
                break;

                case "Ranged Attack":
                ApplyRangedAttack(itemData);
                break;

                case "Berserker Ring":
                ApplyBerserkerRing(itemData);
                break;

                //case "Secret Room Finder":
                //ActivateSecretRoomFinder(itemData);
                //break;

                default:
                ResetPlayerStats();
                Debug.Log("No specific effects for this item.");
                break;
            }
        }

        else
        {
            ResetPlayerStats();
        }
    }

    public bool IsItemEquipped(ItemData itemData)
    {
        return equippedItem == itemData;
    }

    public void UnequipItem()
    {
        audioManager.PlaySFX(audioManager.itemUnequipped);

        if (equippedItem != null && equippedItem.isRangedAttack)
        {
            rangedProjectilePrefab = defaultRangedProjectilePrefab;
        }
        equippedItem = null;
        equippedItemIcon.sprite = null;
        equippedItemIcon.enabled = false;

        ResetPlayerStats();
        Debug.Log("Item unequipped, stats reset.");
    }


    //Phantom Dash
    private void ApplyPhantomDash()
    {
        dashDelay = 0.5f; //Faster dash cooldown
        dashLength = 0.3f; //Longer dash duration
        Debug.Log("Phantom Dash equipped.");
    }

    //Ranged Attack
    private void ApplyRangedAttack(ItemData itemData)
    {
        if (itemData != null && itemData.projectilePrefab != null)
        {
            rangedProjectilePrefab = itemData.projectilePrefab; // Assign the prefab
            Debug.Log("Ranged Attack equipped: Projectile prefab assigned as " + rangedProjectilePrefab.name);
        }

        else
        {
            rangedProjectilePrefab = defaultRangedProjectilePrefab;
            Debug.LogError("Ranged Attack item is missing the projectile prefab.");
        }
    }

    //Ranged Attack 2
    private void PerformRangedAttack()
    {
        if (equippedItem != null && equippedItem.isRangedAttack)
        {
            if (healthPoints > equippedItem.healthCostPerShot)
            {
                healthPoints -= equippedItem.healthCostPerShot;

                if (rangedProjectilePrefab != null)
                {
                    Vector3 spawnPosition = transform.position + (isRight ? Vector3.right : Vector3.left);
                    GameObject projectile = Instantiate(rangedProjectilePrefab, spawnPosition, transform.rotation);

                    RangedProjectile projScript = projectile.GetComponent<RangedProjectile>();
                    if (projScript != null)
                    {
                        projScript.SetDamage(equippedItem.projectileDamage);
                    }

                    Debug.Log("Shot a projectile, remaining health: " + healthPoints);
                }

                else
                {
                    Debug.LogError("RangedProjectilePrefab is not assigned.");
                }
            }

            else
            {
                Debug.Log("Not enough health to shoot!");
            }
        }
    }

    //Berserker Ring
    public void ApplyBerserkerRing(ItemData itemData)
    {
        if (itemData != null)
        {
            ringMod =  itemData.damageMultiplier;

            updateDmg();

            Debug.Log("Berserker Ring: disableLifesteal = " + itemData.disableLifesteal);

            if (itemData.disableLifesteal)
            {
                isLifestealEnabled = false;
                berserkerRingEquipped = true;
                Debug.Log("Berserker Ring equipped: Increased damage, lifesteal disabled.");
            }
        }
    }

    //Default Sword
    private void ApplyBasicSword()
    {
        attackDmg = originalDamage;
        isLifestealEnabled = true;
        Debug.Log("Basic Sword equipped.");
    }

    // Reset player stats to default
    public void ResetPlayerStats()
    {
        dashDelay = 1.0f; // Default dash cooldown
        dashLength = 0.2f; // Default dash duration
        ringMod = 1.0f; // Default damage
        isLifestealEnabled = true; // Reset lifesteal
        //isSecretRoomFinderActive = false;
        berserkerRingEquipped = false;

        if (!berserkerRingEquipped)
        {
            isLifestealEnabled = true;
        }

        if (equippedItem == null || !equippedItem.isRangedAttack)
        {
            rangedProjectilePrefab = defaultRangedProjectilePrefab;
        }

        updateDmg();
    }

    private void ResetItemEffects()
    {
        attackDmg = originalDamage;
        dashDelay = 1.0f;
        dashLength = 0.2f;
        isLifestealEnabled = true;
        //isSecretRoomFinderActive = false;
        berserkerRingEquipped = false;

        if (!berserkerRingEquipped)
        {
            isLifestealEnabled = true;
        }

        if (equippedItem == null || !equippedItem.isRangedAttack)
        {
            rangedProjectilePrefab = defaultRangedProjectilePrefab;
        }

        updateDmg();
        Debug.Log("Item effects reset to defaults.");
    }

    void updateAnimator()
    {
        playerMove.SetBool("isJumping", isJumping);
        playerMove.SetBool("isClimbing", isClimbing);
        playerMove.SetBool("isGrounded", isGrounded);
        playerMove.SetBool("isMoving", isMoving);
        playerMove.SetBool("isAttacking", isAttacking);
        playerMove.SetBool("isDashing", isDashing);
    }

    void movePlayer(Vector2 dir)
    {
        if (isClimbing)
        {
            rb.velocity = new Vector2(dir.x * playerWeightMod, dir.y * playerWeightMod);
            rb.gravityScale = 0;
        }
        else if (dir.x != 0)
        {
            rb.velocity = new Vector2(dir.x * playerWeightMod, rb.velocity.y);
            rb.gravityScale = 1;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.gravityScale = 1;
        }

        if (isPerming)
        {
            gameObject.layer = 14;

            isPerming = false;
            isGrounded = false;

            permTime = Time.time;
        }

        if (isRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (!isRight)
        {
            transform.eulerAngles = new Vector3(0,180, 0);
        }
    }

    void playerAttack()
    {
        audioManager.PlaySFX(audioManager.playerAttack);
        playerMove.Play("Attacking");
        GameObject weapon = Instantiate(weaponObj, weaponSpawn);

        PlayerSword swordScript = weapon.GetComponent<PlayerSword>();
        if (swordScript != null)
        {
            swordScript.SetPlayer(this);
        }

        attackTime = Time.time;

        Destroy(weapon, lengthMod);

        //attackCount++;
    }

    public void takeDmg(float dmg)
    {
        audioManager.PlaySFX(audioManager.playerHit);
        healthPoints -= dmg;

    }

    public void lifeSteal()
    {
        healthPoints += attackDmg;
        if(healthPoints > maxHealth)
        {
            healthPoints = maxHealth;
        }
    }

    //Berserker Ring 2
    public bool IsLifestealEnabled()
    {
        return isLifestealEnabled;
    }

    public float getDmg()
    {
        return DmgMod;
    }

    // Health Potion Code
     public void IncreaseHealth(float amount)
    {
        healthPoints += amount;
        if (healthPoints > maxHealth)
        {
            healthPoints = maxHealth;
        }
    }

    private void OnCollisionStay2D(Collision2D Coll)
    {
        if (Coll.gameObject.tag.Contains("Permeable"))
        {
            permPlat = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D Coll)
    {

        if (Coll.gameObject.tag.Contains("Ladder"))
        {
            isLadder = true;
        }

        if (Coll.gameObject.tag == "KillPlane")
        {
            takeDmg(maxHealth);
        }
    }

    private void OnCollisionExit2D(Collision2D Coll)
    {
        if (Coll.gameObject.tag.Contains("Permeable"))
        {
            permPlat = false;
        }
    }

    private void OnTriggerExit2D(Collider2D Coll)
    {
        if (Coll.gameObject.tag.Contains("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}