using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude, maxVelocity, rotationSpeed;

    private Camera mainCamera;
    private Rigidbody rb;
    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    
    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotatesToFaceVelocity();
    }


    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPos = mainCamera.ScreenToViewportPoint(touchPos);

            Debug.Log("touchPos: " + touchPos + "\nworldPos: " + worldPos);

            movementDirection = transform.position - worldPos;
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero;
        }
        //Debug.Log("shipPos: " + transform.position);
    }
    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(transform.position);

        if(viewPortPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if(viewPortPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        else if (viewPortPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if (viewPortPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;

    }

    private void RotatesToFaceVelocity()
    {
        if (rb.velocity == Vector3.zero) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime
            );

    }
}
