using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameObject misillePrefab;
    public GameObject[] spawnPoints;
    [SerializeField] private float forceMagnitude, maxVelocity, rotationSpeed;
    [SerializeField] TMP_Text bulletText;
    [SerializeField] int scoreMultiplier;
    private PlayerInput playerInput;

    private Camera mainCamera;
    private Rigidbody rbShip, rbBullet;
    private Vector3 movementDirection;

    private float bulletCountFilter;
    private int bulletsCount;


    void Start()
    {
        rbShip = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();

    }

    
    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotatesToFaceVelocity();

        bulletCountFilter += Time.deltaTime * scoreMultiplier;
        if (bulletCountFilter >= 10f)
        {
            bulletCountFilter = 0;
            bulletsCount++;            
        }
        bulletText.text = Mathf.FloorToInt(bulletsCount).ToString();
    }


    private void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        rbShip.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        rbShip.velocity = Vector3.ClampMagnitude(rbShip.velocity, maxVelocity);
    }
    private void ProcessInput()
    {

        if (playerInput.actions["Move"].triggered)
        {
            Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, input.y, 0);
            movementDirection = move;
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    public void FireBullet()
    {
        if(bulletsCount > 0)
        {
            bulletsCount--;
            Firing();
        }
        else
        {
            Debug.Log("NoBullet");
        }
    }

    void Firing()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            rbBullet = Instantiate(misillePrefab, spawnPoints[i].transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rbBullet.AddForce(spawnPoints[i].transform.forward * 25f, ForceMode.Impulse);
            rbBullet.velocity = Vector3.ClampMagnitude(rbBullet.velocity, maxVelocity);
        }

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
        if (rbShip.velocity == Vector3.zero) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(rbShip.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime
            );

    }
}
