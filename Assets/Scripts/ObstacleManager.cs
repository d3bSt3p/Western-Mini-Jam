using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacleStrafePrefabs;
    [SerializeField] private List<GameObject> obstaclesJumpPrefabs;
    [SerializeField] private GameController gameController;

    [Header("Values")]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    public float spawnTimeModifier = 1;

    [Header("Spawn Lanes")] 
    [SerializeField] public float laneTopHeight;
    [SerializeField] public float laneMidHeight;
    [SerializeField] public float laneBotHeight;
    
   
    
    private List<GameObject> obstacles;
    
    public float nextSpawnTime;
    public float spawnTimer;

    private void Start()
    {
        obstacles = new List<GameObject>();
        nextSpawnTime += Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void Update()
    {
        HandleObstacleSpawn();
        MoveObstacles();
        HandleDestroyObstacles();
    }

    // This gets called everytime an object is spawned to slowly ramp-up difficulty

    
    private void HandleObstacleSpawn()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= nextSpawnTime)
        {
            nextSpawnTime += Random.Range(minSpawnTime, maxSpawnTime) * spawnTimeModifier;
            
            // create new obstacle of random type
            // pick random jump obstacle
            int obsticleType = Random.Range(0, 2);
            if (obsticleType == 0)
            {
                int jumpObstacleIndex = Random.Range(0, obstaclesJumpPrefabs.Count);
                GameObject jumpPrefab = obstaclesJumpPrefabs[jumpObstacleIndex];
                Vector3 spawnPosition = new Vector3(10, laneBotHeight, 0);
                GameObject newObstacle = Instantiate(jumpPrefab, spawnPosition, Quaternion.identity, transform);
                obstacles.Add(newObstacle);
            }
            if (obsticleType == 1)
            {
                 // pick random strafe obstacle
                 int obstacleIndex = Random.Range(0, obstacleStrafePrefabs.Count);
                 GameObject prefab = obstacleStrafePrefabs[obstacleIndex];
            
                 // pick random lane
                 Vector3 spawnPosition = new Vector3(10, 0, 0);
                 int lane = Random.Range(1, 3);
                 if (lane == 1) spawnPosition.y = laneTopHeight;
                 if (lane == 2) spawnPosition.y = laneMidHeight;
                 if (lane == 3) spawnPosition.y = laneBotHeight;
            
                 GameObject newObstacle = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
                 obstacles.Add(newObstacle);
            }
            gameController.IncreaseSpeed();
        }
    }

    private void MoveObstacles()
    {
        foreach (var obstacle in obstacles)
        {
            Vector3 velocity = new Vector3(gameController.gameSpeed, 0, 0);
            obstacle.transform.position -= velocity * Time.deltaTime;
        }
    }

    // remove off-screen obstacles
    private void HandleDestroyObstacles()
    {
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = obstacles[i];

            if (obstacle.transform.position.x <= -10f)
            {
                obstacles.Remove(obstacle);
                Destroy(obstacle);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Vector3 laneTopL = new Vector3(-10, laneTopHeight, 0);
        Vector3 laneTopR = new Vector3(10, laneTopHeight, 0);
        Vector3 laneMidL = new Vector3(-10, laneMidHeight, 0);
        Vector3 laneMidR = new Vector3(10, laneMidHeight, 0);
        Vector3 laneBotL = new Vector3(-10, laneBotHeight, 0);
        Vector3 laneBotR = new Vector3(10, laneBotHeight, 0);
        
        Gizmos.DrawLine(laneTopL, laneTopR);
        Gizmos.DrawLine(laneMidL, laneMidR);
        Gizmos.DrawLine(laneBotL, laneBotR);
    }
}
