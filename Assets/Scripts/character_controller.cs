using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_controller : MonoBehaviour
{
    // Variables for movement
    private Vector3 velocity = Vector3.zero;
    
    // Variables for min and max vertical 
    [SerializeField]private float verticalSpeed = 5f;
    [SerializeField]private float minVertical = -5f;
    [SerializeField]private float maxVertical = 5f;
    
    // Variables for min and max horizontal
    [SerializeField]private float rightSpeed = 6;
    [SerializeField]private float leftSpeed = 7;
    [SerializeField]private float minHorizontal = -10f;
    [SerializeField]private float maxHorizontal = 10f;
    
    
    // Variables for directional speed
    // Up and Down (0.5)
    // Left (0.66)
    // Right (1.0)
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        velocity = Vector3.zero;

        // Handle vertical movement (W and S keys)
        if (Input.GetKey(KeyCode.W))
        {
            velocity.y = verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.y = -verticalSpeed;
        }

        // Handle horizontal movement (A and D keys)
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -leftSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.x = rightSpeed;
        }

        // Apply movement
        transform.position += velocity * Time.deltaTime;

        // Clamp position to specified ranges
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minHorizontal, maxHorizontal);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minVertical, maxVertical);
        transform.position = clampedPosition;
    }

    // Draw gizmos in the editor to visualize movement boundaries
    void OnDrawGizmos()
    {
        // Set gizmo color to green
        Gizmos.color = Color.green;

        // Define the corners of the movement boundary rectangle
        Vector3 topLeft = new Vector3(minHorizontal, maxVertical, 0);
        Vector3 topRight = new Vector3(maxHorizontal, maxVertical, 0);
        Vector3 bottomLeft = new Vector3(minHorizontal, minVertical, 0);
        Vector3 bottomRight = new Vector3(maxHorizontal, minVertical, 0);

        // Draw the rectangle boundaries
        Gizmos.DrawLine(topLeft, topRight);           // Top
        Gizmos.DrawLine(topRight, bottomRight);       // Right
        Gizmos.DrawLine(bottomRight, bottomLeft);     // Bottom
        Gizmos.DrawLine(bottomLeft, topLeft);         // Left
    }
}
