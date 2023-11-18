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
    private bool isAttacking = false;

    private void Start()
    {
        // Find the attack collider
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
            Debug.LogError("Attack collider not found!");
        }

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            playerHitpoint = player.GetComponent<playerHP_Bar>();
        }
        else
        {
            Debug.LogError("Player reference not found!");
        }

        attackHitbox.enabled = false;
    }

    private void Update()
    {
        //  Attack when space key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
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

        attackHitbox.enabled = true; // Enable hitbox
        yield return new WaitForSeconds(attackDelay / 2);

        attackHitbox.enabled = false; // Disable hitbox
        isAttacking = false;
    }
//function to damage to player in collision area

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy attack detected!");
            playerHitpoint.TakeDamage((int)damageToPlayer);
        }
    }
}
