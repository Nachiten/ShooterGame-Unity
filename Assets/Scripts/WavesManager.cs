using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class WavesManager : MonoBehaviour
{
    private int actualWaveNumber = 0;

    private int enemiesToSpawn = 5;
    private float actualSpawnCooldown = 4f, totalSpawnCooldown = 4f;
    
    private bool finalAttack, waveRunning, inCooldown;
    private int finalAttackEnemies = 5;

    private const float totalWaveCooldown = 10f;
    private float actualWaveCooldown = 0f;

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
    }

    void Update()
    {
        if (!waveRunning)
        {
            if (!inCooldown)
                return;

            // Run cooldown until next wave
            actualWaveCooldown -= Time.deltaTime;
            
            if (actualWaveCooldown > 0f) 
                return;
            
            // Start next wave
            inCooldown = false;
            startWave(actualWaveNumber + 1);

            return;
        }

        // No more enemies to spawn
        if (enemiesToSpawn <= 0)
        {
            Debug.Log("No more enemies in wave " + actualWaveNumber + ".");
            waveRunning = false;
            return;
        }
        
        actualSpawnCooldown -= Time.deltaTime;

        if (actualSpawnCooldown > 0) 
            return;
        
        Debug.Log("Spawning enemy");
        enemySpawner.spawnEnemies(ObjectType.EnemyNormal, 1);
        enemiesToSpawn--;
        actualSpawnCooldown = totalSpawnCooldown;
    }
    
    public void checkIfWaveIsOver()
    {
        // Check if the wave is over, check if every enemy is killed
        if (waveRunning || !enemySpawner.noEnemiesLeft())
            return;
        
        // TODO - If final attack, spawn final attack enemies

        Debug.Log("Starting cooldown for next wave");
        showWaveText("Wave " + actualWaveNumber + " ended");
        // Start cooldown until next wave
        actualWaveCooldown = totalWaveCooldown;
        inCooldown = true;
    }

    private void startWave(int wave)
    {
        Debug.Log("Wave " + wave + " started");
        showWaveText("Wave " + wave + " started");
        actualWaveNumber = wave;
        
        // Enemy amount starts at 5
        enemiesToSpawn = wave + 4;
        
        // Spawn cooldown increments sligtly every 16 waves, decrements in the others
        totalSpawnCooldown = 4 - 0.2f * (wave - 1) + 3.2f * Mathf.Floor(wave / 16);
        actualSpawnCooldown = totalSpawnCooldown;
        
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
        
        waveRunning = true;

        Debug.Log("Enemies to spawn: " + enemiesToSpawn + "\n" +
                         "Spawn cooldown: " + totalSpawnCooldown + "\n" +
                         "Final attack: " + finalAttack + "\n" +
                         "Final attack enemies: " + finalAttackEnemies + "\n");
    }

    private const float animationTime = 1f, showTime = 2f;
    
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
