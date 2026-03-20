using System.Collections;
using UnityEngine;

public class EnemyThrower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private ObstacleManager obstacleManager;

    [Header("Throw Timing")]
    [SerializeField] private float minThrowTime = 2f;
    [SerializeField] private float maxThrowTime = 4f;
    private float playerX = -3.41f;

   

    private void Start()
    {
        StartCoroutine(ThrowRoutine());
    }

    IEnumerator ThrowRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minThrowTime, maxThrowTime);
            yield return new WaitForSeconds(waitTime);

            // Play throwing animation
            animator.SetTrigger("Throw");

            // Small delay so the projectile spawns during animation
            yield return new WaitForSeconds(0.3f);

            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Vector3 target = new Vector3(playerX, 0, 0);
        int lane = Random.Range(1, 4);
        if (lane == 1) target.y = obstacleManager.laneTopHeight;
        if (lane == 2) target.y = obstacleManager.laneMidHeight;
        if (lane == 3) target.y = obstacleManager.laneBotHeight;
        
        Projectile p = projectile.GetComponent<Projectile>();
        p.SetTarget(target);
    }
}