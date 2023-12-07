using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class scoreData 
{
    public string playerName;
    public int score;
}
// static class contain function to serialize and deserialize 'scoreData' objects using JSON format through Unity's JsonUtility.
public static class ScoreDataSerialization
{
    public static string SerializeScoreData(scoreData data)
    {
        return JsonUtility.ToJson(data);
    }

    public static scoreData DeserializeScoreData(string json)
    {
        return JsonUtility.FromJson<scoreData>(json);
    }
}