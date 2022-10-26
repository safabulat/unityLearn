using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class energyHandler : MonoBehaviour
{
    [SerializeField] AndroidNotificationHandler androidNotificationHandler;

    [SerializeField] private int maxEnergy, energyReChargeDuration;

    [SerializeField] private Button adBTN;
    [SerializeField] private TMP_Text energyText;

    private const string EnergyKey = "Energy", MaxEnergyKey = "MaxEnergy";
    private const string EnergyTimerReadyKey = "EnergyTimerReady", FullEnergyTimerReadyKey = "FullEnergyTimerReady";

    public bool isIncreaseCalled = false;
    private int energy, neededEnergy;
    DateTime energyIncreaseTime, nextEnergyIncreaseTime;

    private void Awake()
    {
        string energyReadyString = PlayerPrefs.GetString(EnergyTimerReadyKey, DateTime.Now.AddMinutes(energyReChargeDuration).ToString());
        energyIncreaseTime = DateTime.Parse(energyReadyString);
        Debug.Log("energyIncreaseTime" + energyIncreaseTime.ToString());
        if (energyIncreaseTime < DateTime.Now)
        {
            Debug.Log("Energy time is less: " + energyIncreaseTime.ToString());
            energyIncreaseTime = DateTime.Now.AddMinutes(energyReChargeDuration);
            PlayerPrefs.SetString(EnergyTimerReadyKey, energyIncreaseTime.ToString());
            Debug.Log("Energy time is changed to: " + energyIncreaseTime.ToString());
        }
    }
    void Start()
    {
        InvokeRepeating(nameof(UpdateEveryXSecond), 0f, 1f);
    }
    private void FixedUpdate()
    {

    }
    void UpdateEveryXSecond()
    {
        //Get all the energy related values everysecond.
        
        string energyReadyString = PlayerPrefs.GetString(EnergyTimerReadyKey, DateTime.Now.AddMinutes(energyReChargeDuration).ToString());
        energyIncreaseTime = DateTime.Parse(energyReadyString);
        
        nextEnergyIncreaseTime = energyIncreaseTime.AddMinutes(energyReChargeDuration);
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        energyText.text = energy.ToString();

        if (maxEnergy > energy && !isIncreaseCalled && DateTime.Now >= energyIncreaseTime)
        {
            Debug.Log("xx");
            Debug.Log("maxE: " + maxEnergy);
            Debug.Log("curE: " + energy);
            Debug.Log("flag: " + isIncreaseCalled);

            isIncreaseCalled = true;
            neededEnergy = maxEnergy - energy;
            handleEnergy(neededEnergy);

        }

        if (energy != maxEnergy) { adBTN.interactable = true; }
    }
    public void handleEnergy(int neededEnergy)
    {
        //TODO
        Debug.Log("Handle energy called");
        Debug.Log("Call time: " + DateTime.Now);
        Debug.Log("Energy time: " + energyIncreaseTime);
        Debug.Log("Next energy time: " + nextEnergyIncreaseTime);

        
        //for(int i=0; i < neededEnergy; i++)
        //{
        //    chargeEnergy(energyIncreaseTime);

        //}
        if(neededEnergy >= 0) { Debug.Log("Energy Need: " + neededEnergy); }
        chargeEnergy(energyIncreaseTime);
    }
    public void invokeOfChargeEnergy()
    {
        
        chargeEnergy(energyIncreaseTime);
    }
    public void chargeEnergy(DateTime energyTime)
    {
        if (DateTime.Now >= energyTime)
        {
            energy++;
            if (energy > maxEnergy) { energy = maxEnergy; }
            PlayerPrefs.SetInt(EnergyKey, energy);
            energyText.text = energy.ToString();
            isIncreaseCalled = false;
            if (maxEnergy > energy)
            {
                PlayerPrefs.SetString(EnergyTimerReadyKey, nextEnergyIncreaseTime.ToString());
                Debug.Log("Saved energy time: " + nextEnergyIncreaseTime.ToString());
            }
            CancelInvoke(nameof(invokeOfChargeEnergy));
        }
        else
        {
            Invoke(nameof(invokeOfChargeEnergy), (energyTime - DateTime.Now).Seconds);
        }

    }
    public void handleNotificationFires(DateTime NotificationTime)
    {
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
        if (energy + 1 == maxEnergy)
        {
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(NotificationTime, 1);
#endif
        }
        else
        {
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(NotificationTime, 0);
#endif
        }
    }
    public void adToEnergy()
    {
        chargeEnergy(DateTime.Now);
        if (energy == maxEnergy) { adBTN.interactable = false; }
    }
    private void OnApplicationQuit()
    {
        CancelInvoke(nameof(UpdateEveryXSecond));
        Debug.Log("Quit");
    }
}
