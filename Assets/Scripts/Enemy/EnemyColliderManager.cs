using UnityEngine;

public class EnemyColliderManager : MonoBehaviour
{
    public float damageTaken;

    public void getHit(Vector3 direction)
    {
        transform.parent.GetComponent<EnemyManager>().takeDamage(damageTaken, direction);
    }
}
