using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    

    /* void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    } */

//collision area detection and enemy damage  
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject); //<--Destroy fireball prefab
        }
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemyHP_Bar enemyHealth = collider.gameObject.GetComponent<enemyHP_Bar>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

}
