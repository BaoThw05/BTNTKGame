using UnityEngine;

public class move : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public int speed = 4;
    public float h_move; //horizontal
    public float v_move; //vertical
    public bool isFacingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        h_move = Input.GetAxisRaw("Horizontal");
		rb.linearVelocity = new Vector2(speed * h_move, rb.linearVelocity.y);

        if (isFacingRight && h_move == -1)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            isFacingRight = false;
        }
        else if (!isFacingRight && h_move == 1)
        {
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
			isFacingRight = true;
		}

        v_move = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed * v_move);

		//Animation
		anim.SetFloat("move_X", Mathf.Abs(h_move));
		anim.SetFloat("move_Y", v_move);

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
	}
}
