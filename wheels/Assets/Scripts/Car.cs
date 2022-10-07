using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float gainOverTime = 1f;
    [SerializeField] private float turningSpeed = 100f;

    private int SteerRotation;
    public static int lapsCounter;
    public Text speedCounterText, lapsCounterText;
    // Start is called before the first frame update
    void Start()
    {
        lapsCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //speed += gainOverTime * Time.deltaTime;
        transform.Rotate(0f, SteerRotation * turningSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        speedCounterText.text = "Speed: " + speed.ToString("F1") + "\nSteering: " + turningSpeed.ToString("F0");
    }
    private void FixedUpdate()
    {
        speed += gainOverTime * Time.deltaTime;
        turningSpeed += gainOverTime * 5 * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Collision");
            SceneManager.LoadScene("Scene_MainMenu");
        }
        else if (other.CompareTag("Finish"))
        {
            lapsCounter++;
            lapsCounterText.text = lapsCounter.ToString();
        }
        else
        {
            Debug.Log("Non Tagged as Obstacle.");
        }
    }

    public void SteerDirection(int rotationVal)
    {
        SteerRotation = rotationVal;
    }

    public void MenuButtonPressed(int a)
    {
        if(a == 1)
        {
            Debug.Log("Menu Button Pressed");
            toggleTimeScale();
        }
    }

    void toggleTimeScale()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            
        }
        else
        {
            Time.timeScale = 0;
        }
        Debug.Log("TimeScale: " + Time.timeScale);

    }
}
