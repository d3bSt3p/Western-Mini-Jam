using System;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private bool canMove = true;

    [Header("References")]
    [SerializeField] private Rigidbody2D charRB;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private Animator charAnimator;
    [SerializeField] private GameController gameController;
    [SerializeField] private AudioSource horseAS;
    [SerializeField] private AudioClip jumpSfx;
    
    private AudioSource audioSource;

    [Header("Lane Movement")]
    [SerializeField] private float laneMoveSmooth = 0.1f;

    private float laneVelocity;
    private int currentLane = 1;

    [Header("Jumping")]
    [SerializeField] private float jumpVal = 10f;
    [SerializeField] private int possibleJumps = 1;
    [SerializeField] private int currentJumps = 0;

    [SerializeField] private bool onBase = false;

    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance = 0.5f;
    [SerializeField] private LayerMask detectLayer;

    [SerializeField] private float jumpingGravityScale = 1f;
    [SerializeField] private float fallingGravityScale = 2f;

    // NEW: Hang time settings
    [Header("Hang Time")]
    [SerializeField] private float hangGravityScale = 0.3f;   
    [SerializeField] private float hangVelocityThreshold = 2f; 
    [SerializeField] private float maxHangTime = 0.6f;         

    private float hangTimer = 0f; 
    private bool isHanging = false; 
    
    private bool jump;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        charAnimator.SetFloat("Speed", Mathf.Abs(gameController.gameSpeed * 0.15f));
        if (gameController.gameStarted)
        {
            HandleInput();
        }
    }

    void FixedUpdate()
    {
        if (gameController.gameStarted)
        {
            HandleMovement();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentLane = Mathf.Clamp(currentLane + 1, 0, 2);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            currentLane = Mathf.Clamp(currentLane - 1, 0, 2);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (currentJumps < possibleJumps || onBase))
        {
            jump = true;
            audioSource.PlayOneShot(jumpSfx);
        }

        // anim and sfx
        bool jumping = currentJumps == 1;
        charAnimator.SetBool("isJumping", jumping);
        horseAS.mute = jumping;
    }

    void HandleMovement()
    {
        DetectBase();

        float targetHeight = GetLaneHeight();

        float newY = Mathf.SmoothDamp(
            transform.position.y,
            targetHeight,
            ref laneVelocity,
            laneMoveSmooth
        );

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Hang time logic — holds Space near apex while still in the air
        bool spaceHeld = Input.GetKey(KeyCode.Space);
        bool nearApex = Mathf.Abs(charRB.velocity.y) < hangVelocityThreshold;

        if (!onBase && spaceHeld && nearApex && currentJumps > 0 && hangTimer < maxHangTime)
        {
            isHanging = true;
            hangTimer += Time.fixedDeltaTime;
            charRB.gravityScale = hangGravityScale;
            if (onBase)
            {
                jump= false;
                
           
            }
        }
        else
        {
            
            // Normal gravity logic 
            if (!onBase)
            {
                isHanging = false;
                if (charRB.velocity.y < 0)
                    charRB.gravityScale = fallingGravityScale;
                else
                    charRB.gravityScale = jumpingGravityScale;
            }
        }

        // Reset hang timer when back on the ground
        if (onBase)
        {
            hangTimer = 0f;
            isHanging = false;
           
        }

        if (jump)
        {
            charRB.AddForce(Vector2.up * jumpVal, ForceMode2D.Impulse);
            charRB.gravityScale = jumpingGravityScale;

            jump = false;
            currentJumps++;
            onBase = false;
        }
    }

    float GetLaneHeight()
    {
        switch (currentLane)
        {
            case 2:
                return obstacleManager.laneTopHeight;

            case 1:
                return obstacleManager.laneMidHeight;

            default:
                return obstacleManager.laneBotHeight;
        }
    }

    void DetectBase()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            jumpDetector.position,
            Vector2.down,
            detectionDistance,
            detectLayer
        );

        if (hit.collider != null)
        {
            onBase = true;
            currentJumps = 0;
        }
        else
        {
            onBase = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (jumpDetector != null)
        {
            Gizmos.DrawRay(jumpDetector.position, Vector3.down * detectionDistance);
        }
    }
}