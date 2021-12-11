using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform enemy;
    
    private Transform player;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        // load enemy from Prefabs/Enemy
        enemy = (Resources.Load("Prefabs/Enemy") as GameObject)?.transform;
        player = GameObject.Find("First Person Player").GetComponent<Transform>();
        parent = GameObject.Find("Enemies").GetComponent<Transform>();
    }

    private float currentCoodlown;
    private const float totalCooldown = 0.1f;
    
    private readonly Vector3 spawnLimit1 = new Vector3(-100,2,100), 
                             spawnLimit2 = new Vector3(100,2,-46);

    // Spawning distance 75
    private const float spawningDistance = 75;
    
    // Update is called once per frame
    void Update()
    {
        // create an enemy every "totalCooldown" seconds
        if (currentCoodlown <= 0)
        {
            bool generacionCorrecta = false;
            Vector3 spawnPosition = new Vector3(0,0,0);
            
            while (!generacionCorrecta)
            {
                // Generate a random position based on spawnLimit1 and spawnLimit2
                spawnPosition = new Vector3(Random.Range(spawnLimit1.x, spawnLimit2.x), Random.Range(spawnLimit1.y, spawnLimit2.y), Random.Range(spawnLimit1.z, spawnLimit2.z));
                
                // Check if spawn is not too near to player
                if (Vector3.Distance(player.position, spawnPosition) > spawningDistance)
                    generacionCorrecta = true;
            }

            // Create an enemy on the spawn position
            Transform myEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
            myEnemy.parent = parent;
            
            currentCoodlown = totalCooldown;
        }
        else
        {
            currentCoodlown -= Time.deltaTime;
        }
    }
}
