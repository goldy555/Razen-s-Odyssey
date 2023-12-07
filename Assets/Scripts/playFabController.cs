using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playFabController : MonoBehaviour
{
    public static playFabController instance;
    //intialize this at start 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // handle the sending of player's score data to playfab for updating it in the playerdata
    public void SubmitScore(string playerName, int playerScore)
    {
        var timestamp = System.DateTime.UtcNow.Ticks.ToString();
        scoreData scoreData = new scoreData { playerName = playerName, score = playerScore };
        string jsonData = JsonUtility.ToJson(scoreData);

        var updateUserDataRequest = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Score_" + timestamp, jsonData }
            }
        };

        PlayFabClientAPI.UpdateUserData(updateUserDataRequest, result =>
        {
            Debug.Log("User score data updated successfully.");
        }, OnError);
    }
    //handles the retrieval and organizing of the data from the playfab to display the 10 score by passing it to leaderboard ui manager
    public void GetScoreHistory(leaderboardUi uiManager)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            List<scoreData> allScores = new List<scoreData>();
            foreach (var entry in result.Data)
            {
                if (entry.Key.StartsWith("Score_"))
                {
                    try
                    {
                        scoreData score = JsonUtility.FromJson<scoreData>(entry.Value.Value);
                        allScores.Add(score);
                    }
                    catch (System.Exception e)
                    {
                       // Debug.Log(e);
                    }
                }
            }

            // Sort for high to low score
            allScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

            //  top 10 score only
            int numberOfScoresToShow = Mathf.Min(10, allScores.Count);
            List<scoreData> topScores = allScores.GetRange(0, numberOfScoresToShow);

            uiManager.OnScoreDataReceived(topScores);

        }, OnError);
    }
    //error check
    void OnError(PlayFabError error)
    {
        Debug.LogError("Error comunnicating with playfab");
        Debug.LogError(error.GenerateErrorReport());
    }
}
