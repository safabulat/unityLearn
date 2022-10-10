using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System;

public class Menu : MonoBehaviour
{
    public GameObject settingButton;

    [SerializeField] private TMP_Text highScoreText, maxLapsText, TargetFPSText, energyText;
    [SerializeField] private int maxEnergy, energyChargeDuration;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private float TargetFrameRate = 60.0f;
    private int fpsVal = 0;
    [Header("Frame Settings")]
    int MaxRate = 9999;
    float currentFrameTime;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
        LeanTween.reset();
        settingButton.LeanMoveLocalY(-1900f, 0.1f);       
    }
    private void Start()
    {
        int highScore = PlayerPrefs.GetInt(ScoreHandler.HighScoreKey, 0);
        int maxLap = PlayerPrefs.GetInt(ScoreHandler.MaxLapKey, 0);

        highScoreText.text = "High Score: " + highScore.ToString();
        maxLapsText.text = "Max Laps: " + maxLap.ToString();

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if(EnergyReadyKey == string.Empty) { return;  }

            DateTime energyReady = DateTime.Parse(energyReadyString);
            //TODO fix it.
            if(DateTime.Now > energyReady)
            {
                energy += 1;
                if(energy >= maxEnergy)
                {
                    energy = maxEnergy;
                }
                PlayerPrefs.SetInt(EnergyKey, energy);

            }
        }
        energyText.text = energy.ToString();

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
        DateTime oneMinFromNow = DateTime.Now.AddMinutes(1);
        
        if (1 > energy)
        {
            //Not Enough Energy
            Debug.Log("Not Enough Energy, you cant play");
            return;
        }
        energy--;
        if (0 > energy) { energy = 0; }

        PlayerPrefs.SetInt(EnergyKey, energy);
        if(maxEnergy > energy)
        {
            //Energy will add-up in x minitos
            PlayerPrefs.SetString(EnergyReadyKey, oneMinFromNow.ToString());
            Debug.Log("Energy will zort in: " + oneMinFromNow.ToString());
        }
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
        settingButton.LeanMoveLocalY(-650f, 0.7f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true);
    }

    public void BackToMenu()
    {
        settingButton.LeanMoveLocalY(-1900f, 0.7f)
            .setEaseInBack()
            .setIgnoreTimeScale(true);
    }
    public void changeFPSButton()
    {

    }
    public void changeFPS()
    {
        fpsVal++;

        if (fpsVal >= 3)
        {
            fpsVal = 0;
        }
        
        switch (fpsVal)
        {
            case 0:
                TargetFrameRate = 60;
                TargetFPSText.text = "Change FPS: 60";
                break;
            case 1:
                TargetFrameRate = 90;
                TargetFPSText.text = "Change FPS: 90";
                break;
            case 2:
                TargetFrameRate = 120;
                TargetFPSText.text = "Change FPS: 120";
                break;
            default:
                TargetFrameRate = 60;
                TargetFPSText.text = "Change FPS: 60";
                break;
        }
    }
    
}
