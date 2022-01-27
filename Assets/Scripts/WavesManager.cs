using System.ComponentModel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class WavesManager : MonoBehaviour
{
    // Current wave
    private int currentWave = 0;

    private int enemiesToSpawn = 5, finalAttackEnemies = 5;
    private float actualSpawnCooldown = 4f, totalSpawnCooldown = 4f, actualWaveCooldown = 0f;
    
    private bool finalAttack = true, enemiesSpawning, inCooldown;

    // Random spawning ranges
    private int fastRange, normalRange, oneRange, twoRange;

    private const float animationTime = 1f, showTime = 2f, totalWaveCooldown = 10f;
    
    private TMP_Text waveText;

    private EnemySpawner enemySpawner;
    
    void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        Assert.IsNotNull(enemySpawner);
        
        waveText = GameObject.Find("Wave Info").GetComponent<TMP_Text>();
        Assert.IsNotNull(waveText);

        waveText.alpha = 0f;
        waveText.gameObject.SetActive(false);

        actualWaveCooldown = totalWaveCooldown;
        inCooldown = true;
        
        startWave(3);
        startWave(4);
        startWave(8);
        startWave(10);
        startWave(14);
        startWave(22);
    }

    void Update()
    {
        if (!enemiesSpawning)
        {
            if (!inCooldown)
                return;

            // Run cooldown until next wave
            actualWaveCooldown -= Time.deltaTime;
            
            if (actualWaveCooldown > 0f) 
                return;
            
            // Start next wave
            inCooldown = false;
            startWave(currentWave + 1);

            return;
        }

        // No more enemies to spawn
        if (enemiesToSpawn <= 0)
        {
            Debug.Log("No more enemies in wave " + currentWave + ".");
            enemiesSpawning = false;
            return;
        }
        
        actualSpawnCooldown -= Time.deltaTime;

        if (actualSpawnCooldown > 0) 
            return;
        
        spawnEnemyCluster();

        enemiesToSpawn--;
        actualSpawnCooldown = totalSpawnCooldown;
    }

    private void spawnEnemyCluster()
    {
        // Generate random number of enemies to spawn
        int enemyAmountRandom = Random.Range(1, 101);
        int enemyAmount = 3;
        
        if (enemyAmountRandom <= oneRange)
            enemyAmount = 1;
        else if (enemyAmountRandom <= oneRange + twoRange)
            enemyAmount = 2;

        // Generate random enemy type
        int enemyTypeRandom = Random.Range(1, 101);
        ObjectType enemyType = ObjectType.EnemySlow;
        
        if (enemyTypeRandom <= fastRange)
            enemyType = ObjectType.EnemyFast;
        else if (enemyTypeRandom <= fastRange + normalRange)
            enemyType = ObjectType.EnemyNormal;

        Debug.Log("Enemy spawned of type " + enemyType + ". Amount: " + enemyAmount + ".");
        enemySpawner.spawnEnemies(enemyType, enemyAmount);
    }

    public void checkIfWaveIsOver()
    {
        // Check if the wave is over, check if every enemy is killed
        if (enemiesSpawning || !enemySpawner.noEnemiesLeft())
            return;
        
        if (finalAttack)
        {
            Debug.Log("Spawning final attack:");
            for (int i = 0; i < finalAttackEnemies; i++)
                spawnEnemyCluster();
            
            finalAttack = false;
            return;
        }
        
        Debug.Log("Starting cooldown for next wave");
        showWaveText("Wave " + currentWave + " ended");
        // Start cooldown until next wave
        actualWaveCooldown = totalWaveCooldown;
        inCooldown = true;
    }

    private void startWave(int wave)
    {
        Debug.Log("Wave " + wave + " started");
        showWaveText("Wave " + wave + " started");
        currentWave = wave;
        
        // Enemy amount starts at 5
        enemiesToSpawn = wave + 4;
        
        // Spawn cooldown increments sligtly every 16 waves, decrements in the others
        totalSpawnCooldown = 4 - 0.2f * (wave - 1) + 3.2f * Mathf.FloorToInt((float)wave / 16);
        actualSpawnCooldown = totalSpawnCooldown;

        calculateRanges(wave);

        calculateFinalAttack(wave);
        
        enemiesSpawning = true;

        Debug.Log("Enemies to spawn: " + enemiesToSpawn + "\n" +
                         "Spawn cooldown: " + totalSpawnCooldown + "\n" +
                         "Final attack: " + finalAttack + "\n" +
                         "Final attack enemies: " + finalAttackEnemies + "\n");
        
        Debug.Log("Fast range: " + fastRange + "\n" +
                         "Normal range: " + normalRange + "\n" +
                         "One range: " + oneRange + "\n" +
                         "Two range: " + twoRange + "\n");
    }

    private void calculateFinalAttack(int wave)
    {
        if (wave % 4 == 0)
        {
            finalAttack = true;
            // Final attack enemies starts at 5, increments by 5 every 4 waves
            finalAttackEnemies = 5 * wave / 4;
        }
        else
            finalAttack = false;
    }

    private void calculateRanges(int wave)
    {
        fastRange = wave <= 7 ? (wave - 1) * 5 : 35;

        normalRange = wave <= 10 ? 100 - (wave - 1) * 5 : 50;
        
        oneRange = wave <= 17 ? 95 - (wave - 1) * 5 : 10;

        twoRange = wave <= 6 ? wave * 4 : 25;
    }
    
    private void showWaveText(string text)
    {
        waveText.text = text;
        waveText.gameObject.SetActive(true);

        waveText.DOFade(1f, animationTime).onComplete += hideWaveText;
    }

    private void hideWaveText()
    {
        waveText.DOFade(0, animationTime).SetDelay(showTime).onComplete += () => waveText.gameObject.SetActive(false);
    }
}
