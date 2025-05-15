using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public float playerSpeed = 5f;
    public float playerRotationSpeed = 50f;
    public float playerJumpForce = 5f;
    public Transform playerCamera;

    private Rigidbody playerRigidBody;
    private Vector2 lastMousePosition;
    private bool isRotating = false;

    void processPlayerMovement()
    {
        
        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) {
            direction += playerCamera.forward;
        }
        if (Keyboard.current.dKey.isPressed) {
            direction += playerCamera.right;
        }
        if (Keyboard.current.aKey.isPressed) {
            direction += -playerCamera.right;
        }
        if (Keyboard.current.sKey.isPressed) {
            direction += -playerCamera.forward;
        }

        if (playerIsGrounded())
        {
            if (Keyboard.current.leftShiftKey.isPressed) 
            {
                playerSpeed = 8f;
            }
            else
            {
                playerSpeed = 5f;
            }
        }

        if (direction != Vector3.zero)
        {
            direction.Normalize();
            direction = GetProjectedMovement(direction);
            transform.position += direction * playerSpeed * Time.deltaTime;
        }
    }

    Vector3 GetProjectedMovement(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            return Vector3.ProjectOnPlane(direction, hit.normal);
        }
        return direction;
    }

    void processCameraRotation() 
    {
        if (Mouse.current.rightButton.isPressed)
        {
            if (!isRotating)
            {
                isRotating = true;
                lastMousePosition = Mouse.current.position.ReadValue();
            }

            Vector2 delta = Mouse.current.position.ReadValue() - lastMousePosition;
            lastMousePosition = Mouse.current.position.ReadValue();

            float rotationX = delta.y * playerRotationSpeed * Time.deltaTime;
            float rotationY = delta.x * playerRotationSpeed * Time.deltaTime;

            playerCamera.Rotate(Vector3.right, -rotationX);
            transform.Rotate(Vector3.up, rotationY);
        }
        else
        {
            isRotating = false;
        }
    }

    void processPlayerJump() 
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && playerIsGrounded())
        {
            playerRigidBody.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        }
    }

    bool playerIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        processPlayerMovement();
        processCameraRotation();
        processPlayerJump();
    }
    
}
