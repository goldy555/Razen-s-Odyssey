using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_Timer : MonoBehaviour
{
    public Text timerText; 
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }
//update timer in UI 
    void Update()
    {
        float timeSinceStarted = Time.time - startTime;
        string minutes = ((int)timeSinceStarted / 60).ToString();
        string seconds = (timeSinceStarted % 60).ToString("f2"); 

        timerText.text = minutes + ":" + seconds;
    }
}
