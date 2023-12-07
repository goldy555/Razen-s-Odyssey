using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_Attack : MonoBehaviour
{
    private Collider2D attackHitbox;
    private Animator animator;
    private Transform player;
    private playerHP_Bar playerHitpoint;
    public float attackDelay = 1.0f;
    public float damageToPlayer = 20.0f;
    public float attackRange = 2.0f; 
    private bool isAttacking = false;
    private void Start()
    {
        // this tries to find the (weapon) attack collider and debug logs in case it cannot
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                attackHitbox = col;
                break;
            }
        }

        if (attackHitbox == null)
        {
            Debug.LogError("collider missing!");
        }

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            playerHitpoint = player.GetComponent<playerHP_Bar>();
        }
        else
        {
            Debug.LogError("Player missing!");
        }

        attackHitbox.enabled = false;
    }
    //start attacking coroutine in case the player is near the enemy
    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("attack");
     

        yield return new WaitForSeconds(attackDelay / 2);

        attackHitbox.enabled = true; 
        yield return new WaitForSeconds(attackDelay / 2);

        attackHitbox.enabled = false; 
        isAttacking = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy attacked!");
            playerHitpoint.TakeDamage((int)damageToPlayer);
        }
    }
}
