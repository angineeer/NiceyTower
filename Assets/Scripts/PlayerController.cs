using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1000f;
    [SerializeField] private float jumpVelocity = 34f;
    [Range(0, 1f)] [SerializeField] private float movementSmoothingAmount = .05f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    float horizontalMove = 0f;
    const float checkGroundBoxWidth = 1f;
    const float checkGroundBoxHeight = .01f;
    private bool isGrounded;
    private bool facingRight = true;

    private Rigidbody2D rb2d;
    private Vector3 currentVelocity = Vector3.zero;

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        if (Physics2D.OverlapBox(groundCheck.position, new Vector2(checkGroundBoxWidth, checkGroundBoxHeight), 0, whatIsGround))
        {
            isGrounded = true;
            if (!wasGrounded)
                OnLandEvent.Invoke();
        }
        // Move our character
        Move(horizontalMove * Time.fixedDeltaTime);

    }


    public void Move(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move, rb2d.velocity.y);
        // And then smoothing it out and applying it to the character
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref currentVelocity, movementSmoothingAmount);

        //Flip the players facing direction accordingly
        if (move > 0 && !facingRight) Flip();
        else if (move < 0 && facingRight) Flip();

        float maxSpeed = moveSpeed * Time.fixedDeltaTime;

        if (isGrounded && rb2d.velocity.y <= 0 && rb2d.velocity.x >= maxSpeed * 0.9f)
        {
            Debug.Log("0.75 JUMP");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity * (1 + (Mathf.Abs(rb2d.velocity.x) / (maxSpeed * 0.75f))));
            isGrounded = false;
        }
        else if (isGrounded && rb2d.velocity.y <= 0 && rb2d.velocity.x >= maxSpeed * 0.2f)
        {
            Debug.Log("0.25 JUMP");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity * (1 + (Mathf.Abs(rb2d.velocity.x) / maxSpeed)));
            isGrounded = false;
        }
        else if (isGrounded && rb2d.velocity.y <= 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
            isGrounded = false;
        }

    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
