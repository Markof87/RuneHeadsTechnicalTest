using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{

    [Header("Idle Animation Clips")]
    [SerializeField] 
    private AnimationClip playerFrontIdle;
    [SerializeField] 
    private AnimationClip playerBackIdle;
    [SerializeField] 
    private AnimationClip playerSideIdle;

    [Header("Walk Animation Clips")]
    [SerializeField] 
    private AnimationClip playerFrontWalk;
    [SerializeField] 
    private AnimationClip playerBackWalk;
    [SerializeField] 
    private AnimationClip playerSideWalk;

    [SerializeField]
    private float moveSpeed = 5f;
    private bool isPlayerActive;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveInput;
    private BoxCollider2D playerCollider;
    private Vector2 lastFacingDirection = Vector2.down;
    private AnimationClip currentClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        playerCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
            //If I press some of WASD keys or arrows, player will move
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

        rb.MovePosition(rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }

    private void EnablePlayer()
    {
        isPlayerActive = true;
    }

    private void DisablePlayer()
    {
        isPlayerActive = false;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    private void UpdateAnimation()
    {
        bool isMoving = moveInput.sqrMagnitude > 0f;
        Vector2 targetDirection = isMoving ? moveInput : lastFacingDirection;

        AnimationClip targetClip = null;

        if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.y))
        {
            targetClip = isMoving ? playerSideWalk : playerSideIdle;
            spriteRenderer.flipX = targetDirection.x < 0;
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
}
