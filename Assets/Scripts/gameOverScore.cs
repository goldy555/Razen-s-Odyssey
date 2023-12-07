using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOverScore : MonoBehaviour
{
    public InputField playerNameInput;
    public Text scoreText;
    //handles the submission of a player's score to a PlayFab controller after validating the input.
    public void OnUploadButtonClick()
    {
        int score;
        if (int.TryParse(scoreText.text, out score))
        {
            playFabController.instance.SubmitScore(playerNameInput.text, score);
        }
        else
        {
            Debug.LogError("score format wrong!");
        }
    }
}

