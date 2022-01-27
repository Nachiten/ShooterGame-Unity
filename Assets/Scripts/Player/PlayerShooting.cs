using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShooting : MonoBehaviour
{
    private float shootingRange = 30f;
    
    private const float totalCooldown = 0.25f;
    private float cooldown;

    private bool isInCooldown;
    
    private Camera thisCamera;
    private PlayerAmmoManager ammoManager;
    
    void Start()
    {
        thisCamera = Camera.main;
        Assert.IsNotNull(thisCamera);
        
        ammoManager = GetComponent<PlayerAmmoManager>();
        Assert.IsNotNull(ammoManager);
    }
    
    void Update()
    {
        // If the R key is pressed, reload
        if (Input.GetKeyDown(KeyCode.R))
            ammoManager.reload();
        
        // Cooldown of totalCooldown seconds before shooting again
        if (isInCooldown)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
                isInCooldown = false;
            
            return;
        }

        // if the left mouse button is not pressed or there is no ammo, return
        if (!Input.GetMouseButtonDown(0) || !ammoManager.canShoot())
            return;
        
        ammoManager.shoot();
        
        // shoot a ray
        Ray ray = thisCamera.ScreenPointToRay(Input.mousePosition);
        
        // draw the ray
        Debug.DrawRay(ray.origin, ray.direction * shootingRange, Color.green, 1);
        
        // if raycast hits a target with tag "enemy"
        if (!Physics.Raycast(ray, out var hit, shootingRange) || !hit.transform.CompareTag("Enemy")) 
            return;
        
        hit.transform.GetComponent<EnemyColliderManager>().getHit(ray.direction);
        isInCooldown = true;
        cooldown = totalCooldown;
    }
}
