using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] ObstacleManager obstacleManager;
    [SerializeField] CharacterController2D characterController;
    public float gameSpeed = 1;
    public float score;
    


    // Update is called once per frame
    void Update()
    {
        CalculateScore();
    }
    
    // give the player a score based on how long they have survived and the speed of the game
    private void CalculateScore()
    {
        score = Time.timeSinceLevelLoad * gameSpeed * 10;
    }
    public void IncreaseSpeed()
    {
        gameSpeed *= 1.05f;
        obstacleManager.spawnTimeModifier *= 0.98f; 
    }
    
    // fuction called to end the game and show the final score
    public void EndGame()
    {
        // calculate score based on time survived
        score = Time.timeSinceLevelLoad;
        Debug.Log("Game Over! Your score: " + score);
        // freeze game and stop spawning obstacles
         Time.timeScale = 0;
         obstacleManager.enabled = false;
         // disable player movement
         characterController.enabled = false;
         
         // show game over screen
         
         // display game over text
         // tint the screen red
         
        // show final score to player and give option to restart
    }
}
