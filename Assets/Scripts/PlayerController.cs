using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        float moveX = 0;
        float moveY = 0;

        if(Keyboard.current != null) //I'm pressing some key
        {
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
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }
}
