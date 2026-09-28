using UnityEngine;
// Movement code (Thanks for reviewing my code teacher :)
public class KnightInput : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform sensorGround; //  empty GameObject Ground Sensor
    [SerializeField] private Vector3 sensorSize; 
    [SerializeField] private float jumpTimeDuration; // How long you can hold the jump button to jump higher
    [SerializeField] private LayerMask layerGround; // referec of layer ground to ground sensor 
    [SerializeField] private float localGravity; // personalized gravity for player

    private Vector2 direction; 
    private float currentJumpTime; 

    private Rigidbody2D rigidbody2D; 
    private SpriteRenderer spriteRenderer; 

    void Awake()
    {
        // Grabs the components attached to player as soon as the game wakes up Awake works before Start
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Sets the character's gravity to my custom Gravity
        rigidbody2D.gravityScale = localGravity;
    }

    void Update()
    {
        // inputs placed on update because update happens before fixed update avoiding input lag
        Jump();
        Move();
    }

    void FixedUpdate()
    {
        // Physics must happen on fixed update
        OnMove();
        OnJump();
    }

    //  Move Input
    void Move()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

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
        
        rigidbody2D.linearVelocity = new Vector2(direction.x, rigidbody2D.linearVelocityY);
    }

    // Jump Input
    void Jump()
    {
        
        if (Input.GetButtonDown("Jump") && Grounded() == true)
        {
            currentJumpTime = jumpTimeDuration; 
        }
        
        else if (Input.GetButton("Jump") && currentJumpTime > 0)
        {
            currentJumpTime -= Time.deltaTime; 
        }
       
        else if (Input.GetButtonUp("Jump"))
        {
            currentJumpTime = 0; 
        }
    }

    void OnJump()
    {
        
        if (currentJumpTime > 0)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    
    public bool Grounded()
    {
       
        return Physics2D.OverlapBox(sensorGround.position, sensorSize, 0, layerGround);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(sensorGround.position, sensorSize);
    }

    public int MoveValueX()
    {
        return (int)Mathf.Abs(direction.x);
    }

    public int JumpValue()
    {
        return (int)rigidbody2D.linearVelocityY;
    }
}