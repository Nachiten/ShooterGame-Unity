using UnityEngine;

public class AmmoColission : MonoBehaviour
{
    private const int ammoAdded = 4;

    private void OnTriggerEnter(Collider other)
    {
        // if the object that collided with the ammo is the player
        if (!other.gameObject.CompareTag("Player")) 
            return;
        
        // Add the ammo to the player's ammo
        other.GetComponent<PlayerAmmoManager>().addAmmo(ammoAdded);
        
        // Destroy the ammo
        Destroy(gameObject);
    }
}
