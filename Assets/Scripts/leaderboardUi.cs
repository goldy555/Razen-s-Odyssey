using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leaderboardUi : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardEntryContainer;

    private void Start()
    {
        leaderboardPanel.SetActive(false);
    }
    //makes panel visible and calls playfab GetScoreHistory function to get scores
    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        playFabController.instance.GetScoreHistory(this);
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
    //display the scores on the leaderboard panel ui
    public void OnScoreDataReceived(List<scoreData> allScores)
    {

        int numberOfScoresToShow = Mathf.Min(10, allScores.Count); 

        for (int i = 0; i < numberOfScoresToShow; i++)
        {
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardEntryContainer);
            Text[] texts = newEntry.GetComponentsInChildren<Text>();
            texts[0].text = allScores[i].playerName;
            texts[1].text = allScores[i].score.ToString();
        }
    }
    // to clear the entries on board
    private void ClearLeaderboardEntries()
    {
        foreach (Transform child in leaderboardEntryContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
