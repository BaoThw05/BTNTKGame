using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator animator;
    private Vector2 lastMoveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (MenuManager.isPaused) return;
        //moveInput.x = Input.GetAxisRaw("Horizontal");
        //moveInput.y = Input.GetAxisRaw("Vertical");
        //animator.SetFloat("Horizontal", moveInput.x);
        //animator.SetFloat("Vertical", moveInput.y);
        //animator.SetFloat("Speed", moveInput.sqrMagnitude);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // CHỈ CẬP NHẬT KHI ĐANG DI CHUYỂN
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            
            // Lưu lại hướng này trước khi người chơi thả phím
            lastMoveDirection = moveInput;
        }

        // Speed dùng để chuyển từ Idle sang Walk (0 là đứng yên, >0 là đi)
        animator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}