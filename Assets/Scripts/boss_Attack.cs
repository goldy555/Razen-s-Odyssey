using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_Attack : MonoBehaviour
{
  
    [SerializeField] private Collider2D attackHitbox1;
    [SerializeField] private Collider2D attackHitbox2;
    public float attackDelay = 1.0f;
    public float damageToPlayer = 20.0f;
    public float attackTriggerDistance = 3.0f;
    public bossEnemy boss;

    private Animator animator;
    private bool isAttacking = false;
    private Transform playerTransform;
    private playerHP_Bar playerHitpoint;
    private Vector3 initialPosition;

  //boss's initial conditions and missing component checks
    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                if (attackHitbox1 == null)
                {
                    attackHitbox1 = col;
                }
                else
                {
                    attackHitbox2 = col;
                    break;
                }
            }
        }

        if (attackHitbox1 == null || attackHitbox2 == null)
        {
            Debug.LogError(" hitbox missing!");
        }
        else
        {
            attackHitbox1.enabled = false;
            attackHitbox2.enabled = false;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform != null)
        {
            playerHitpoint = playerTransform.GetComponent<playerHP_Bar>();
          
        }
        else
        {
            Debug.LogError("Player missing");
        }
    }

    private void Update()
    {
        
        if (!isAttacking && playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackTriggerDistance)
            {
                TriggerAttack();
            }
        }
    
    }

    public void TriggerAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }
    // attack coroutine and enabling and disabling weapon attack collider in unity
    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isRunning", false);
        string attackTrigger = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";
        animator.SetTrigger(attackTrigger);
        yield return new WaitForSeconds(attackDelay / 2);
        Collider2D activeHitbox = attackTrigger == "Attack1" ? attackHitbox1 : attackHitbox2;
        activeHitbox.enabled = true;
        yield return new WaitForSeconds(attackDelay / 2);
        activeHitbox.enabled = false;
        isAttacking = false;
    }



    public void EnableAttackHitbox(string attackType)
    {
        if (attackType == "Attack1")
        {
            attackHitbox1.enabled = true;
        }
        else if (attackType == "Attack2")
        {
            attackHitbox2.enabled = true;
        }
    }

    public void DisableAttackHitbox(string attackType)
    {
        if (attackType == "Attack1")
        {
            attackHitbox1.enabled = false;
        }
        else if (attackType == "Attack2")
        {
            attackHitbox2.enabled = false;
        }
    }
}
