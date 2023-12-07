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
        // music of the main menu
        if (SceneManager.GetActiveScene().name == "mainMenuScene") 
        {
            audioSource.Play();
        }
    }
    //main menu sscene check
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "mainMenuScene") 
        {
            Destroy(gameObject);
        }
    }
    //to prevent any memory leak
    private void OnDestroy()
    {
       
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
