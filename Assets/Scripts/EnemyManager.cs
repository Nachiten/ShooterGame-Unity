using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // the life of the enemy
    private float life = 4;
    
    public void takeDamage(float damage)
    {
        // lose life
        life-= damage;
        
        Debug.Log( "Recibi " + damage + " de daño");
        Debug.Log("Vida restante: " + life);
        
        if (life <= 0)
            Destroy(gameObject);
    }
}
