using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float gainOverTime = 1f;
    [SerializeField] private float turningSpeed = 100f;
    
    public GameObject deathScreenPOP, menuScreenPOP;

    private int SteerRotation;
    public static int lapsCounter;

    private float _fpsTimer;
    private int isFPSshow, adCount =0;
    [SerializeField] private TMP_Text _fpsText, HUDText, energyText;
    [SerializeField] private Button playAgainButton, continueOnAd, adBTN;
    private const string showFPSKey = "showFPS";
    public GameObject showfpslabel;

    private bool isRewinding = false;
    public float rewindRecordTime = 2f;
    List<PointInTime> pointsInTime;

    public DateTime continueTime;
    private void Awake()
    {
        //deathScreenPOP.LeanMoveLocalX(-1200f, 0.1f);
        isFPSshow = PlayerPrefs.GetInt(showFPSKey, 0);
        if(isFPSshow == 1) { showfpslabel.SetActive(true); }
        else { showfpslabel.SetActive(false); }
        

    }
    void Start()
    {
        lapsCounter = 0;
        pointsInTime = new List<PointInTime>();
        continueTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        //speed += gainOverTime * Time.deltaTime;
        transform.Rotate(0f, SteerRotation * turningSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        HUDText.text = "Speed: " + speed.ToString("F1") + "\nSteering: " + turningSpeed.ToString("F0") + "\nLaps: " + lapsCounter.ToString("F0");
        if(isFPSshow == 1)
        {
            FPSCounter();
        }
        
    }
    private void FixedUpdate()
    {
        speed += gainOverTime * Time.deltaTime;
        turningSpeed += gainOverTime * 3 * Time.deltaTime;
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle.");
            continueOnAd.interactable = false;
            DeathScreenMenu();
        }
        else if (other.CompareTag("Finish"))
        {
            lapsCounter++;
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

    void toggleTimeScale()
    {
        if(Time.timeScale == 0){ Time.timeScale = 1; }
        else { Time.timeScale = 0; }
    }

    void FPSCounter()
    {
        if (Time.unscaledTime > _fpsTimer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
            _fpsTimer = Time.unscaledTime + 1f;
        }
    }

    void OnDestroy()
    {

    }
    //DeathScreen Methods
    public void DeathScreenMenu()
    {
        toggleTimeScale();
        deathScreenPOP.SetActive(true);
        deathScreenPOP.LeanScale(Vector3.one, 0.3f)
        .setIgnoreTimeScale(true);
        //leanscale(vector3 1 , süresi);
        
        //deathScreenPOP.transform.localScale = Vector3.one;

        int energy = PlayerPrefs.GetInt("Energy", 0);
        energyText.text = energy.ToString();
        if (energy > 0) { playAgainButton.interactable = true; }
        else { playAgainButton.interactable = false; }
    }

    public void closeDeathScreenMenu()
    {
        //deathScreenPOP.transform.Translate(new Vector3(-1200f, 0f, 0f));
        deathScreenPOP.LeanScale(Vector3.zero, 0.3f)
        .setIgnoreTimeScale(true);
        deathScreenPOP.SetActive(false);
        //deathScreenPOP.transform.localScale = Vector3.zero;

        Time.timeScale = 1;

    }

    public void adWatch()
    {
        adCount++;
        if (adCount >= 2)
        {
            adCount = 0;
            adBTN.interactable = false;
        }
        continueOnAd.interactable = true;
    }
    public void adToContinue()
    {
        StartRewinding();
        closeDeathScreenMenu();
    }
    public void playAgain()
    {
        

        int energy = PlayerPrefs.GetInt("Energy", 0);
        PlayerPrefs.SetInt("Energy", --energy);

        SceneManager.LoadScene("Scene_Game");
        Time.timeScale = 1;
    }
    public void quitToMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
        toggleTimeScale();
    }
    //MenuScreen Methods
    public void MenuButtonPressed()
    {
        
        toggleTimeScale();
        OpenMenuScreen();
    }
    public void OpenMenuScreen()
    {        
        menuScreenPOP.LeanMoveLocalX(0f, 0.7f)
        .setEaseOutBack()
        .setIgnoreTimeScale(true);
    }

    public void offThePause()
    {
        CloseMenuScreen();
        StartCoroutine(Pause(3));
    }
    public void CloseMenuScreen()
    {
        menuScreenPOP.transform.Translate(new Vector3(-1200f, 0f, 0f));
        Time.timeScale = 1;
    }
    //Rewind Methods
    public void StartRewinding()
    {
        isRewinding = true;
    }
    public void StopRewinding()
    {
        isRewinding = false;
    }
    public void Rewind()
    {
        if(pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
            
        }
        else
        {
            StopRewinding();
            Time.timeScale = 0;
            StartCoroutine(Pause(3));
        }
        

    }
    public void Record()
    {   
        if(pointsInTime.Count > Mathf.Round(rewindRecordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }
    private IEnumerator Pause(int p)
    {
        Time.timeScale = 0.1f;
        float pauseEndTime = Time.realtimeSinceStartup + 1;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }
}
