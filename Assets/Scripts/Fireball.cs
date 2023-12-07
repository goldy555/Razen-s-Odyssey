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
    //fireball do damage to the enmy and get destroyed when it collides with ground
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
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
