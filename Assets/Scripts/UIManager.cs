using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score Data")]
    [SerializeField] 
    private ScoreData scoreData;

    [Header("Components")]
    [SerializeField] 
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField] 
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private Button menuButton;

    private void OnEnable()
    {
        GameManager.OnTimerUpdated += TimerUpdate;
        GameManager.OnScoreUpdated += ScoreUpdate;
    }

    private void OnDisable()
    {
        GameManager.OnTimerUpdated -= TimerUpdate;
        GameManager.OnScoreUpdated -= ScoreUpdate;
    }

    private void Start()
    {
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
}
