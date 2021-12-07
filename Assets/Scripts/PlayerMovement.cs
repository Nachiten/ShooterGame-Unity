using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float gravity = -9.8f * 2f;
    public float jumpHeight = 2.4f;
    public float runningMultiplier = 2f;
    
    public LayerMask groundMask;

    private Transform groundCheck;
    private const float groundDistance = 0.4f;
   
    private bool isGrounded;
    
    private Vector3 velocity;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    
        // Reset y velocity if player is grounded
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        // Create player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Generate movement vector
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        float finalSpeed = speed;
        
        if (Input.GetKey(KeyCode.LeftShift))
            finalSpeed *= runningMultiplier;
        
        // Move acording to x and z velocity
        controller.Move(movement * (finalSpeed * Time.deltaTime));
        
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        
        // Add gravity to the velocity
        velocity.y += gravity * Time.deltaTime;

        // Move acording to y velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
