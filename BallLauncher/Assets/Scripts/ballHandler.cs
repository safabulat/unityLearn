using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ballHandler : MonoBehaviour
{
    //Inspector Properities
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;
    //Private declerations
    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSpringJoint;
    private bool isDragging;
    private Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        SpawnNewBall();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBallRigidBody == null) {  return; }

        if (Touch.activeTouches.Count == 0) //one touch -> Touchscreen.current.primaryTouch.press.isPressed
        {
            if (isDragging)
            {
                LaunchBall();
                
            }
            isDragging = false;
            
            return;
        }

        isDragging = true;

        currentBallRigidBody.isKinematic = true;

        //For single touch - > Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        //Multi Touch;
        Vector2 touchPos = new Vector2();

        foreach(Touch touch in Touch.activeTouches)
        {
            touchPos += touch.screenPosition;
        }

        touchPos /= Touch.activeTouches.Count;

        Vector3 worldPosition = m_Camera.ScreenToWorldPoint(touchPos);
        
        currentBallRigidBody.position = worldPosition;    
        

        Debug.Log("touch pos: " + touchPos + "\tworld pos: " + worldPosition);
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;

    }

    private void LaunchBall()
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody = null;

        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
