using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private List<GameObject> EnemyPrefabs;
    [SerializeField] private List<GameObject> SpawnPoints;

    [Header("Values")]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    public float spawnTimeModifier = 1;

    [Header("Debug")]
    public float nextSpawnTime;
    public float spawnTimer;

    private List<GameObject> obstacles = new List<GameObject>();

    private void Start()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void Update()
    {
        if (gameController.gameStarted)
        {
            HandleEnemySpawn();
        }
    }

    private void HandleEnemySpawn()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            spawnTimer = 0;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime) * spawnTimeModifier;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // try each spawn point once
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            GameObject spawn = SpawnPoints[Random.Range(0, SpawnPoints.Count)];

            if (!Physics2D.OverlapCircle(spawn.transform.position, 0.5f, LayerMask.GetMask("Enemy")))
            {
                GameObject newEnemy = Instantiate(
                    EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)],
                    spawn.transform.position,
                    Quaternion.identity,
                    transform
                );

                obstacles.Add(newEnemy);
                return;
            }
        }
    }
}