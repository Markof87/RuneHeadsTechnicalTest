using System;
using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public static event Action OnGemCollected;

    [SerializeField] 
    private Vector2 sizeMap = new Vector2(20f, 12f);

    [SerializeField]
    private float spawnDelay = 0.5f;

    private bool isGemActive = true;
    private Renderer gemRenderer;
    private BoxCollider2D gemCollider;

    private void Awake()
    {
        gemRenderer = GetComponent<Renderer>();
        gemCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
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
        if (isGemActive && collision.CompareTag("Player"));
        {
            OnGemCollected?.Invoke();
            StartCoroutine(SpawnGem());
        }
    }

    private IEnumerator SpawnGem()
    {
        if (isGemActive)
        {
            gemRenderer.enabled = false;
            gemCollider.enabled = false;

            if (spawnDelay > 0f)
                yield return new WaitForSeconds(spawnDelay);

            float randomX = UnityEngine.Random.Range(-sizeMap.x / 2f, sizeMap.x / 2f);
            float randomY = UnityEngine.Random.Range(-sizeMap.y / 2f, sizeMap.y / 2f);

            transform.position = new Vector3(randomX, randomY, 0f);

            gemCollider.enabled = true;
            gemRenderer.enabled = true;
        }
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
