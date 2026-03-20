using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float reachThreshold = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private float deathDelay = .4f;

    private Vector3 targetPosition;
    private bool hasTarget = false;
    private bool isDying = false;

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    void Update()
    {
        if (!hasTarget || isDying) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= reachThreshold)
        {
            Die();
        }
    }
    
    

    public void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}