using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Gem))]
public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnGameEnd;
    public static event Action<float> OnTimerUpdated;
    public static event Action<int> OnScoreUpdated;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Gem gem;

    [SerializeField]
    private ScoreData scoreData;

    [SerializeField]
    private float timer = 30;

    public float TimeRemaining { get; private set; }
    public bool IsGameActive { get; private set; }

    private int score = 0;

    private void Start()
    {
        StartGame();
    }

    private void OnEnable()
    {
        Gem.OnGemCollected += HandleGemCollected;
    }

    private void OnDisable()
    {
        Gem.OnGemCollected -= HandleGemCollected;
    }

    private void HandleGemCollected()
    {
        scoreData.AddScore(1);
        OnScoreUpdated?.Invoke(scoreData.currentScore);
    }

    private IEnumerator Countdown()
    {
        while(TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            OnTimerUpdated?.Invoke(TimeRemaining);
            yield return null;
        }

        EndGame();
    }

    private void StartGame()
    {
        Instantiate(player, new Vector2(0, 0), Quaternion.identity);
        Instantiate(gem, new Vector2(10, 0), Quaternion.identity);

        OnGameStart?.Invoke();

        scoreData.ResetScore();

        TimeRemaining = timer;
        IsGameActive = true;
        StartCoroutine(Countdown());
    }

    private void EndGame()
    {
        OnGameEnd?.Invoke();

        TimeRemaining = 0f;
        IsGameActive = false;
    }
}
