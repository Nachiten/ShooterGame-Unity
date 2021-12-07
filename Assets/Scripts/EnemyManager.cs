using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // the life of the enemy
    private float life = 4;

    private float totalCooldown = 2f;
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
            cooldownLeft -= Time.deltaTime;

            if (!(cooldownLeft <= 0)) 
                return;
            
            isInCooldown = false;
            cooldownLeft = totalCooldown;

            return;
        }
        
        RaycastHit hit;

        float rayDistance = 1.5f;

        bool rayHit = Physics.Raycast(transform.position, transform.forward, out hit, rayDistance);
        
        if (!(rayHit && hit.collider.gameObject.CompareTag("Player")))
            return;
        
        // draw the ray
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red, 2f);

        isInCooldown = true;
        Debug.Log("Enemy detecta al jugador");
        hit.collider.gameObject.GetComponent<PlayerLifeManager>().loseLife();
    }
}
