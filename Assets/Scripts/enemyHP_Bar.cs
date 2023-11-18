using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHP_Bar : MonoBehaviour
{
    public float maxHealth = 50f;
    public GameObject healthBarPrefab;
    private Hp_Bar healthBarInstance;

    private float currentHealth;
    private Canvas canvas;
    private Animator animator;  // <-- Animator reference for the enemy
    private bool isDead = false;  // <-- To make sure Die is called only once

    private void Start()
    {
        currentHealth = maxHealth;

        // Spawn and position health bar above enemy
        GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        healthBarInstance = bar.GetComponent<Hp_Bar>();
        healthBarInstance.SetMaxHealth(maxHealth);

        // Make sure health bar follows the enemy
        bar.transform.SetParent(transform);

        // Initialize the animator
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBarInstance.SetHealth(currentHealth);

        if (currentHealth <= 0 && !isDead)  // <-- Check for isDead here to prevent multiple calls
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;  // <-- Mark the enemy as dead
        animator.SetTrigger("Dead");  // <-- Activate the Dead trigger

         StartCoroutine(DestroyAfterDelay(3f));  // <-- Adjust the delay as needed
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
