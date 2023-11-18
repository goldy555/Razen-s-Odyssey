using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerMovements : MonoBehaviour
{
//variables
    private Rigidbody2D rigidb;
    private Animator animator;
    private bool hasWeapon = false;
    public RuntimeAnimatorController defaultController;
    private AnimatorOverrideController currentOverrideController;
    private bool isGrounded;
    private int jumpCount;
    private const int maxJumps = 2;
    private weaponChange nearbyWeapon;
    public Mp_Bar mpBar; 
    public GameObject fireballPrefab; 
    public Transform fireballSpawnPoint; 
    private int coinCount = 0;
    public Text coinText;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private player_Attack playerAttack;
    private playerHP_Bar playerHitpoint;
    private float currentDirection = 1f;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverCanvas;
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

//function to update/call functions depending on various conditions
    void Update()
    {
        if (isDead) return;
        
        if (Input.GetKeyDown(KeyCode.E) && nearbyWeapon != null)
        {
            nearbyWeapon.PickUp(this.gameObject);
        }
       
        float dirX = Input.GetAxisRaw("Horizontal");
        rigidb.velocity = new Vector2(dirX * 2.2f, rigidb.velocity.y);
        if (dirX > 0)
        {
            spriteRenderer.flipX = false; // Facing right
            currentDirection = 1f;
            Debug.Log("Player is facing right.");
        }
        else if (dirX < 0)
        {
            spriteRenderer.flipX = true;  // Facing left
            currentDirection = -1f;
            Debug.Log("Player is facing left.");
        }
        animator.SetBool("isRunning", Mathf.Abs(rigidb.velocity.x) > 0.1f);
        animator.SetBool("isJumping", !isGrounded);
       
        if (Input.GetKeyDown(KeyCode.V) && isGrounded)
        {
            StartCoroutine(TriggerRoll());
        }

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
        if (Input.GetKeyDown(KeyCode.B) && !mpBar.IsRegenerating())
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
   
    public void Die()
    {
        if (isDead) return;  // Return if the player is already dead

        isDead = true;  // Set the player as dead
        animator.SetTrigger("Dead");  // Play the death animation
        rigidb.velocity = Vector2.zero;  // Stop player movement
        if (scoreText != null)
        {
            scoreText.text = "Score: " + coinCount.ToString(); // Update the UI Text element with the final coin count
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
        //Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;

    }
       
    public bool HasWeapon()
    {
        return hasWeapon;
    }
//function for fireball shooting logic
    void ShootFireball()
    {
        Debug.Log("Shooting fireball in direction: " + currentDirection);
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(5f * currentDirection, 0);

        // Flip fireball sprite based on direction.
        SpriteRenderer fireballSprite = fireball.GetComponent<SpriteRenderer>();
        if (fireballSprite)
        {
            fireballSprite.flipX = currentDirection < 0;
        }
    }

    
   //function to update the coin counter and coin UI
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

    IEnumerator TriggerRoll()
    {
        animator.SetBool("isRolling", true);

        float rollDuration = 0.5f;
        float rollSpeed = 4f;
        float direction = Mathf.Sign(rigidb.velocity.x);

        if (direction == 0) direction = 1;

        float elapsedTime = 0;
        while (elapsedTime < rollDuration)
        {
            rigidb.velocity = new Vector2(rollSpeed * direction, rigidb.velocity.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isRolling", false);
    }

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

            //  if the enemy_Attack script is found on the colliding object
            if (enemyAttackScript != null)
            {
                float damageFromEnemy = enemyAttackScript.damageToPlayer;

                // if playerHitpoint is correctly referenced
                if (playerHitpoint != null)
                {
                    playerHitpoint.TakeDamage((int)damageFromEnemy);
                }
                else
                {
                    Debug.LogError("playerHitpoint reference is missing!");
                }
            }
            else
            {
                Debug.LogError("Colliding object doesn't have enemy_Attack script!");
            }
        }

        }
//collision proximity detection and the condition for various GameObject depending on the tag they are assigned to! Debug statements for debugging
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            //Debug.Log("Entered weapon's trigger zone.");
            nearbyWeapon = collision.GetComponent<weaponChange>();
        }

    }
    private void HandleWeaponAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.0f); // 1.0f is the range, adjust as needed.

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