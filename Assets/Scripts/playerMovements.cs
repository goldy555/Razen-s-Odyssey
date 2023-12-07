using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerMovements : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverCanvas;
    public Mp_Bar mpBar;
    public Text coinText;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    private Rigidbody2D rigidb;
    private Animator animator;
    private bool hasWeapon = false;
    private AnimatorOverrideController currentOverrideController;
    private bool isGrounded;
    private int jumpCount;
    private const int maxJumps = 2;
    private weaponChange nearbyWeapon;
    private int coinCount = 0;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool isRolling = false;
    private player_Attack playerAttack;
    private playerHP_Bar playerHitpoint;
    private float currentDirection = 1f;
    
    void Start()
    {
        rigidb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<player_Attack>();
        playerHitpoint = GetComponent<playerHP_Bar>();
        animator.SetBool("isIdle", false);
        UpdateCoinUI();
    }
    //calling different function depending on certain input conditions
    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.E) && nearbyWeapon != null)
        {
            nearbyWeapon.PickUp(this.gameObject);
        }

        float dirX = Input.GetAxisRaw("Horizontal");
        HandleMovementInput(dirX);
        if (Input.GetKeyDown(KeyCode.V) && isGrounded && !isRolling)
        {
            StartCoroutine(TriggerRoll());
        }

        HandleJumpInput();

        if (Input.GetKeyDown(KeyCode.B) && !mpBar.IsRegenerating())
        {
            HandleMPBarAndFireball();
        }

        HandleWeaponInputs();
    }

    // handles the player running animation and flip the sprite to the direction player was moving
    void HandleMovementInput(float dirX)
    {
        rigidb.velocity = new Vector2(dirX * 2.2f, rigidb.velocity.y);

        // Flip sprite based on direction
        if (dirX > 0)
        {
            // Facing right
            spriteRenderer.flipX = false;
            currentDirection = 1f;
            Debug.Log("Player is facing right.");
        }
        else if (dirX < 0)
        {
            // Facing left
            spriteRenderer.flipX = true;
            currentDirection = -1f;
            Debug.Log("Player is facing left.");
        }

        animator.SetBool("isRunning", Mathf.Abs(rigidb.velocity.x) > 0.1f);
        animator.SetBool("isJumping", !isGrounded);
    }
    //handles the double jump 
    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rigidb.velocity = new Vector2(rigidb.velocity.x, 4.4f);
                isGrounded = false;
                jumpCount = 1;
            }
            else if (jumpCount < maxJumps)
            {
                rigidb.velocity = new Vector2(rigidb.velocity.x, 4.4f);
                jumpCount++;
            }
        }
    }
    //handles the player with weapon attack animation when f and g key are pressed
    void HandleWeaponInputs()
    {
        if (hasWeapon)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F key pressed");
                animator.ResetTrigger("TriggerAttack2");
                animator.SetTrigger("TriggerAttack1");
                animator.SetBool("isIdle", false);

                HandleWeaponAttack();
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("G key pressed");
                animator.ResetTrigger("TriggerAttack1");
                animator.SetTrigger("TriggerAttack2");
                animator.SetBool("isIdle", false);

                HandleWeaponAttack();
            }
            else
            {
                animator.SetBool("isIdle", true);
            }
        }
    }
    //shoots the fireball prefab when b is pressed 
    void HandleMPBarAndFireball()
    {
        Debug.Log("B pressed");
        if (mpBar.UseMP(1))
        {
            Debug.Log("Shooting fireball");
            ShootFireball();
        }
        else
        {
            Debug.Log("Not enough MP");
        }
    }
    //trigger the dead animation when player die, while showing the game over canvas
    public void Die()
    {
        if (isDead) return; 

        isDead = true;  
        animator.SetTrigger("Dead");  
        rigidb.velocity = Vector2.zero;  
        if (scoreText != null)
        {
            scoreText.text = "Score: " + coinCount.ToString(); 
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

    }
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        // Quit the game build 
        Application.Quit();

        //  UnityEditor.EditorApplication.isPlaying = false;

    }

    public bool HasWeapon()
    {
        return hasWeapon;
    }
    //handles the fieball shooting mechanism, changes the fireball sprite to the direction player is shooting
    void ShootFireball()
    {
        Debug.Log("Shooting fireball in direction: " + currentDirection);
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(5f * currentDirection, 0);

        
        SpriteRenderer fireballSprite = fireball.GetComponent<SpriteRenderer>();
        if (fireballSprite)
        {
            fireballSprite.flipX = currentDirection < 0;
        }
           Destroy(fireball, 2f);
    }


    //add the coin to the coin counter and update the coin count UI on game UI
    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = " " + coinCount;
    }

    public void EquipWeapon(AnimatorOverrideController newOverride)
    {
        hasWeapon = true;
        currentOverrideController = newOverride;
        animator.runtimeAnimatorController = currentOverrideController;
    }

    //handles the player's rolling animation while moving it to the direction it is facing playng the animation for that duration
    IEnumerator TriggerRoll()
    {
        isRolling = true;
        animator.SetBool("isRolling", true);

        float rollDuration = 0.5f;
        float rollSpeed = 4f;

        float direction = currentDirection;

        float elapsedTime = 0;
        while (elapsedTime < rollDuration)
        {
            rigidb.velocity = new Vector2(rollSpeed * direction, rigidb.velocity.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isRolling", false);
        isRolling = false;
    }

    //handles the player collision check with vaious game object/ whether player is grounded or not, if tags as trap and collides, player die, if enemy is tagged and they attack, player calls take damage function etc.
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collided with: " + col.gameObject);
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
        if (col.gameObject.CompareTag("trap"))
        {
            Die();
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy attack detected!");

           
            enemy_Attack enemyAttackScript = col.gameObject.GetComponent<enemy_Attack>();
            if (enemyAttackScript != null)
            {
                playerHitpoint.TakeDamage((int)enemyAttackScript.damageToPlayer);
            }

          
            boss_Attack bossAttackScript = col.gameObject.GetComponent<boss_Attack>();
            if (bossAttackScript != null)
            {
                playerHitpoint.TakeDamage((int)bossAttackScript.damageToPlayer);
            }

        }
    }

    //ground check

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    //if it collides with a object tagged as weapon

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            //Debug.Log("Entered weapon's trigger zone.");
            nearbyWeapon = collision.GetComponent<weaponChange>();
        }

    }
    //handles player's attack on enemy 
    private void HandleWeaponAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.0f); 

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                playerAttack.HandleWeaponAttack(enemy.gameObject);
            }
        }
    }

    public void DropWeapon()
    {
        hasWeapon = false;
        animator.runtimeAnimatorController = null;
        currentOverrideController = null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            //Debug.Log("Exited weapon's trigger zone.");
            nearbyWeapon = null;
        }
    }
}