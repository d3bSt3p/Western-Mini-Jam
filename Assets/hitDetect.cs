using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitDetect : MonoBehaviour
{
    [SerializeField] GameController gameController;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Hit " + other.gameObject.name);
            

            
            gameController.EndGame();
        }
    }
}
