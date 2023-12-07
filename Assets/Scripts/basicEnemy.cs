using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemy : MonoBehaviour
{
    //public in Unity field
    public Transform pointA;
    public Transform pointB;
    public float speed = 5.0f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackDelay = 1f;

    //private fields
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

        
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider)
        {
            attackCollider.enabled = false;  
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (IsWithinChaseZone(player.position) && distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
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

    //chase zone to check enemy within bounds
    private bool IsWithinChaseZone(Vector3 position)
    {
        float leftBound = Mathf.Min(pointA.position.x, pointB.position.x);
        float rightBound = Mathf.Max(pointA.position.x, pointB.position.x);

        return position.x >= leftBound && position.x <= rightBound;
    }
    //function to make enemy object walk within two point while changing animation to running
    private void Patrol()
    {
        if (!IsWithinChaseZone(transform.position))
        {
            Patrol();
            return;
        }
        animator.SetBool("isRunning", true);

        bool movingRight = (targetPoint.position.x - transform.position.x) > 0;
        spriteRenderer.flipX = !movingRight;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }
    //function to chase the player if it's at proximity with enemy object
    private void ChasePlayer()
    {
        animator.SetBool("isRunning", true);

        bool movingRight = (player.position.x - transform.position.x) > 0;
        spriteRenderer.flipX = !movingRight;

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    //function to trigger attack animation while disabling and enabling attack collider ( weapon one )
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