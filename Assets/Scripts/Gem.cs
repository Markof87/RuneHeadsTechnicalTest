using System;
using System.Collections;
using UnityEngine;

//Class for Gem element and its behaviour
public class Gem : MonoBehaviour
{
    public static event Action OnGemCollected;

    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private float screenMarginPercentage = 0.85f;
    [SerializeField] private float minDistanceFromPlayer = 3f;

    private bool isGemActive = true;
    private bool isGemCollected = false;
    private Renderer gemRenderer;
    private BoxCollider2D gemCollider;
    private Camera mainCamera;
    private Transform playerTransform;

    private void Awake()
    {
        gemRenderer = GetComponent<Renderer>();
        gemCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnGem());
    }

    private void OnEnable()
    {
        GameManager.OnGameStart += EnableGem;
        GameManager.OnGameEnd += DisableGem;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= EnableGem;
        GameManager.OnGameEnd -= DisableGem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGemCollected || !isGemActive) 
            return;

        if (collision.CompareTag("Player"))
        {
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position, 1.0f);

            isGemCollected = true;

            if(gemCollider != null && gemRenderer != null)
            {
                gemCollider.enabled = false;
                gemRenderer.enabled = false;
            }

            OnGemCollected?.Invoke();
            StartCoroutine(SpawnGem());
        }
    }

    //Spawn the Gem when it is collected, in a certain distance from the Player
    private IEnumerator SpawnGem()
    {
        if (!isGemActive) yield break;

        if (gemCollider != null && gemRenderer != null)
        {
            gemCollider.enabled = false;
            gemRenderer.enabled = false;
        }

        if (spawnDelay > 0f)
            yield return new WaitForSeconds(spawnDelay);

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

        Vector2 mapSize = GetScreenBounds();
        Vector3 newSpawnPosition = Vector3.zero;

        //The system will try a maximum amout of 100 attempts
        int maxAttempts = 100;
        int attempts = 0;
        bool validPositionFound = false;

        //The loop could stop if we found a good position to spawn the Gem
        while (!validPositionFound && attempts < maxAttempts)
        {
            attempts++;

            float randomX = UnityEngine.Random.Range(-mapSize.x / 2f, mapSize.x / 2f);
            float randomY = UnityEngine.Random.Range(-mapSize.y / 2f, mapSize.y / 2f);
            newSpawnPosition = new Vector3(randomX, randomY, 0f);

            //If there is an active Player, we find if there is enough distance between him and the new position of the gem that is spawning
            if (playerTransform != null)
            {
                float distance = Vector2.Distance(newSpawnPosition, playerTransform.position);
                if (distance >= minDistanceFromPlayer)
                    validPositionFound = true;
            }
            else
                validPositionFound = true;
        }

        transform.position = newSpawnPosition;
        Physics2D.SyncTransforms();

        isGemCollected = false;
        if (gemCollider != null && gemRenderer != null)
        {
            gemCollider.enabled = true;
            gemRenderer.enabled = true;
        }
    }

    //Helper method computing the bounds of the screen depending on main camera
    private Vector2 GetScreenBounds()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        //Calculate the bounds of the camera size subtracting a margin to avoid a gem spawning out of the screen
        return new Vector2(screenWidth * screenMarginPercentage, screenHeight * screenMarginPercentage);
    }

    private void EnableGem()
    {
        isGemActive = true;
    }

    private void DisableGem()
    {
        isGemActive = false;
    }
}
