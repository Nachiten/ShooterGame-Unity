using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShooting : MonoBehaviour
{
    private Camera thisCamera;
    public float shootingRange = 50f;
    
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
        
        // draw the ray
        Debug.DrawRay(ray.origin, ray.direction * shootingRange, Color.green, 1);
        
        // if raycast hits a target with tag "enemy"
        if (Physics.Raycast(ray, out var hit, shootingRange) && hit.transform.CompareTag("Enemy"))
        {
            hit.transform.GetComponent<EnemyColliderManager>().getHit(ray.direction);
        }
    }
}
