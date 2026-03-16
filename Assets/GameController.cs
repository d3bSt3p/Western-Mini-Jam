using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] ObstacleManager obstacleManager;
    [SerializeField] CharacterController2D characterController;

    [Header("UI")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text instructionsText;
    [SerializeField] Text finalScoreText;
    [SerializeField] public float startingGameSpeed = 1f;
    
    public float gameSpeed = 0;
    
    public float score;

    public bool gameStarted = false;

    // start with the game frozen and show a start screen
    void Start()
    {
        
        // show start screen
        startScreen.SetActive(true);

        // display instructions to player
        instructionsText.text = "Press SPACE or CLICK to start";

        // hide game over screen initially
        gameOverScreen.SetActive(false);
        gameSpeed = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // wait for player input to start game
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                gameStarted = true;
                gameSpeed = startingGameSpeed;
            }

            return;
        }

        CalculateScore();
    }

    void StartGame()
    {
        gameStarted = true;

        // hide start screen
        startScreen.SetActive(false);

        // unfreeze game
        Time.timeScale = 1;
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

    // function called to end the game and show the final score
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
        gameOverScreen.SetActive(true);

        // display game over text and final score
        finalScoreText.text = "Game Over!\nScore: " + Mathf.FloorToInt(score);
        
    }
}