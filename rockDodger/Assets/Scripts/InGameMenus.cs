using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class InGameMenus : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button continueButton;
    [SerializeField] private AstroidSpawner asteroidSpawner;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] int scoreMultiplier;
    [SerializeField] GameObject endGameUIHandler;
    private string highScoreKey = "HighScore";
    private float score;
    private int highScore;
    public bool isGameOn = true;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOn) { endGameUIHandler.SetActive(true); return; }
        score += Time.deltaTime * scoreMultiplier;
        scoreText.text = Mathf.FloorToInt(score).ToString();
        if(score >= highScore) { highScore = Mathf.FloorToInt(score); PlayerPrefs.SetInt(highScoreKey, highScore); }

    }
    //InGame UI
    public void PauseGame()
    {
        //Pause the game
        if(Time.timeScale == 0) { Time.timeScale = 1; }
        else { Time.timeScale = 0; }
    }
    //EndGame UI
    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ContinueOnAD()
    {
        AdManager.Instance.ShowAd(this);
        continueButton.interactable = false;
    }

    public void GameOver()
    {
        isGameOn = false;
        asteroidSpawner.enabled = false;
    }

    public void ContinueGame()
    {
        isGameOn = true;
        asteroidSpawner.enabled = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = Vector3.zero;
        player.SetActive(true);


        endGameUIHandler.SetActive(false);
        

    }
}
