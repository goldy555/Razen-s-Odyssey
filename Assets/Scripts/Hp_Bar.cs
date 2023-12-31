using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Bar : MonoBehaviour
{
    public Image fillImage; 

    private float maxHealth;
    //setting health to certain value passed
    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        SetHealth(health);
      
    }

    public void SetHealth(float health)
    {
        fillImage.fillAmount = health / maxHealth;
    }
}
