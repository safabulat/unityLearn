using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float gainOverTime = 1f;
    [SerializeField] private float turningSpeed = 100f;
    
    public GameObject deathScreenPOP;

    private int SteerRotation;
    public static int lapsCounter;

    private float _fpsTimer;
    private int isFPSshow, adCount =0;
    [SerializeField] private TMP_Text _fpsText, HUDText, energyText;
    [SerializeField] private Button playAgainButton, continueOnAd, adBTN;
    private const string showFPSKey = "showFPS";
    public GameObject showfpslabel;

    private bool isRewinding = false;
    public float recordTime = 0.5f;
    List<PointInTime> pointsInTime;
    Rigidbody rb;

    private void Awake()
    {
        deathScreenPOP.LeanMoveLocalX(-1200f, 0.1f);
        isFPSshow = PlayerPrefs.GetInt(showFPSKey, 0);
        if(isFPSshow == 1) { showfpslabel.SetActive(true); }
        else { showfpslabel.SetActive(false); }
        

    }
    void Start()
    {
        lapsCounter = 0;
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
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

    public void MenuButtonPressed()
    {
        toggleTimeScale();
    }

    public void quitToMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
        toggleTimeScale();
    }
    public void playAgain()
    {
        int energy = PlayerPrefs.GetInt("Energy", 0);
        PlayerPrefs.SetInt("Energy", --energy);

        SceneManager.LoadScene("Scene_Game");
        toggleTimeScale();
    }
    public void adWatch()
    {
        adCount++;
        if(adCount >= 2)
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
        //rewindTimeHandler.StopRewinding();
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

    public void DeathScreenMenu()
    {
        toggleTimeScale();
        deathScreenPOP.LeanMoveLocalX(0f, 0.7f)
        .setEaseOutBack()
        .setIgnoreTimeScale(true);

        int energy = PlayerPrefs.GetInt("Energy", 0);
        energyText.text = energy.ToString();
        if (energy > 0) { playAgainButton.interactable = true; }
        else { playAgainButton.interactable = false; }
    }

    public void closeDeathScreenMenu()
    {
        deathScreenPOP.transform.Translate(new Vector3(-1200f, 0f, 0f));
        //TODO wait x seconds and continue game
        Time.timeScale = 1;
        
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("zort timescale sifir olunca bu da calismiyor");

    }

    public void ToggleRewind()
    {
        if (!isRewinding)
        {
            isRewinding = true;
        }

        //else
        //{
        //    isRewinding = false;       
        //}

        Debug.Log("." + isRewinding);
    }
    public void StartRewinding()
    {
        isRewinding = true;
        //rb.isKinematic = true;
    }
    public void StopRewinding()
    {
        isRewinding = false;
        //rb.isKinematic = false;
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
        }

    }
    public void Record()
    {   
        if(pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }
}
