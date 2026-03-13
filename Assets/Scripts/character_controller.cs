using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_controller : MonoBehaviour
{
    // Rigidbody reference
    [SerializeField] Rigidbody2D characterRigidbody;
    [SerializeField] Transform groundDietector;
    [SerializeField] private LayerMask detectLayer;
    
    // Variables for movement
    private Vector3 velocity = Vector3.zero;
    private Vector3 desiredVelocity = Vector3.zero;
    
    // Variables for min and max vertical 
    [SerializeField]private float verticalSpeed = 5f;
    [SerializeField]private float minVertical = -5f;
    [SerializeField]private float maxVertical = 5f;
    
    // Variables for min and max horizontal
    [SerializeField]private float rightSpeed = 6;
    [SerializeField]private float leftSpeed = 7;
    [SerializeField]private float minHorizontal = -10f;
    [SerializeField]private float maxHorizontal = 10f;
    
    // Variables for movement smoothing
    [SerializeField]private float accelerationTime = 0.1f;
    [SerializeField]private float decelerationTime = 0.15f;
    
    // Variables for jumping
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private float groundCheckDistance = 0.5f;
    private bool isGrounded = false;
    
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        desiredVelocity = Vector3.zero;

        // Handle vertical movement (W and S keys)
        if (Input.GetKey(KeyCode.W))
        {
            desiredVelocity.y = verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            desiredVelocity.y = -verticalSpeed;
        }

        // Handle horizontal movement (A and D keys)
        if (Input.GetKey(KeyCode.A))
        {
            desiredVelocity.x = -leftSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            desiredVelocity.x = rightSpeed;
        }

        // Smoothly interpolate velocity based on acceleration/deceleration time
        float smoothTime = Vector3.Distance(velocity, desiredVelocity) > 0.01f ? accelerationTime : decelerationTime;
        velocity = Vector3.Lerp(velocity, desiredVelocity, Time.deltaTime / smoothTime);

        // Apply movement
        transform.position += velocity * Time.deltaTime;

        // Clamp position to specified ranges
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minHorizontal, maxHorizontal);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minVertical, maxVertical);
        transform.position = clampedPosition;
    }

    void HandleJump()
    {
        // Check if the character is grounded
        CheckGrounded();

        // Jump when space bar is pressed and character is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            characterRigidbody.velocity = new Vector2(characterRigidbody.velocity.x, 0);
            characterRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundDietector.position, -Vector2.up, groundCheckDistance, detectLayer);
        if(hit.collider != null)
        {
            isGrounded = true;
            //currentJumps = 0;
        }
        else
        {
            isGrounded = false;
        }
    
        

    }

    // Draw gizmos in the editor to visualize movement boundaries and ground check
    void OnDrawGizmos()
    {
        // Draw movement boundary rectangle
        Gizmos.color = Color.green;

        Vector3 topLeft = new Vector3(minHorizontal, maxVertical, 0);
        Vector3 topRight = new Vector3(maxHorizontal, maxVertical, 0);
        Vector3 bottomLeft = new Vector3(minHorizontal, minVertical, 0);
        Vector3 bottomRight = new Vector3(maxHorizontal, minVertical, 0);

        Gizmos.DrawLine(topLeft, topRight);           
        Gizmos.DrawLine(topRight, bottomRight);       
        Gizmos.DrawLine(bottomRight, bottomLeft);     
        Gizmos.DrawLine(bottomLeft, topLeft);

        // Draw ground check raycast visualization
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(groundDietector.position, groundDietector.position + Vector3.down * groundCheckDistance);
    }
}
