using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude, maxVelocity;

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

    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
}
