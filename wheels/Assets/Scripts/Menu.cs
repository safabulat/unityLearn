using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject settingButton;


    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text maxLapsText;
    private void Start()
    {
        int highScore = PlayerPrefs.GetInt(ScoreHandler.HighScoreKey, 0);
        int maxLap = PlayerPrefs.GetInt(ScoreHandler.MaxLapKey, 0);

        highScoreText.text = "High Score: " + highScore.ToString();
        maxLapsText.text = "Max Laps: " + maxLap.ToString();
    }
    public void Play()
    {
        Debug.Log("Loading Game Scene.");
        SceneManager.LoadScene("Scene_Game");
    }
    public void QuitApp()
    {
        Debug.Log("Quit.");
        Application.Quit();   
    }

    public void GoSettings()
    {
        settingButton.SetActive(true);
    }


    
}
