using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
// ReSharper disable Unity.PreferNonAllocApi

public class AmmoSpawner : MonoBehaviour
{
    private float totalCooldown, currentCoodlown;

    private Transform player;
    
    private readonly Vector3 spawnLimit1 = new(-100, 2, 100), 
                             spawnLimit2 = new(100, 2, -46);

    [SerializeField]
    private float randomMin = 6, 
                  randomMax = 10;
    
    private const float spawningDistance = 15f;

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

        // Spawn ammo
        Transform myAmmo = ObjectPoolManager.instance.getObject(ObjectType.Ammo).GetComponent<Transform>();

        myAmmo.position = new Vector3(spawnPosition.x, -0.007f, spawnPosition.z);
        myAmmo.gameObject.SetActive(true);
        
        generateRandomCooldown();
        
    }
    
}
