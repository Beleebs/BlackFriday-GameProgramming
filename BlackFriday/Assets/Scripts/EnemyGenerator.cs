using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private TileManager tm;
    public GameObject enemyPrefab;
    // array of multiple spawnpoints (usually containing spawnPointAmount number of these)
    public Vector3[] spawnPoints;
    public int spawnPointAmount = 30;
    public int enemiesPerSpawn;
    public int spawnTime = 15;

    private void Start()
    {
        tm = FindObjectOfType<TileManager>();
        ApplyDifficulty();
        GenerateSpawnLocations();
        StartCoroutine(SpawnEnemies());
    }

    private void GenerateSpawnLocations()
    {
        // Holds the points to generate enemies
        spawnPoints = new Vector3[spawnPointAmount];
        // used to get bounds of places the points can be at
        Vector3 dim = tm.GetDimensions();
        for (int i = 0; i < spawnPointAmount; ++i)
        {
            // attempt to find spawnpoint based on range of area
            Vector3 randomPoint = new Vector3(Random.Range(dim.x / 2 * -1 * tm.tileSize, dim.x / 2 * tm.tileSize), Random.Range(2, dim.y * tm.wallHeight), Random.Range(tm.tileSize, dim.z / 2 * tm.tileSize));
            spawnPoints[i] = randomPoint;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        // spawns enemy
        SpawnEnemy();
        yield return new WaitForSeconds(spawnTime);
    }

    private void SpawnEnemy()
    {
        // if there are no spawn points available
        for (int i = 0; i < enemiesPerSpawn * spawnPoints.Length; ++i)
        {
            if (spawnPoints.Length == 0)
            {
                return;
            }
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }

    private void ApplyDifficulty()
    {
        if (IterationManager.Instance != null)
        {
            enemiesPerSpawn = Mathf.RoundToInt(5 + (IterationManager.Instance.difficulty / 100));
        }
    }
}
