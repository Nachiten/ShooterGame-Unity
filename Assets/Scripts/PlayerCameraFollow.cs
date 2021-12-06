using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    private Transform target;
    private float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float mouseSenstivity = 1f;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {   
        // Follow the player
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Move position to player position
        transform.position = smoothedPosition;
        
        // rotate the camera based on the mouse x and y position
        transform.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSenstivity, 0, 0);
        transform.Rotate(0 , Input.GetAxis("Mouse X") * Time.deltaTime * mouseSenstivity, 0);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
