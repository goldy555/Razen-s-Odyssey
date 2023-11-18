using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for Npc characters!
public class Npc : MonoBehaviour
{
    public float detectionRadius = 5.0f; 
    public GameObject instructionPanel; 
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        instructionPanel.SetActive(false); 
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            instructionPanel.SetActive(true); // Show the panel
        }
        else
        {
            instructionPanel.SetActive(false); // Hide the panel
        }
    }
}
