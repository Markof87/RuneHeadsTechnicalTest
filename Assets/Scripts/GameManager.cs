using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Gem))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action OnGameStart;
    public static event Action OnGameEnd;
    public static event Action<float> OnTimerUpdated;
    public static event Action<int> OnScoreUpdated;

    [SerializeField]
    private PlayerController player, instantiatedPlayer;

    [SerializeField]
    private Gem gem, instantiatedGem;

    [SerializeField]
    private ScoreData scoreData;

    [SerializeField]
    private float timer = 30;

    public float TimeRemaining { get; private set; }
    public bool IsGameActive { get; private set; }

    private Coroutine timerCoroutine, startGameCoroutine;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadResults()
    {
        SceneManager.LoadScene(2);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Gem.OnGemCollected += HandleGemCollected;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Gem.OnGemCollected -= HandleGemCollected;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        if(timerCoroutine  != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (startGameCoroutine != null)
        {
            StopCoroutine(startGameCoroutine);
            startGameCoroutine = null;
        }

        if (scene.name == "Game")
            startGameCoroutine = StartCoroutine(StartGame());
        else
            IsGameActive = false;
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

    private IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();

        instantiatedPlayer = Instantiate(player, new Vector2(0, 0), Quaternion.identity);
        instantiatedGem = Instantiate(gem, new Vector2(10, 0), Quaternion.identity);

        OnGameStart?.Invoke();

        scoreData.ResetScore();

        TimeRemaining = timer;
        IsGameActive = true;
        timerCoroutine = StartCoroutine(Countdown());
    }

    private void EndGame()
    {
        Destroy(instantiatedPlayer.gameObject);
        Destroy(instantiatedGem.gameObject);

        OnGameEnd?.Invoke();

        TimeRemaining = 0f;
        IsGameActive = false;
    }
}
