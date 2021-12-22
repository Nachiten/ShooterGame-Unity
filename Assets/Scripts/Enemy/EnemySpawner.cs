using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
// ReSharper disable Unity.PreferNonAllocApi

public class EnemySpawner : MonoBehaviour
{
    private float totalCooldown, currentCoodlown;
    private const float spawningDistance = 41;
    
    private Transform player;
    
    private readonly Vector3 spawnLimit1 = new(-100, 2, 100), spawnLimit2 = new(100, 2, -46);

    [SerializeField]
    private float randomMin = 10, 
                  randomMax = 15;

    void Start()
    {
        player = GameObject.Find("First Person Player").GetComponent<Transform>();

        Assert.IsNotNull(player);

        generateRandomCooldown();
    }

    private void generateRandomCooldown()
    {
        totalCooldown = Random.Range(randomMin, randomMax);
        currentCoodlown = totalCooldown;
    }

    private const float rangoSlow = 15, rangoNormal = 60;
    
    void Update()
    {
        // Reduce cooldown if its not finished
        if (currentCoodlown > 0)
        {
            currentCoodlown -= Time.deltaTime;
            return;
        }

        bool generacionCorrecta = false;
        Vector3 spawnPosition = new Vector3(0, 0, 0);

        while (!generacionCorrecta)
        {
            // Generate a random position based on spawnLimit1 and spawnLimit2
            spawnPosition = new Vector3(Random.Range(spawnLimit1.x, spawnLimit2.x),
                Random.Range(spawnLimit1.y, spawnLimit2.y), Random.Range(spawnLimit1.z, spawnLimit2.z));

            Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, 6.5f);

            // Check if spawnposition is not inside non spawnable area
            bool noCollisions = hitColliders.All(hitCollider => !hitCollider.gameObject.CompareTag("Non-Spawnable"));

            // Check bool and distance from player
            if (noCollisions && Vector3.Distance(player.position, spawnPosition) > spawningDistance)
                generacionCorrecta = true;
        }

        float randomRange = Random.Range(0, 100);

        var objectChosen = randomRange switch
        {
            < rangoSlow => ObjectType.EnemySlow,
            < rangoSlow + rangoNormal => ObjectType.EnemyNormal,
            _ => ObjectType.EnemyFast
        };

        // TODO | Add more possible random spawning arrangements
        
        Vector3[] spawnPositionsFinal = {
            spawnPosition + new Vector3(1.56f,0,3.83f), 
            spawnPosition + new Vector3(4.1f,0,-1.1f), 
            spawnPosition + new Vector3(-3.8f,0,-2.67f)
        };

        foreach (Vector3 spawnPositionFinal in spawnPositionsFinal)
        {
            // Create an enemy on the spawn position
            Transform myEnemy = ObjectPoolManager.instance.getObject(objectChosen).GetComponent<Transform>();

            myEnemy.position = spawnPositionFinal;
            myEnemy.gameObject.SetActive(true);
        }
        
        generateRandomCooldown();
        
    }
}