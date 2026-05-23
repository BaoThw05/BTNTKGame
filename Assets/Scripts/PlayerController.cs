using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator animator;
    private Vector2 lastMoveDirection;
    private StaminaBar staminaBar;

    [Header("Audio Settings")]
    public float stepDelay = 0.4f;
    private float stepTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        staminaBar = GetComponent<StaminaBar>();
    }

    void Update()
    {
        if (MenuManager.isPaused) return;
        if (staminaBar != null && staminaBar.IsExhausted())
        {
            moveInput = Vector2.zero;
        }
        else
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                SoundManager.Instance.PlaySound("Walk", transform.position);
                stepTimer = stepDelay;
            }
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            lastMoveDirection = moveInput;
        }
        else
        {
            stepTimer = 0f;
        }
        animator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}