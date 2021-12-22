using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    private const int ammoAdded = 4;

    private const float despawnCooldown = 30f;
    private float despawnTimer = 0f;

    private void OnEnable()
    {
        despawnTimer = despawnCooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the object that collided with the ammo is the player
        if (!other.gameObject.CompareTag("Player"))
            return;

        // Add the ammo to the player's ammo
        other.GetComponent<PlayerAmmoManager>().addAmmo(ammoAdded);
        
        returnAmmoToPool();
    }
    
    private void Update()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0)
            returnAmmoToPool();
    }
    
    private void returnAmmoToPool()
    {
        // Return ammo to the corresponding pool
        ObjectPoolManager.instance.returnObject(transform.parent.gameObject, ObjectType.Ammo);
    }
}
