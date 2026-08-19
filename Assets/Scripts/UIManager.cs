using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private ScoreData scoreData;

    [SerializeField]
    private GameObject MenuPanel, GamePanel, ResultsPanel, FinishGamePanel, NewRecordText;
    [SerializeField] 
    private TextMeshProUGUI scoreText, highScoreText, timerText;

    private void OnEnable()
    {
        GameManager.OnTimerUpdated += TimerUpdate;
        GameManager.OnScoreUpdated += ScoreUpdate;
        GameManager.OnHighScoreUpdated += HighScoreUpdate;
        GameManager.OnGameEnd += FinishGameDisplay;
    }

    private void OnDisable()
    {
        GameManager.OnTimerUpdated -= TimerUpdate;
        GameManager.OnScoreUpdated -= ScoreUpdate;
        GameManager.OnHighScoreUpdated -= HighScoreUpdate;
        GameManager.OnGameEnd -= FinishGameDisplay;
    }

    private void Start()
    {
        if(scoreText != null)
            scoreText.text = "0";

        ActivatePanels();
    }

    private void ActivatePanels()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        MenuPanel.SetActive(currentScene == "Menu");
        GamePanel.SetActive(currentScene == "Game");
        ResultsPanel.SetActive(currentScene == "Results");
    }

    private void TimerUpdate(float time)
    {
        timerText.text = Mathf.RoundToInt(time).ToString();
    }

    private void ScoreUpdate()
    {
        scoreText.text = scoreData.currentScore.ToString();
    }

    private void HighScoreUpdate()
    {
        scoreData.LoadHighScore();
        highScoreText.text = scoreData.currentScore.ToString();
        NewRecordText.SetActive(scoreData.isRecord);
    }

    private void FinishGameDisplay()
    {
        FinishGamePanel.SetActive(true);
    }
}
