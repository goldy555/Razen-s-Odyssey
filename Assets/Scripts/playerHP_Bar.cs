using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHP_Bar : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private playerMovements playerMovementsScript;

    // Reference to the HP bar UI elements
    public Image hpBarFill; 
    public Image hpBarBackground; 

    private void Start()
    {
        currentHealth = maxHealth;
        playerMovementsScript = GetComponent<playerMovements>();
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {

        
        currentHealth -= damage;
        Debug.Log("Taking damage. Current health: " + currentHealth);
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

        // Check if the player has died
        if (currentHealth <= 0)
        {
            
            playerMovementsScript.Die();
        }
    }

   
//function for updating UI HP bar when player TakeDamage 
    private void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        hpBarFill.fillAmount = healthPercentage;

    }

   
    private void Die()
    {
        
         GetComponent<Animator>().SetTrigger("Die");
    }
}