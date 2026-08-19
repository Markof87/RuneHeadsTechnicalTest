using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Scriptable Objects/ScoreData")]
public class ScoreData : ScriptableObject
{
    public int currentScore;
    public int highScore;
    public bool isRecord;

    public void ResetScore()
    {
        currentScore = 0;
        isRecord = false;
        LoadHighScore();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

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
