using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Scriptable Objects/ScoreData")]

//Scriptable Object managing current and highest score
public class ScoreData : ScriptableObject
{
    public int currentScore, highScore;
    public bool isRecord;

    //Reset score and load the highest score from player prefs
    public void ResetScore()
    {
        currentScore = 0;
        isRecord = false;
        LoadHighScore();
    }

    //Add score and save on player prefs if we beat the record
    public void AddScore(int amount)
    {
        currentScore += amount;

        //if the current score becomes greater than highest score, then save it on player prefs
        if(currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            isRecord = true;
        }
    }

    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
