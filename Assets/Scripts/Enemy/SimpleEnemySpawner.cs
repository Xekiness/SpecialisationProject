using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval = 5f; // Time between spawns
    public int maxEnemiesPerWave = 10; // Maximum number of enemies to spawn per wave
    public float waveInterval = 20f; // Time between waves
    public int waveCount = 1; // Initial wave count

    private int currentEnemyCount = 0; // Counter for the number of spawned enemies

    // References to TextMeshProUGUI components
    public TextMeshProUGUI waveCountText;
    public TextMeshProUGUI enemyCountText;

    void Start()
    {
        // Start the wave spawning coroutine
        StartCoroutine(SpawnWaves());
        UpdateWaveCountText();
    }

    private IEnumerator SpawnWaves()
    {
        while (true) // Run indefinitely, or set a condition to stop if needed
        {
            // Check if the player is alive before starting a new wave
            if (!GameManager.instance.IsPlayerAlive())
            {
                yield break; // Exit the coroutine if the player is not alive
            }


            Debug.Log("Wave " + waveCount + " starting...");

            // Spawn enemies for the current wave
            yield return StartCoroutine(SpawnEnemies());

            // Wait until all enemies from the current wave are defeated
            while (currentEnemyCount > 0)
            {
                yield return null; // Wait for the next frame to check enemy count
            }

            // Wait for the interval between waves
            yield return new WaitForSeconds(waveInterval);

            // Increment wave count and difficulty if desired
            waveCount++;
            maxEnemiesPerWave += 5; // Example: Increase the number of enemies per wave

            // Update the wave count text
            UpdateWaveCountText();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        currentEnemyCount = 0;
        UpdateEnemyCountText();

        while (currentEnemyCount < maxEnemiesPerWave)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Choose a random spawn point from the array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the enemy at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Increase the enemy count
            currentEnemyCount++;
            UpdateEnemyCountText();
        }
    }

    private void UpdateWaveCountText()
    {
        if (waveCountText != null)
        {
            waveCountText.text = "Wave: " + waveCount;
        }
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies: " + currentEnemyCount + " / " + maxEnemiesPerWave;
        }
    }
}