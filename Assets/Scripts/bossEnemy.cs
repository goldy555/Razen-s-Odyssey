using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemy : MonoBehaviour
{
    // public variables
    public Transform pointA;
    public Transform pointB;
    public GameObject player;
    public GameObject monsterPrefab;
    public float moveSpeed = 2f;
    public float chaseDistance = 5f;
    public float attackRange = 1f;
    public float maxHealth = 150f;
    public float attackDelay = 1f;

    // private variables
    private enemyHP_Bar enemyHealthBar;
    private Animator animator;
    private float currentHealth = 100f;
    private bool isChasing = false;
    private bool isStunned = false;
    private Transform currentTarget;
    private float initialYPosition;
    private bool isAttacking = false;
    private boss_Attack attackScript;
    private bool hasSummoned = false;
    private float stunTimer = 20f;
    private int summonedMonsterCount = 0;
    private float refillTimer = 30f;
    private float timeSinceRefill = 0f;
    private bool isDashing = false;
    private bool isSummoning = false;
    private float stunInterval = 20f;
    private bool isGrounded = false;
    private float dashSpeed = 10f; 
    private float dashDuration = 1f; 
    private Vector3 dashDirection;
   

    void Start()
    {
        initialYPosition = transform.position.y;
        animator = GetComponent<Animator>();
        currentTarget = pointA;
        attackScript = GetComponent<boss_Attack>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealthBar = GetComponent<enemyHP_Bar>();
        StartCoroutine(StunAtIntervals());
        StartCoroutine(DashAtIntervals());
    }
    //updating different function under certain condition
    void Update()
    {
        if (currentHealth <= 0 || isStunned)
            return;
        CheckGrounded();
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (IsWithinChaseZone(transform.position) && distanceToPlayer <= chaseDistance && !isAttacking)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (!isDashing)
        {
            // Check if the boss is grounded
            CheckGrounded();

            // If grounded, follow the player
            if (isGrounded)
            {
                Vector3 playerPos = player.transform.position;
                MoveTowardsTarget(playerPos);
            }
        }

        if (isDashing)
        {
            Vector3 newPosition = transform.position + (dashDirection * dashSpeed * Time.deltaTime);
            newPosition.y = initialYPosition;
            transform.position = newPosition;


            dashDuration -= Time.deltaTime;

            if (dashDuration <= 0)
            {
                EndDash();
            }
        }
    

        if (Input.GetKeyDown(KeyCode.I) && !isSummoning)
        {
            StartCoroutine(DelayedSummon());
        }
       

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            Stun();
            stunTimer = 20f;
        }

        timeSinceRefill += Time.deltaTime;
        if (timeSinceRefill >= refillTimer)
        {
            RefillHealth();
            timeSinceRefill = 0f;
        }
    }

    private bool IsWithinChaseZone(Vector3 position)
    {
        float leftBound = Mathf.Min(pointA.position.x, pointB.position.x);
        float rightBound = Mathf.Max(pointA.position.x, pointB.position.x);

        return position.x >= leftBound && position.x <= rightBound;
    }
    //Ground check
    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        isGrounded = (hit.collider != null && hit.collider.CompareTag("Ground"));
    }
    //patrolling between two points while changing animation state to running
    private void Patrol()
    {
        
        if (!isChasing && !isAttacking)
        {
            animator.SetBool("isRunning", true);
            MoveTowardsTarget(currentTarget.position);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                currentTarget = currentTarget == pointA ? pointB : pointA;
            }
        }
    }

    private void ChasePlayer()
    {
        if (!isAttacking)
        {
            animator.SetBool("isRunning", true);
            MoveTowardsTarget(player.transform.position);
        }
    }
    //handle Dash toward player
    private void MoveTowardsTarget(Vector3 target)
    {
        bool shouldFaceRight = target.x > transform.position.x;

        
        if (isDashing)
        {
            shouldFaceRight = dashDirection.x > 0; 
        }
        transform.localScale = new Vector3(shouldFaceRight ? 0.18f : -0.18f, 0.18f, 1f);
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    //attack/running animation change while enabling and disabling the attack collider ( weapon )
    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isRunning", false);
        string attackTrigger = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";
        animator.SetTrigger(attackTrigger);
        attackScript.EnableAttackHitbox(attackTrigger);
        yield return new WaitForSeconds(attackDelay);
        attackScript.DisableAttackHitbox(attackTrigger);
        isAttacking = false;
        if (!isChasing && !isStunned)
        {
            animator.SetBool("isRunning", false);
        }
    }

  
    //stun animation change under bool condition
    private IEnumerator StunAtIntervals()
    {
        while (true)
        {
            yield return new WaitForSeconds(stunInterval);
            isStunned = true;
            animator.SetBool("isStunned", true);
            yield return new WaitForSeconds(2f);
            isStunned = false;
            animator.SetBool("isStunned", false);
        }
    }
    //dash  enabled
    IEnumerator DashAtIntervals()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Dash every 10 seconds
            if (!isDashing)
            {
                Vector3 playerPos = player.transform.position;
                StartDash(playerPos);
            }
        }
    }
    //summoning monster around the player and increasing mosnter count to make sure it if condition is false next time so it cannot summon again
    private void SummonMonsters()
    {
        if (!hasSummoned && summonedMonsterCount < 2)
        {
            hasSummoned = true;
            Vector3 playerPosition = player.transform.position;
            Instantiate(monsterPrefab, playerPosition + new Vector3(2, 0, 0), Quaternion.identity);
            Instantiate(monsterPrefab, playerPosition + new Vector3(-2, 0, 0), Quaternion.identity);
            summonedMonsterCount += 2;
        }
    }
    //setting helath full 
    public void RefillHealth()
    {
        currentHealth = maxHealth;
        enemyHealthBar.SetHealth(currentHealth);
    }
    //summon after certain time

    private IEnumerator DelayedSummon()
    {
        isSummoning = true;
        yield return new WaitForSeconds(15f);
        SummonMonsters();
        isSummoning = false;
    }
    //stunning and dash animations enabled and disabled under certain bool conditions
    public void Stun()
    {
        isStunned = true;
        animator.SetBool("isStunned", true);
    }

    public void EndStun()
    {
        isStunned = false;
        animator.SetBool("isStunned", false);
    }
    public void StartDash(Vector3 targetPosition)
    {
        isDashing = true;
        animator.SetBool("isDashing", true);
        animator.SetTrigger("Dash");
      
        dashDirection = (targetPosition - transform.position).normalized;
        animator.SetBool("isDashing", true);

        Vector3 newPosition = transform.position;
        newPosition.y = initialYPosition;
        transform.position = newPosition;

    }

    public void EndDash()
    {
      
        isDashing = false;
        animator.SetBool("isDashing", false);
        dashDuration = 1f;
    }

}

