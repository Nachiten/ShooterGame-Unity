using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
// ReSharper disable Unity.PreferNonAllocApi

public class EnemySpawner : MonoBehaviour
{
    //public Transform enemyFast, enemyNormal, enemySlow;
    
    private float totalCooldown, currentCoodlown;
    private const float spawningDistance = 41;
    
    private Transform player, parent;
    
    private readonly Vector3 spawnLimit1 = new Vector3(-100, 2, 100), spawnLimit2 = new Vector3(100, 2, -46);

    public float randomMin = 4, randomMax = 8;

    void Start()
    {
        player = GameObject.Find("First Person Player").GetComponent<Transform>();
        parent = GameObject.Find("Enemies").GetComponent<Transform>();
        
        /*
        // load enemyFast from "Prefabs/Enemy Fast"
        enemyFast = Resources.Load<Transform>("Prefabs/Enemy/Enemy Fast");
        // load enemyNormal from "Prefabs/Enemy Normal"
        enemyNormal = Resources.Load<Transform>("Prefabs/Enemy/Enemy Normal");
        // load enemySlow from "Prefabs/Enemy Slow"
        enemySlow = Resources.Load<Transform>("Prefabs/Enemy/Enemy Slow");

        Assert.IsNotNull(enemyFast);
        Assert.IsNotNull(enemyNormal);
        Assert.IsNotNull(enemySlow);
        */

        Assert.IsNotNull(player);
        Assert.IsNotNull(parent);
        
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
            {
                generacionCorrecta = true;
            }
        }

        EnemyType enemyChosen;
        
        float randomRange = Random.Range(0, 100);

        enemyChosen = randomRange switch
        {
            < rangoSlow => EnemyType.Slow,
            < rangoSlow + rangoNormal => EnemyType.Normal,
            _ => EnemyType.Fast
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
            Transform myEnemy = ObjectPoolManager.instance.getObject(enemyChosen).GetComponent<Transform>();

            myEnemy.position = spawnPositionFinal;
            myEnemy.parent = parent;
        }
        
        generateRandomCooldown();
        
    }
}