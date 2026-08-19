using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("Score Data")]
    [SerializeField] 
    private ScoreData scoreData;

    [Header("Components")]
    [SerializeField]
    private GameObject FinishGamePanel;

    [SerializeField] 
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private void OnEnable()
    {
        GameManager.OnTimerUpdated += TimerUpdate;
        GameManager.OnScoreUpdated += ScoreUpdate;
        GameManager.OnGameEnd += FinishGameDisplay;
    }

    private void OnDisable()
    {
        GameManager.OnTimerUpdated -= TimerUpdate;
        GameManager.OnScoreUpdated -= ScoreUpdate;
        GameManager.OnGameEnd -= FinishGameDisplay;
    }

    private void Start()
    {
        if(scoreText != null)
            scoreText.text = "0";
    }

    private void TimerUpdate(float time)
    {
        timerText.text = Mathf.RoundToInt(time).ToString();
    }

    private void ScoreUpdate(int score)
    {
        scoreText.text = score.ToString();
    }

    private void FinishGameDisplay()
    {
        FinishGamePanel.SetActive(true);
    }
}
