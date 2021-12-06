using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float speed = 8.0f;
    private const float gravity = -19.62f;

    public LayerMask groundMask;

    private Transform groundCheck;
    private const float groundDistance = 0.4f;
    private const float jumpHeight = 3.0f;
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
            finalSpeed *= 2;
        
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
