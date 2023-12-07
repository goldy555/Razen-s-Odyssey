using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLogin : MonoBehaviour
{
    //if titleid empty it sets to to the given one! then attempts to log in using a custom ID derived from the device's unique identifier, creating an account if it doesn't exist.
    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "41CAC"; 
        }
        var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged In!");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error Logging In!");
        Debug.LogError(error.GenerateErrorReport());
    }
}
