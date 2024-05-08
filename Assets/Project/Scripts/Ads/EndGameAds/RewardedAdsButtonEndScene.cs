using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardedAdsButtonEndScene : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    [SerializeField] private EndGameSceneController endGameSceneController;
    //[SerializeField] private GameObject loadingGameObject;
    string _adUnitId = null; // This will remain null for unsupported platforms

    void Awake()
    {
        //loadingGameObject.SetActive(false);
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        //Disable the button until the ad is ready to show:
        if (AdManager.Instance.CanShowRewardedInterstitialAd())
        {
            _showAdButton.interactable = true;
            _showAdButton.onClick.AddListener(ShowAd);
        }
        else
        {
            _showAdButton.interactable = false;
        }
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        //loadingGameObject.SetActive(true);
        // Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        //loadingGameObject.SetActive(false);

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.interactable = false;
        Debug.Log("Ad Clicked");
        // Then show the ad:
        // Advertisement.Show(_adUnitId, this);
        if (AdManager.Instance.CanShowRewardedInterstitialAd())
        {
            UnityEvent onShowRewardedAd = new UnityEvent();
            onShowRewardedAd.AddListener(GiveReward);
            AdManager.Instance.ShowRewardedInterstitialAd(onShowRewardedAd);
        }
    }

    private void GiveReward()
    {
        Debug.Log("Give Reward!");
        StartCoroutine(endGameSceneController.StrartSpawnCoinImage(PlayerPrefs.GetInt("BonusBalance")));
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
        }
    }

    
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        //loadingGameObject.SetActive(false);
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}