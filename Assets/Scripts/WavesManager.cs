using UnityEngine;
using UnityEngine.Assertions;

public class WavesManager : MonoBehaviour
{
    private int waveNumber = 1;

    private int enemiesToSpawn = 5;
    private float actualSpawnCooldown = 4f;
    private float totalSpawnCooldown = 4f;
    
    private bool finalAttack = false;
    private int finalAttackEnemies = 5;

    private bool waveRunning = false;

    private EnemySpawner enemySpawner;
    
    void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        Assert.IsNotNull(enemySpawner);
        
        startWave(1);
        startWave(4);
        startWave(12);
        startWave(16);
        startWave(26);
    }

    void Update()
    {
        if (!waveRunning)
            return;

        if (enemiesToSpawn <= 0)
        {
            waveRunning = false;
            return;
        }
        
        actualSpawnCooldown -= Time.deltaTime;

        if (actualSpawnCooldown > 0) 
            return;
        
        enemySpawner.spawnEnemies(ObjectType.EnemyNormal, 1);
        enemiesToSpawn--;
        actualSpawnCooldown = totalSpawnCooldown;
     
    }

    public void startNextWave()
    {
        startWave(waveNumber + 1);
    }

    private void startWave(int wave)
    {
        Debug.Log("Wave " + wave + " started");
        waveNumber = wave;
        
        // Enemy amount starts at 5
        enemiesToSpawn = wave + 4;
        // Spawn cooldown increments sligtly every 16 waves, decrements in the others
        totalSpawnCooldown = 4 - 0.2f * (wave - 1) + 3.2f * Mathf.Floor(wave / 16);

        if (wave % 4 == 0)
        {
            finalAttack = true;
            // Final attack enemies starts at 5, increments by 5 every 4 waves
            finalAttackEnemies = 5 * wave / 4;
        }
        else
        {
            finalAttack = false;
        }

        Debug.Log("Enemies to spawn: " + enemiesToSpawn);
        Debug.Log("Spawn cooldown: " + totalSpawnCooldown);
        Debug.Log("Final attack: " + finalAttack);
        Debug.Log("Final attack enemies: " + finalAttackEnemies);
    }
}
