using System;
using UnityEngine;

public class LegacyCharacterController2D : MonoBehaviour
{
    [SerializeField] private bool canMove = true;

    [SerializeField] private Rigidbody2D baseRB;

    [Header("Movement Speeds")]
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float rightSpeed = 6f;
    [SerializeField] private float leftSpeed = 7f;

    [Range(0, 1f)]
    [SerializeField] private float movementSmooth = 0.05f;

    [Header("Movement Boundaries")]
    [SerializeField] private float minVertical = -5f;
    [SerializeField] private float maxVertical = 5f;
    [SerializeField] private float minHorizontal = -10f;
    [SerializeField] private float maxHorizontal = 10f;

    [Header("Jumping")]
    [SerializeField] private Rigidbody2D charRB;
    [SerializeField] private float jumpVal = 10f;
    [SerializeField] private int possibleJumps = 1;
    [SerializeField] private int currentJumps = 0;

    [SerializeField] private bool onBase = false;

    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance = 0.5f;
    [SerializeField] private LayerMask detectLayer;

    [SerializeField] private float jumpingGravityScale = 1f;
    [SerializeField] private float fallingGravityScale = 2f;

    private bool jump;

    private Vector2 inputVelocity;
    private Vector2 smoothVelocity;

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            HandleMovement();
        }
    }

    void HandleInput()
    {
        inputVelocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            inputVelocity.y = verticalSpeed;

        if (Input.GetKey(KeyCode.S))
            inputVelocity.y = -verticalSpeed;

        if (Input.GetKey(KeyCode.A))
            inputVelocity.x = -leftSpeed;

        if (Input.GetKey(KeyCode.D))
            inputVelocity.x = rightSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && (currentJumps < possibleJumps || onBase))
        {
            jump = true;
        }
    }

    void HandleMovement()
    {
        DetectBase();

        Vector2 targetVelocity = inputVelocity;

        Vector2 smoothedVelocity = Vector2.SmoothDamp(
            baseRB.velocity,
            targetVelocity,
            ref smoothVelocity,
            movementSmooth
        );

        baseRB.velocity = smoothedVelocity;

        if (onBase)
        {
            charRB.velocity = smoothedVelocity;
        }
        else
        {
            if (charRB.velocity.y < 0)
                charRB.gravityScale = fallingGravityScale;

            charRB.velocity = new Vector2(smoothedVelocity.x, charRB.velocity.y);
        }

        if (jump)
        {
            charRB.AddForce(Vector2.up * jumpVal, ForceMode2D.Impulse);
            charRB.gravityScale = jumpingGravityScale;

            jump = false;
            currentJumps++;
            onBase = false;
        }

        ClampPosition();
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

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minHorizontal, maxHorizontal);
        pos.y = Mathf.Clamp(pos.y, minVertical, maxVertical);

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Hit " + other.gameObject.name);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 topLeft = new Vector3(minHorizontal, maxVertical, 0);
        Vector3 topRight = new Vector3(maxHorizontal, maxVertical, 0);
        Vector3 bottomLeft = new Vector3(minHorizontal, minVertical, 0);
        Vector3 bottomRight = new Vector3(maxHorizontal, minVertical, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

        if (jumpDetector != null)
        {
            Gizmos.DrawRay(jumpDetector.position, Vector3.down * detectionDistance);
        }
    }
}