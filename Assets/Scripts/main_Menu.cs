using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_Menu : MonoBehaviour
{
    leaderboardUi ui;
 
    //play main game on play button clicked
    public void OnPlayButtonClicked()
    {
        // Load your game scene
        SceneManager.LoadScene("SampleScene");
    }
    //quit the application
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    //called leadboard ui showleaderboard function to display scores
    public void ShowHighscores()
    {
        ui.ShowLeaderboard();
    }
}
