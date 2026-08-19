using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//The GameManager class, the core of the game loop management
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action OnGameStart, OnGameEnd, OnScoreUpdated, OnHighScoreUpdated;
    public static event Action<float> OnTimerUpdated;

    [SerializeField]private GameObject player, gem;

    [SerializeField]private ScoreData scoreData;

    [SerializeField]private float timer = 30;

    public float TimeRemaining { get; private set; }
    public bool IsGameActive { get; private set; }

    private Coroutine timerCoroutine, startGameCoroutine;
    private GameObject instantiatedPlayer, instantiatedGem;

    private void Awake()
    {
        //Singleton Pattern: it will avoid to create multiple GameManager, or destroy the only one active 
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Methods for the buttons
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

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    //End of methods for the buttons

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
        Time.timeScale = 1f; //if the game is in pause, this will restore the normal time 

        //We close the previous timer coroutine
        if(timerCoroutine  != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        //And the previous game coroutine
        if (startGameCoroutine != null)
        {
            StopCoroutine(startGameCoroutine);
            startGameCoroutine = null;
        }

        if (scene.name == "Game")
            startGameCoroutine = StartCoroutine(StartGame());
        else if (scene.name == "Results")
            OnHighScoreUpdated?.Invoke();
        else
            IsGameActive = false;
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

    //Get a point for a collected Gem and invoke an update for UI Manager
    private void HandleGemCollected()
    {
        scoreData.AddScore(1);
        OnScoreUpdated?.Invoke();
    }

    //Destroy the Player and the Gem, pause the game and show the two buttons to start a new game o back to menu (with Invoke)
    private void EndGame()
    {
        Destroy(instantiatedPlayer.gameObject);
        Destroy(instantiatedGem.gameObject);

        OnGameEnd?.Invoke();

        TimeRemaining = 0f;
        IsGameActive = false;
    }
}
