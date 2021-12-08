using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // the life of the enemy
    private float life = 4;
    // Shooting range
    private const float shootingRange = 1.5f;
    
    private const float totalCooldown = 2f;
    private float cooldownLeft = 2f;
    private bool isInCooldown;
    
    public void takeDamage(float damage)
    {
        // lose life
        life-= damage;
        
        Debug.Log( "Recibi " + damage + " de daño");
        Debug.Log("Vida restante: " + life);
        
        if (life <= 0)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (isInCooldown)
        {
            // reduce cooldown
            cooldownLeft -= Time.deltaTime;

            // return if there is cooldown remaining
            if (cooldownLeft > 0) 
                return;
            
            // reset cooldown if it finished
            isInCooldown = false;
            cooldownLeft = totalCooldown;
            
            return;
        }
        
        if (!(Physics.Raycast(transform.position, transform.forward, out var hit, shootingRange) && hit.collider.gameObject.CompareTag("Player")))
            return;
        
        // draw the ray
        Debug.DrawRay(transform.position, transform.forward * shootingRange, Color.red, 2f);

        isInCooldown = true;
        hit.collider.gameObject.GetComponent<PlayerLifeManager>().loseLife(1);
    }
}
