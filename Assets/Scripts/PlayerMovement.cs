using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private CharacterController controller;

    private void Start()
    {
        controller = GameObject.Find("First Person Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Create player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        // Update player position
        controller.Move(movement * (speed * Time.deltaTime));
        
        // Jump with espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
}
