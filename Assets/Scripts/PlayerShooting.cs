using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShooting : MonoBehaviour
{
    private Camera thisCamera;
    
    void Start()
    {
        thisCamera = Camera.main;
        Assert.IsNotNull(thisCamera);
    }
    
    void Update()
    {
        // if the left mouse button is not pressed, return
        if (!Input.GetMouseButtonDown(0))
            return;
        
        // shoot a ray
        Ray ray = thisCamera.ScreenPointToRay(Input.mousePosition);
        
        // if raycast hits a target with tag "enemy"
        if (Physics.Raycast(ray, out var hit, 100) && hit.transform.CompareTag("Enemy"))
        {
            // log the hit
            Debug.Log("You hit an enemy!!!");
        }
        else
        {
            // Didnt hit an enemy
            Debug.Log("You did not hit an enemy.");
        }

    }
}
