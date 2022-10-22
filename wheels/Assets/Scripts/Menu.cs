using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject popSettings;

    [SerializeField] private TMP_Text highScoreText, maxLapsText, TargetFPSText, showFPSText, energyText;    
    [SerializeField] private Button musicBTN, sfxBTN, showFpsBTN, playBTN;

    private int maxEnergy = 5;


    private const string FPSKey = "FPSKey";
    private const string showFPSKey = "showFPS";

    private int isFPSshow;
    private int TargetFrameRate = 60;
    private int fpsValIndex = 0;

    [Header("Frame Settings")]
    int MaxRate = 9999;
    float currentFrameTime;
    void Awake()
    {
        //FPS Settings
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
        TargetFrameRate = PlayerPrefs.GetInt(FPSKey, 60);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;
        isFPSshow = PlayerPrefs.GetInt(showFPSKey, 0);
        if(isFPSshow == 1) { showFPSText.text = "ON";  }
        else { showFPSText.text = "OFF"; }

        //Reset Animations
        LeanTween.reset();
        popSettings.LeanMoveLocalY(-1900f, 0.1f);

        //Get Scores
        int highScore = PlayerPrefs.GetInt(ScoreHandler.HighScoreKey, 0);
        int maxLap = PlayerPrefs.GetInt(ScoreHandler.MaxLapKey, 0);
        highScoreText.text = "High Score: " + highScore.ToString();
        maxLapsText.text = "Max Laps: " + maxLap.ToString();
    }
    private void Start()
    {
        int energy = PlayerPrefs.GetInt("Energy", maxEnergy);
        if (energy > 0) { playBTN.interactable = true; }
        else { playBTN.interactable = false; }
    }
    private void FixedUpdate()
    {   
        if(!playBTN.interactable)
        {
            int energy = PlayerPrefs.GetInt("Energy", maxEnergy);
            if(energy > 0) { playBTN.interactable = true; }
        }
    }
    private void Update()
    {

    }
    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
    public void Play()
    {
        int energy = PlayerPrefs.GetInt("Energy", maxEnergy);
        if (1 > energy) { playBTN.interactable = false; return; }
        energy--;

        energyText.text = energy.ToString();
        PlayerPrefs.SetInt("Energy", energy);

        Debug.Log("Loading Game Scene.");
        SceneManager.LoadScene("Scene_Game");

        TargetFrameRate = PlayerPrefs.GetInt(FPSKey, 60);
        Debug.Log("TargetFrameRate: " + TargetFrameRate);
    }
    public void QuitApp()
    {
        Debug.Log("Quit.");
        Application.Quit();   
    }
    public void GoSettings()
    {
        TargetFrameRate = PlayerPrefs.GetInt(FPSKey, 60);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;

        popSettings.LeanMoveLocalY(-650f, 0.7f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true);

    }
    public void BackToMenu()
    {
        popSettings.LeanMoveLocalY(-1900f, 0.7f)
            .setEaseInBack()
            .setIgnoreTimeScale(true);

        TargetFrameRate = PlayerPrefs.GetInt(FPSKey, 60);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;
    }
    public void changeFPS()
    {
        TargetFrameRate = PlayerPrefs.GetInt(FPSKey, 60);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;
        fpsValIndex = TargetFrameRate / 30 - 2;

        fpsValIndex++;

        if (fpsValIndex >= 3)
        {
            fpsValIndex = -1;
        }
        
        switch (fpsValIndex)
        {
            case -1:
                TargetFrameRate = 30;
                break;
            case 0:
                TargetFrameRate = 60;
                break;
            case 1:
                TargetFrameRate = 90;
                break;
            case 2:
                TargetFrameRate = 120;
                break;
            default:
                TargetFrameRate = 60;
                break;
        }
        PlayerPrefs.SetInt(FPSKey, TargetFrameRate);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;
    }
    public void showFPStoggle()
    {
        if(isFPSshow == 0)
        {
            isFPSshow = 1;
            showFPSText.text = "ON";
            showFpsBTN.GetComponent<Image>().color = new Color32(121, 49, 111,255);
        }
        else
        {
            isFPSshow = 0;
            showFPSText.text = "OFF";
            showFpsBTN.GetComponent<Image>().color = new Color32(92, 16, 81, 255);
        }
        PlayerPrefs.SetInt(showFPSKey, isFPSshow);
    }

}
