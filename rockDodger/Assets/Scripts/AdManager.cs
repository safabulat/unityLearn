using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool testMode = true;
#if UNITY_ANDROID
    private string gameID = "4994285";
#elif UNITY_IOS
    private string gameID = "4994284";
#endif

    public static AdManager Instance;
    private InGameMenus gameOverHandler;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Advertisement.AddListener(this);
            Advertisement.Initialize(gameID, testMode);
        }
    }
    public void ShowAd(InGameMenus gameOverHandler)
    {
        this.gameOverHandler = gameOverHandler;

        Advertisement.Show("rewardedVideo");
    }
    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError("Unity Adds Error" + message);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Finished:
                gameOverHandler.ContinueGame();
                break;
            case ShowResult.Skipped:
                //Ad was skipped.
                break;
            case ShowResult.Failed:
                Debug.LogWarning("Ad Failed");
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ad Started");
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads Ready");
    }
}
