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

    [SerializeField] AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private TMP_Text highScoreText, maxLapsText, TargetFPSText, showFPSText, energyText;
    [SerializeField] private int maxEnergy, energyReChargeDuration;
    [SerializeField] private Button playBTN, musicBTN, sfxBTN, showFpsBTN;

    private int energy;
    DateTime nextEnergyTime = DateTime.Now;

    private const string EnergyKey = "Energy";
    private const string EnergyTimerReadyKey = "EnergyTimerReady";
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
    private void Update()
    {
        
    }

    private void Start()
    {
        //Go Stuff
        OnApplicationFocus(true);
        //Check Energy
        Debug.Log("start");//InvokeRepeating
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) { return; }
        CancelInvoke(nameof(chargeEnergy)); 

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        energyText.text = energy.ToString();

        if(energy > 0) { playBTN.interactable = true; }
        else { playBTN.interactable = false; }

        if (maxEnergy > energy)
        {           
            string energyReadyString = PlayerPrefs.GetString(EnergyTimerReadyKey, string.Empty);
            if(energyReadyString == string.Empty) { return; }
            DateTime energyReady = DateTime.Parse(energyReadyString);
            if(DateTime.Now > energyReady)
            {
                chargeEnergy();
                PlayerPrefs.SetString(EnergyTimerReadyKey, nextEnergyTime.ToString());
            }
            else
            {
                Invoke(nameof(chargeEnergy), (energyReady - DateTime.Now).Seconds);
            }
        }
    }
    public void chargeEnergy()
    {
        energy++;
        if(energy > maxEnergy) { energy = maxEnergy; }
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = energy.ToString();
    }
    public void checkEnergy()
    {
  
    }
    public void manageEnergyTimers(int energyNeed)
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
        if (1 > energy){ playBTN.interactable = false; return; } 
        energy--;

        energyText.text = energy.ToString();
        PlayerPrefs.SetInt(EnergyKey, energy);

        if (maxEnergy > energy)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyTimerReadyKey, string.Empty);
            if (energyReadyString == string.Empty) { return; }
            DateTime energyReady = DateTime.Parse(energyReadyString);
            if(energyReady >= nextEnergyTime)
            {
                energyReady = DateTime.Now.AddMinutes(energyReChargeDuration);
                PlayerPrefs.SetString(EnergyTimerReadyKey, energyReady.ToString());
                nextEnergyTime = energyReady.AddMinutes(energyReChargeDuration);
#if UNITY_ANDROID
                androidNotificationHandler.ScheduleNotification(energyReady, 0);
#endif
            }

        }

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
