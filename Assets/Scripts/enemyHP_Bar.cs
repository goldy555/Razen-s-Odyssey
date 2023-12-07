using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHP_Bar : MonoBehaviour
{
    public float maxHealth = 150f;
    public GameObject healthBarPrefab;
    public bool isBoss = false;
    public bossEnemy boss;
    public GameObject gameOverCanvas;

    private Hp_Bar healthBarInstance;
    private float currentHealth;
    private Animator animator;  
    private bool isDead = false;  

    private void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("CheckHealthAndRestore", 2f, 2f);
        // hp bar on top of enemy and follow the enemy
        GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        healthBarInstance = bar.GetComponent<Hp_Bar>();
        healthBarInstance.SetMaxHealth(maxHealth);

       
        bar.transform.SetParent(transform);

        
        animator = GetComponent<Animator>();
        if (isBoss)
        {
            boss= GetComponent<bossEnemy>(); 
        }

    }

    //take damage function of enemy to reduce it's health
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBarInstance.SetHealth(currentHealth);
        
        if (currentHealth <= 0 && !isDead)  
        {
            Die();
        }
    }
   
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;
        if (healthBarInstance != null)
        {
            healthBarInstance.SetHealth(currentHealth);
        }
    }
    public void Die()
    {
        isDead = true;  
        animator.SetTrigger("Dead");  // <-- Activate the Dead trigger
        if (isBoss && gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
        StartCoroutine(DestroyAfterDelay(1f));  // <-- Adjust the delay as needed
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    //to refill boss's health to max
    private void CheckHealthAndRestore()
    {
        if (isBoss && currentHealth < 30f && boss != null)
        {
            boss.RefillHealth();
            UpdateHealthBar(boss.maxHealth, boss.maxHealth); // Update the health bar
        }
    }


    internal void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
        
    }
}
