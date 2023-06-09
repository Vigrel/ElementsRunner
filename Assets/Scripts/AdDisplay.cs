using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdDisplay : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string myGameIdAndroid = "5315739";
    public string myGameIdIOS = "5315738";
    public string adUnitIdAndroid = "Rewarded_Android";
    public string adUnitIdIOS = "Rewarded_iOS";
    public string myAdUnitId;
    public string myAdStatus = "";
    public bool adStarted;
    public bool adCompleted;
    public GameObject player;
    public GameObject deathCanva;
    public GameObject obstacle;
    private ObstacleMovement _obstacleMovementScript;
    
    // Score
    public GameObject score;
    private ScoreController _scoreControllerScript;

    private bool testMode = true;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS
	        Advertisement.Initialize(myGameIdIOS, testMode, this);
	        myAdUnitId = adUnitIdIOS;
        #else
                Advertisement.Initialize(myGameIdAndroid, testMode, this);
                myAdUnitId = adUnitIdAndroid;
        #endif

        _obstacleMovementScript = obstacle.GetComponent<ObstacleMovement>();
        _scoreControllerScript = score.GetComponent<ScoreController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AdShow()
    {
        if (!adStarted)
        {
            Advertisement.Show(myAdUnitId,this);
        }
    }

    public void OnInitializationComplete()
    {
        //Debug.Log("Unity Ads initialization complete.");
        Advertisement.Load(myAdUnitId,this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        myAdStatus = message;
        //Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //Debug.Log("Ad Loaded: " + adUnitId);
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        myAdStatus = message;
        //Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");

    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        myAdStatus = message;
        //Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        adStarted = true;
        //Debug.Log("Ad Started: " + adUnitId);
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        //Debug.Log("Ad Clicked: " + adUnitId);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        adCompleted = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
        //Debug.Log("Ad Completed: " + adUnitId);
        deathCanva.SetActive(false);
        _scoreControllerScript.ResumeScore();
        
        player.transform.position = new Vector2(0, 0);
        _obstacleMovementScript.ResetObstacles();
        player.GetComponent<CharacterAbilityController>().AdSeen = true;
    }

}