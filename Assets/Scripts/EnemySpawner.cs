using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
// ReSharper disable Unity.PreferNonAllocApi

public class EnemySpawner : MonoBehaviour
{
    public Transform enemy;
    
    private float totalCooldown, currentCoodlown;
    private const float spawningDistance = 41;
    
    private Transform player, parent;
    
    private readonly Vector3 spawnLimit1 = new Vector3(-100, 2, 100), spawnLimit2 = new Vector3(100, 2, -46);

    public float randomMin = 4, randomMax = 8;

    void Start()
    {
        player = GameObject.Find("First Person Player").GetComponent<Transform>();
        parent = GameObject.Find("Enemies").GetComponent<Transform>();
        
        Assert.IsNotNull(player);
        Assert.IsNotNull(parent);
        
        generateRandomCooldown();
    }

    private void generateRandomCooldown()
    {
        totalCooldown = Random.Range(randomMin, randomMax);
        currentCoodlown = totalCooldown;
    }

    void Update()
    {
        // Reduce cooldown if its not finished
        if (currentCoodlown > 0)
        {
            currentCoodlown -= Time.deltaTime;
            return;
        }
        
        bool generacionCorrecta = false;
        Vector3 spawnPosition = new Vector3(0,0,0);
        
        while (!generacionCorrecta)
        {
            // Generate a random position based on spawnLimit1 and spawnLimit2
            spawnPosition = new Vector3(Random.Range(spawnLimit1.x, spawnLimit2.x), Random.Range(spawnLimit1.y, spawnLimit2.y), Random.Range(spawnLimit1.z, spawnLimit2.z));
            
            Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, 1.3f);

            // Check if spawnposition is not inside non spawnable area
            bool noCollisions = hitColliders.All(hitCollider => !hitCollider.gameObject.CompareTag("Non-Spawnable"));

            // Check bool and distance from player
            if (noCollisions && Vector3.Distance(player.position, spawnPosition) > spawningDistance)
            {
                generacionCorrecta = true;
            }
        }

        // Create an enemy on the spawn position
        Transform myEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        myEnemy.parent = parent;

        generateRandomCooldown();
        
    }
}