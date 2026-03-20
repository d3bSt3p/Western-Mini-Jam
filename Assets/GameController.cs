using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] ObstacleManager obstacleManager;
    [SerializeField] CharacterController2D characterController;
    [SerializeField] ShootingController shootingController;

    [Header("UI")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameScreen;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI instructionsText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] public float startingGameSpeed = 1f;
    
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip yeehaw;
    
    [SerializeField] AudioSource playerAS;
    [SerializeField] AudioSource horseAS;
    [SerializeField] AudioClip deathSfx;
    [SerializeField] AudioClip neighSfx;
    
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
        gameScreen.SetActive(false);
        gameSpeed = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // restart after game over
        if (Time.timeScale == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
                );
            }
            return;
        }

        // wait for player input to start game
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                gameStarted = true;
                gameSpeed = startingGameSpeed;
                startScreen.SetActive(false);
                gameScreen.SetActive(true);
                audioSource.PlayOneShot(yeehaw);
            }

            return;
        }

        CalculateScore();
    }
    
    // give the player a score based on how long they have survived and the speed of the game
    private void CalculateScore()
    {
        score = Time.timeSinceLevelLoad * gameSpeed * 10;
        scoreText.text = "Score: " + Mathf.FloorToInt(score);
    }

    public void IncreaseSpeed()
    {
        gameSpeed *= 1.05f;
        obstacleManager.spawnTimeModifier *= 0.98f;
    }

    // function called to end the game and show the final score
    public void EndGame()
    {
        playerAS.PlayOneShot(deathSfx);
            
        horseAS.loop = false;
        horseAS.volume = 1f;
        horseAS.PlayOneShot(neighSfx);
        // calculate score based on time survived
        score = Time.timeSinceLevelLoad;

        Debug.Log("Game Over! Your score: " + score);

        // freeze game and stop spawning obstacles
        Time.timeScale = 0;
        obstacleManager.enabled = false;

        // disable player movement
        characterController.enabled = false;
        shootingController.enabled = false;

        // show game over screen
        gameOverScreen.SetActive(true);
        gameScreen.SetActive(false);

        // display game over text and final score
        finalScoreText.text = "Game Over! Score: " + score + "\nPress SPACE to Restart";
        
        
    }
}