using UnityEngine;
using UnityEngine.Assertions;

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
    
    private PlayerLifeManager lifeManager;
    
    private void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        controller = GetComponent<CharacterController>();
        lifeManager = GetComponent<PlayerLifeManager>();
        
        Assert.IsNotNull(groundCheck);
        Assert.IsNotNull(controller);
        Assert.IsNotNull(lifeManager);
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
        
        // If player presses shift, is grounded and has stamina, run
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && lifeManager.canRun())
        {
            finalSpeed *= runningMultiplier;
            lifeManager.setIsRunning(true);
        }
        
        // If player presses up shift, stop running
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            lifeManager.setIsRunning(false);
        }
        
        // Move acording to x and z velocity
        controller.Move(movement * (finalSpeed * Time.deltaTime));
        
        // Check if player is jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        
        // Add gravity to the velocity
        velocity.y += gravity * Time.deltaTime;

        // Move acording to y velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
