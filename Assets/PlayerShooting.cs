using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShooting : MonoBehaviour
{
    // Camera gameobject
    private Camera thisCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = Camera.main;
        Assert.IsNotNull(thisCamera);
    }

    // Update is called once per frame
    void Update()
    {
        // shoot a ray
        Ray ray = thisCamera.ScreenPointToRay(Input.mousePosition);

        // if raycast hits a target with tag "enemy"
        if (Physics.Raycast(ray, out var hit, 100) && hit.transform.tag == "Enemy")
        {
            // shoot a bullet
            Debug.Log("hit");
        }
        
    }
}
