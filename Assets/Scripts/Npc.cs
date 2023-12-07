using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //show and hide the panel depending on the player's proximity to the NPC
    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            instructionPanel.SetActive(true); 
        }
        else
        {
            instructionPanel.SetActive(false); 
        }
    }
}
