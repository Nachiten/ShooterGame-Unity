using UnityEngine;

public class EnemyColliderManager : MonoBehaviour
{
    public float damageTaken;

    public void getHit(Vector3 direction)
    {
        transform.parent.GetComponent<EnemyManager>().takeDamage(damageTaken, direction);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("There hasbeen a trigger");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("There has been a colission");
    }
}
