using System.Collections;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float deathDelay = 1f;

    public void Die()
    {
        animator.SetTrigger("Death");
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
