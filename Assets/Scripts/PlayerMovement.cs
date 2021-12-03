using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    
    // Update is called once per frame
    void Update()
    {
        // Create player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
       
        // Update player position
        transform.position += movement * (speed * Time.deltaTime);
        
        // Jump with espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
}
