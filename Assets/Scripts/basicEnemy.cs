using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemy : MonoBehaviour
{

    public Transform pointA;
    public Transform pointB;
    public float speed = 5.0f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackDelay = 1f;

    private Transform targetPoint;
    private Animator animator;
    private Transform player;
    private bool isAttacking = false;
    private SpriteRenderer spriteRenderer;
    private enemy_Attack enemyAttackScript;
    private Collider2D attackCollider;

    private void Start()
    {
        targetPoint = pointB;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyAttackScript = GetComponent<enemy_Attack>();

        // Assuming you have named the attack collider "AttackCollider"
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider)
        {
            attackCollider.enabled = false;  // Ensure the attack collider is disabled initially
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        animator.SetBool("isRunning", true);

        bool movingRight = (targetPoint.position.x - transform.position.x) > 0;
        spriteRenderer.flipX = !movingRight;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    private void ChasePlayer()
    {
        animator.SetBool("isRunning", true);

        bool movingRight = (player.position.x - transform.position.x) > 0;
        spriteRenderer.flipX = !movingRight;

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(attackDelay / 2);

        if (attackCollider)
        {
            attackCollider.enabled = true;
        }

        yield return new WaitForSeconds(attackDelay / 2);

        if (attackCollider)
        {
            attackCollider.enabled = false;
        }

        isAttacking = false;
    }
}