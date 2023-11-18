using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Make sure the music plays only in the game scene
        if (SceneManager.GetActiveScene().name == "SampleScene") // Replace with your main game scene name
        {
            audioSource.Play();
        }
    }
}
