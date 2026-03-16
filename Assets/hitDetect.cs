using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitDetect : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] AudioSource playerAS;
    [SerializeField] AudioSource horseAS;
    [SerializeField] AudioClip deathSfx;
    [SerializeField] AudioClip neighSfx;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Hit " + other.gameObject.name);
            
            playerAS.PlayOneShot(deathSfx);
            
            horseAS.loop = false;
            horseAS.volume = 1f;
            horseAS.PlayOneShot(neighSfx);
            
            gameController.EndGame();
        }
    }
}
