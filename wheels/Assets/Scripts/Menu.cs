using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonPressed(int a)
    {
        if (a == 1)
        {
            Debug.Log("Start Button Pressed");
            SceneManager.LoadScene("Scene_Game");
        }
    }

    public void QuitButtonPressed(int a)
    {
        if (a == 1)
        {
            Debug.Log("Quit Button Pressed");
            Application.Quit();
        }
    }

    
}
