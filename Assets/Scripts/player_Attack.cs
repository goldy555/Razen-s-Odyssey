using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Attack : MonoBehaviour
{
    public float weaponDamage = 15f;
    bool isAttacking = false;
    //damage to the enemy 
    private void AttackEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            enemyHP_Bar enemyHealth = enemy.GetComponent<enemyHP_Bar>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(weaponDamage);
            }
        }
    }

    
    public void HandleWeaponAttack(GameObject hitEnemy)
    {
        AttackEnemy(hitEnemy);
    }
}
