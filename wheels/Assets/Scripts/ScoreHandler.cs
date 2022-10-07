using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public const string HighScoreKey = "HighScore";
    public const string MaxLapKey = "MaxLap";
  
    [SerializeField] float scoreMultiplier;
    [SerializeField] TMP_Text scoreText;

    private float score = 0f;
    private int maxLap;

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;
        scoreText.text = Mathf.FloorToInt(score).ToString();
        maxLap = Car.lapsCounter;
    }

    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        int currentMaxLap = PlayerPrefs.GetInt(MaxLapKey, 0);

        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
        if(maxLap > currentMaxLap)
        {
            PlayerPrefs.SetInt(MaxLapKey, Mathf.FloorToInt(maxLap));
        }

    }
}
