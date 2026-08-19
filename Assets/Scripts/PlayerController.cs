using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms;

//Player Controller class
public class PlayerController : MonoBehaviour
{
    //Animation clips
    [Header("Idle Animation Clips")]
    [SerializeField] private AnimationClip playerFrontIdle, playerBackIdle, playerSideIdle;

    [Header("Walk Animation Clips")]
    [SerializeField] private AnimationClip playerFrontWalk, playerBackWalk, playerSideWalk;
    [SerializeField]private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveInput, minBounds, maxBounds;
    private BoxCollider2D playerCollider;
    private AnimationClip currentClip;
    private Camera mainCamera;
    private Vector2 lastFacingDirection = Vector2.down;

    private bool isPlayerActive;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; //avoid the falling of the player

        playerCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        ComputeScreenBounds();
    }

    private void OnEnable()
    {
        GameManager.OnGameStart += EnablePlayer;
        GameManager.OnGameEnd += DisablePlayer;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= EnablePlayer;
        GameManager.OnGameEnd -= DisablePlayer;
    }

    void Update()
    {
        if (!isPlayerActive){
            moveInput = Vector2.zero;
            UpdateAnimation();
            return;
        }

        float moveX = 0;
        float moveY = 0;

        if(Keyboard.current != null) //I'm pressing some key
        {
            //If I press some of WASD keys or arrows, player will move to the corresponding direction
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveX -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveX += 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                moveY += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                moveY -= 1f;
        }

        moveInput = new Vector2(moveX, moveY).normalized; //avoiding diagonal movement faster than horizontal or vertical

        if (moveInput.sqrMagnitude > 0f)
            lastFacingDirection = moveInput;

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!isPlayerActive)
            return;

        Vector2 targetPosition = rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime);

        //Player moves to x and y positions until he reaches the bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        rb.MovePosition(targetPosition);
    }

    private void EnablePlayer()
    {
        ComputeScreenBounds();
        isPlayerActive = true;
    }

    private void DisablePlayer()
    {
        isPlayerActive = false;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    //The visual animation management for the player
    private void UpdateAnimation()
    {
        bool isMoving = moveInput.sqrMagnitude > 0f;
        Vector2 targetDirection = isMoving ? moveInput : lastFacingDirection;

        AnimationClip targetClip = null;

        if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.y))
        {
            targetClip = isMoving ? playerSideWalk : playerSideIdle;
            spriteRenderer.flipX = targetDirection.x < 0; //we can have movement on left or right side without using two different clips, it's enough to flip the sprite on x axis
        }
        else if (targetDirection.y > 0)
        {
            targetClip = isMoving ? playerBackWalk : playerBackIdle;
            spriteRenderer.flipX = false;
        }
        else
        {
            targetClip = isMoving ? playerFrontWalk : playerFrontIdle;
            spriteRenderer.flipX = false;
        }

        if (targetClip != null && targetClip != currentClip)
        {
            currentClip = targetClip;
            AnimationPlayableUtilities.PlayClip(animator, currentClip, out _);
        }
    }

    //Helper method computing the bounds of the screen depending on main camera
    private void ComputeScreenBounds()
    {
        if (mainCamera == null) 
            mainCamera = Camera.main;

        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;

        float minX = camPos.x - (screenWidth / 2f);
        float maxX = camPos.x + (screenWidth / 2f);
        float minY = camPos.y - (screenHeight / 2f);
        float maxY = camPos.y + (screenHeight / 2f);

        minBounds = new Vector2(minX, minY);
        maxBounds = new Vector2(maxX, maxY);
    }

}
