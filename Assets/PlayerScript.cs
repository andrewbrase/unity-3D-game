using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public float playerSpeed = 10f;
    public float playerRotationSpeed = 50f;
    public float playerJumpForce = 10f;
    public Transform playerCamera;

    private Rigidbody playerRigidBody;
    private Vector2 lastMousePosition;
    private bool isRotating = false;

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

    void processPlayerMovement() {
        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) {
            direction += playerCamera.forward;
        }
        if(Keyboard.current.dKey.isPressed) {
            direction += playerCamera.right;
        }
        if(Keyboard.current.aKey.isPressed) {
            direction += -playerCamera.right;
        }
        if(Keyboard.current.sKey.isPressed) {
            direction += -playerCamera.forward;
        }

        if (direction != Vector3.zero)
        {
            direction.Normalize();
            transform.position += direction * playerSpeed * Time.deltaTime;
        }
    }

    void processCameraRotation() {
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

    void processPlayerJump() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && playerIsGrounded())
        {
            playerRigidBody.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        }
    }

    bool playerIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    
}
