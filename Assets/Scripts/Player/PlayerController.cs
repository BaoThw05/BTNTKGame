using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private Animator moveAnimator;

	public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
	public Vector2 lastMoveDirection { get; private set; } = Vector2.down;

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
            moveAnimator.SetFloat("Horizontal", moveInput.x);
			moveAnimator.SetFloat("Vertical", moveInput.y);
            lastMoveDirection = moveInput;
        }
		moveAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}