using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHP_Bar : MonoBehaviour
{
   
    private int currentHealth;
    private playerMovements playerMovementsScript;
    private bool isAttacking = false;
    private bool hasRestoredHealth = false;
   

    public int maxHealth = 100;
    public Image hpBarFill; 
    public Image hpBarBackground; 

    private void Start()
    {
        currentHealth = maxHealth;
        playerMovementsScript = GetComponent<playerMovements>();
        UpdateHealthBar();
    }
    // if player take damage, update healthbar with the new health after the damage, in case 0, player dies
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("damaged. health " + currentHealth);
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

       
        if (currentHealth <= 0)
        {
            playerMovementsScript.Die();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !hasRestoredHealth)
        {
            RestoreFullHealth();
            hasRestoredHealth = true; 
        }
    }
    //to update UI element in the unity
    private void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        hpBarFill.fillAmount = healthPercentage;
    }

    public bool IsPlayerAttacking()
    {
       
        return isAttacking;
    }

    //one time full restore health before boss fight
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

}