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

    [SerializeField] AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private TMP_Text highScoreText, maxLapsText, TargetFPSText, energyText, debugText;
    [SerializeField] private int maxEnergy, energyReChargeDuration;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";
    private const string FPSKey = "FPSKey";

    private int TargetFrameRate = 60;
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
        TargetFrameRate = PlayerPrefs.GetInt(FPSKey, 60);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;
        int highScore = PlayerPrefs.GetInt(ScoreHandler.HighScoreKey, 0);
        int maxLap = PlayerPrefs.GetInt(ScoreHandler.MaxLapKey, 0);

        highScoreText.text = "High Score: " + highScore.ToString();
        maxLapsText.text = "Max Laps: " + maxLap.ToString();

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        energyText.text = energy.ToString();

        
    }
    private void Update()
    {
        
    }
    private void Start()
    {
        InvokeRepeating(nameof(checkEnergy), 0f, 15f);
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
        if (1 > energy)
        {
            //Not Enough Energy
            Debug.Log("Not Enough Energy, you cant play");
            debugText.text = "Not Enough Energy, you cant play";
            return;
        }
        energy--;
        energyText.text = energy.ToString();
        PlayerPrefs.SetInt(EnergyKey, energy);

        if (maxEnergy > energy)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyReChargeDuration); 
            //Energy will increase in x minitos
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
            Debug.Log("Energy will increase at: " + energyReady.ToString());
            debugText.text = "Energy will increase at: " + energyReady.ToString();
            
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(energyReady, energyReady.AddMinutes(1));
#endif
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
    public void checkEnergy()
    {
        Debug.Log("Invoke: checkEnergy");
        if (maxEnergy > energy)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if (EnergyReadyKey == string.Empty) { return; }
            DateTime energyReady = DateTime.Parse(energyReadyString);
            DateTime nextEnergyTime;
            DateTime energyMaxReadyTime = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                energy += 1;
                int needEnergy = maxEnergy - energy;
                if (needEnergy > 0)
                {
                    nextEnergyTime = energyReady.AddMinutes(energyReChargeDuration);
                }
                else
                {
                    nextEnergyTime = energyReady;
                }
                PlayerPrefs.SetString(EnergyReadyKey, nextEnergyTime.ToString());
                Debug.Log("Energy Ready at: " + energyReady.ToString());
                Debug.Log("Energy Increased at: " + DateTime.Now.ToString());
                Debug.Log("Next Energy Increase Time: " + nextEnergyTime.ToString());
                debugText.text = "Next Energy Increase Time: " + nextEnergyTime.ToString();
                if (energy >= maxEnergy)
                {
                    energy = maxEnergy;
                }
                PlayerPrefs.SetInt(EnergyKey, energy);

            }


        }

        energyText.text = energy.ToString();
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
        PlayerPrefs.SetInt(EnergyReadyKey, TargetFrameRate);
        TargetFPSText.text = "Change FPS: " + TargetFrameRate;
    }
    
}
