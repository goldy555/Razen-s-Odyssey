using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//base HP bar script for UI HP bar actions
public class Hp_Bar : MonoBehaviour
{
    public Image fillImage; 

    private float maxHealth;

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
