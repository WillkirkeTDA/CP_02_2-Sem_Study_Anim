using UnityEngine;

public class KnightInput : MonoBehaviour
{
    // [SerializeField] lets you edit these private variables directly in the Unity Inspector
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform sensorGround; // The empty GameObject placed at the character's feet
    [SerializeField] private Vector3 sensorSize; // The width and height of the ground detection box
    [SerializeField] private float jumpTimeDuration; // How long you can hold the jump button to jump higher
    [SerializeField] private LayerMask layerGround; // Tells the game which layer is the floor (e.g., your Ground layer)
    [SerializeField] private float localGravity; // The default gravity scale for the knight

    private Vector2 direction; // Stores the X and Y input from the player's keyboard/controller
    private float currentJumpTime; // Tracks how much "higher jump" time is left while holding the button

    private Rigidbody2D rigidbody2D; // The physics engine component
    private SpriteRenderer spriteRenderer; // The component that draws the 2D image

    void Awake()
    {
        // Grabs the components attached to your character as soon as the game wakes up
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Sets the character's gravity to your custom value right when the level starts
        rigidbody2D.gravityScale = localGravity;
    }

    void Update()
    {
        // Update runs every single frame. It is the best place to read player button presses.
        Jump();
        Move();
    }

    void FixedUpdate()
    {
        // FixedUpdate runs at a fixed rate in sync with Unity's physics engine.
        // You MUST apply movement and forces to the Rigidbody2D here, not in Update.
        OnMove();
        OnJump();
    }

    // ******** MOVE ***********
    void Move()
    {
        // Input.GetAxisRaw returns -1 (left), 0 (still), or 1 (right). We multiply that by your moveSpeed.
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        // Flips the character's image left or right depending on which way they are walking
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnMove()
    {
        // Applies the horizontal movement to the physics body. 
        // We keep the current Y velocity (falling/jumping) exactly as it is so we don't mess up gravity.
        rigidbody2D.linearVelocity = new Vector2(direction.x, rigidbody2D.linearVelocityY);
    }

    // ******** JUMP ***********
    void Jump()
    {
        // If the player presses the jump button AND the character's feet are touching the ground
        if (Input.GetButtonDown("Jump") && Grounded() == true)
        {
            currentJumpTime = jumpTimeDuration; // Start the timer for a higher jump
        }
        // If the player is still holding the jump button and there is still time left on the timer
        else if (Input.GetButton("Jump") && currentJumpTime > 0)
        {
            currentJumpTime -= Time.deltaTime; // Drain the timer
        }
        // If the player lets go of the jump button early
        else if (Input.GetButtonUp("Jump"))
        {
            currentJumpTime = 0; // Kill the timer so they start falling
        }
    }

    void OnJump()
    {
        // As long as the jump timer has time left, keep pushing the character upward
        if (currentJumpTime > 0)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // ******** DETECTION ***********
    public bool Grounded()
    {
        // Draws an invisible box at 'sensorGround'. If it touches the 'layerGround', it returns true.
        return Physics2D.OverlapBox(sensorGround.position, sensorSize, 0, layerGround);
    }

    void OnDrawGizmos()
    {
        // Draws a yellow box in the Unity Editor so you can visually see and resize your ground sensor
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(sensorGround.position, sensorSize);
    }

    // ******** ANIMATION VALUES ***********
    // These methods just pass data to your Animator script so it knows what to animate
    public int MoveValueX()
    {
        // Returns the absolute value (always positive) of the movement.
        // If walking left (-5) or right (5), this returns 5. If standing still, it returns 0.
        return (int)Mathf.Abs(direction.x);
    }

    public int JumpValue()
    {
        return (int)rigidbody2D.linearVelocityY;
    }
}