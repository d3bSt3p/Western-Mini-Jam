using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] topHalves;
    [SerializeField] private Transform[] bottomHalves;
    [SerializeField] private GameController gameController;

    [Header("Scroll speeds")] 
    public float topSpeed;
    public float bottomSpeed;

    private void Update()
    {
        if (gameController.gameStarted)
        {
            bottomSpeed = gameController.gameSpeed;
        
            foreach (Transform th in topHalves)
            {
                th.transform.position += Vector3.left * Time.deltaTime * topSpeed;

                if (th.transform.position.x < -18.1f)
                {
                    th.transform.position = new Vector3(18.1f, 0f, 0f);
                }
            }
        
            foreach (Transform bh in bottomHalves)
            {
                bh.transform.position += Vector3.left * Time.deltaTime * bottomSpeed;
            
                if (bh.transform.position.x < -18.1f)
                {
                    bh.transform.position = new Vector3(18.1f, 0f, 0f);
                }
            }
        }

    }
}
