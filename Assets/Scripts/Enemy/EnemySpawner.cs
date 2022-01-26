using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
// ReSharper disable Unity.PreferNonAllocApi

public class EnemySpawner : MonoBehaviour
{
    private const float minPlayerDistance = 41f, spawningRadius = 6.5f;
    
    private Transform player;
    
    private readonly Vector3 spawnLimit1 = new(-100, 2, 100), spawnLimit2 = new(100, 2, -46);

    void Start()
    {
        player = GameObject.Find("First Person Player").GetComponent<Transform>();

        Assert.IsNotNull(player);
    }

    public bool noEnemiesLeft()
    {
        return ObjectPoolManager.instance.noEnemiesLeftOf(ObjectType.EnemyNormal) && 
               ObjectPoolManager.instance.noEnemiesLeftOf(ObjectType.EnemyFast) && 
               ObjectPoolManager.instance.noEnemiesLeftOf(ObjectType.EnemySlow);
    }

    public void spawnEnemies(ObjectType enemyType, int amount)
    {
        bool correctGeneration = false;
        Vector3 spawnPosition = Vector3.zero;

        while (!correctGeneration)
        {
            // Generate a random position based on spawnLimit1 and spawnLimit2
            spawnPosition = new Vector3(Random.Range(spawnLimit1.x, spawnLimit2.x),
                Random.Range(spawnLimit1.y, spawnLimit2.y), Random.Range(spawnLimit1.z, spawnLimit2.z));

            Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, spawningRadius);

            // Check if spawnposition is not inside non spawnable area
            bool noCollisions = hitColliders.All(hitCollider => !hitCollider.gameObject.CompareTag("Non-Spawnable"));

            // Check bool and distance from player
            if (noCollisions && Vector3.Distance(player.position, spawnPosition) > minPlayerDistance)
                correctGeneration = true;
        }

        // TODO | Add more possible random spawning arrangements

        Vector3[] spawnPositionsFinalPossible =
        {
            spawnPosition + new Vector3(1.56f, 0, 3.83f),
            spawnPosition + new Vector3(4.1f, 0, -1.1f),
            spawnPosition + new Vector3(-3.8f, 0, -2.67f)
        };

        List<Vector3> spawnPositionsFinal = new() {spawnPositionsFinalPossible[0]};

        if (amount > 1)
            spawnPositionsFinal.Add(spawnPositionsFinalPossible[1]);

        if (amount > 2)
            spawnPositionsFinal.Add(spawnPositionsFinalPossible[2]);

        foreach (Vector3 spawnPositionFinal in spawnPositionsFinal)
        {
            // Create an enemy on the spawn position
            Transform myEnemy = ObjectPoolManager.instance.getObject(enemyType).GetComponent<Transform>();

            myEnemy.position = spawnPositionFinal;
            myEnemy.gameObject.SetActive(true);
        }
    }
}