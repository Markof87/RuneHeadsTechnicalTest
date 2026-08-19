using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class for UI management (activation/deactivation of panels, updating of score and timer on HUD etc...)
public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private GameObject MenuPanel, GamePanel, ResultsPanel, FinishGamePanel, NewRecordText;
    [SerializeField] private TextMeshProUGUI scoreText, highScoreText, timerText;

    private void OnEnable()
    {
        //All the events called by Game Manager about UI changings
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

    //Activate panels based on the current scene
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

    //Activate buttons to reload game or back to main menu when the game is finished
    private void FinishGameDisplay()
    {
        FinishGamePanel.SetActive(true);
    }
}
