using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollection : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip coinSound;
    // gets triggered when something enters the coin's trigger collider.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            audioSource.PlayOneShot(coinSound);
            other.GetComponent<playerMovements>().AddCoin(1);
            Destroy(gameObject); 
        }
    }
}
