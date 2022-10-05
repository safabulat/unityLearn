using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gainOverTime = 0.2f;
    [SerializeField] private float turningSpeed = 200f;

    private int SteerRotation;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(updateSpeedandGOT), 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        speed += gainOverTime * Time.deltaTime;
        transform.Rotate(0f, SteerRotation * turningSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Collision");
            SceneManager.LoadScene("Scene_MainMenu");
        }
        else
        {
            Debug.Log("Non Collision");
        }
    }

    public void SteerDirection(int rotationVal)
    {
        SteerRotation = rotationVal;
    }
    //void updateSpeedandGOT()
    //{
    //    gainOverTime += 0.3f;
    //    speed += gainOverTime;
    //}

    public void MenuButtonPressed(int a)
    {
        if(a == 1)
        {
            Debug.Log("Menu Button Pressed");
        }
    }
}
