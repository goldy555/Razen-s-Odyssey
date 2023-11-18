using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
       
    }

    private void Start()
    {
        // Make sure the music plays only in the game scene
        if (SceneManager.GetActiveScene().name == "mainMenuScene") // Replace with your main game scene name
        {
            audioSource.Play();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "mainMenuScene") // Replace with your main menu scene name
        {
            Destroy(gameObject); // If we're not in the main menu, destroy the audio source.
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent any potential memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
