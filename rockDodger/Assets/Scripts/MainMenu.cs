using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string highScoreKey = "HighScore";
    [SerializeField] TMP_Text highScoreText;
    private int highScore;
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = "HighScore: " + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SettingsButton()
    {
        //GoSettings
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
