using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using SS.View;
 
public class RewardedSkinAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //[SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _androidAdUnitId2 = "Rewarded_Android2";

    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    [SerializeField] string _iOSAdUnitId2 = "Rewarded_iOS2";
    [SerializeField] private GameObject loadingGameObject;

    private string typeReward = "";

    string _adUnitId = null; // This will remain null for unsupported platforms
    string _adUnitId2 = null;
 
    void Awake()
    {   
        loadingGameObject.SetActive(false);
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
        _adUnitId = _iOSAdUnitId2;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
        _adUnitId2 = _androidAdUnitId2;
#endif

        //Disable the button until the ad is ready to show:
       // _showAdButton.interactable = false;
    }
 
    // Load content to the Ad Unit:
    public void LoadAd(string typeReward)
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        loadingGameObject.SetActive(true);
        this.typeReward = typeReward;
        Advertisement.Load(_adUnitId, this);
    }

 
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        loadingGameObject.SetActive(false);
        if (adUnitId.Equals(_adUnitId))
        {
            ShowAd();
        }
    }
 
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
    }
 
    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            switch (this.typeReward)
            {
                case "skin":
                    //SkinPannel currentSkinPannel = UnlockSkinProgress.Instance.GetSkinPannel(PlayerPrefs.GetInt("currentSkinPannel"));
                    //currentSkinPannel.AddAdsProgress(1);
                    break;
                case "coin":
                    Bank.Instance.Deposit(1000, Currency.Gold);
                    break;
            }
            // Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            
        }

        // if (adUnitId.Equals(_adUnitId2) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        // {
        //     Debug.Log("Unity Ads Rewarded Ad Completed");
            
        //     Bank.Instance.Deposit(1000);
        // }
    }
 
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
        loadingGameObject.SetActive(false);
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

    }
}