using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class AdManager : MonoBehaviour
{

    [SerializeField]
    private string iOSRewardedAd = "ca-app-pub-3940256099942544/1712485313";

    [SerializeField]
    private string iOSInterstitialAd = "ca-app-pub-3940256099942544/4411468910";

    [SerializeField]
    private string iOSRewardedInterstitialAd = "ca-app-pub-3940256099942544/6978759866";

    [SerializeField]
    private string androidRewardedAd = "ca-app-pub-3940256099942544/5224354917";

    [SerializeField]
    private string androidInterstitialAd = "ca-app-pub-3940256099942544/1033173712";

    [SerializeField]
    private string androidRewardedInterstitialAd = "ca-app-pub-3940256099942544/5354046379";

    [SerializeField]
    private List<String> iOSTestDeviceIds = new List<string>();

    [SerializeField]
    private List<String> androidTestDeviceIds = new List<string>();

    public static AdManager Instance;

    private RewardedInterstitialAd rewardedInterstitialAd;
    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    private UnityEvent rewardedEvent;
    private UnityEvent interstitialEvent;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // On Android, Unity is paused when displaying interstitial or rewarded video.
        // This behavior should be made consistent with iOS.
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
#if UNITY_IOS
        deviceIds.AddRange(iOSTestDeviceIds);
#elif UNITY_ANDROID
        deviceIds.AddRange(androidTestDeviceIds);
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        var requestConfiguration = new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.False)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        MobileAds.Initialize((InitializationStatus status) =>
        {
            Debug.Log("Init Admob completed.");
            RequestAndLoadRewardedAd();
            RequestAndLoadInterstitialAd();
            RequestAndLoadRewardedInterstitialAd();
        });
    }


    # region INTERSTITIAL ADS

    public bool CanShowInterstitialAd()
    {
        return interstitialAd is not null && interstitialAd.CanShowAd();
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }

    public void ShowInterstitialAd(UnityEvent eventHandler)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            interstitialEvent = eventHandler;
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }


    private void RequestAndLoadInterstitialAd()
    {
        Debug.Log("Requesting Interstitial ad.");

#if UNITY_ANDROID
        string adUnitId = androidInterstitialAd;
#elif UNITY_IOS
        string adUnitId = iOSInterstitialAd;
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        InterstitialAd.Load(adUnitId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("Interstitial ad failed to load with error: " +
                                loadError.GetMessage());
                    return;
                }

                if (ad == null)
                {
                    Debug.Log("Interstitial ad failed to load.");
                    return;
                }

                Debug.Log("Interstitial ad loaded.");
                interstitialAd = ad;

                RegisterEventHandlers(ad);
            });
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            RequestAndLoadInterstitialAd();
            Debug.Log("Interstitial ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            interstitialEvent.Invoke();
            interstitialEvent = null;
            Destroy(ad);
            Debug.Log("Interstitial ad full screen content closed.");
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            RequestAndLoadRewardedAd();
            ad.Destroy();
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void Destroy(InterstitialAd ad)
    {
        ad?.Destroy();
        ad = null;
    }

    # endregion

    #region REWARDED ADS

    public bool CanShowRewardedAd()
    {
        return rewardedAd is not null && rewardedAd.CanShowAd();
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Rewarded ad granted a reward: " + reward.Amount);
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    public void ShowRewardedAd(UnityEvent eventHandler)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Rewarded ad granted a reward: " + reward.Amount);
            });
            rewardedEvent = eventHandler;
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    private void RequestAndLoadRewardedAd()
    {
        Debug.Log("Requesting Rewarded ad.");

#if UNITY_ANDROID
        string adUnitId = androidRewardedAd;
#elif UNITY_IOS
        string adUnitId = iOSRewardedAd;
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        RewardedAd.Load(adUnitId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("Rewarded ad failed to load with error: " +
                                loadError.GetMessage());
                    return;
                }

                if (ad == null)
                {
                    Debug.Log("Rewarded ad failed to load.");
                    return;
                }

                Debug.Log("Rewarded ad loaded.");
                rewardedAd = ad;

                RegisterEventHandlers(ad);
            });
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            RequestAndLoadRewardedAd();
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            rewardedEvent.Invoke();
            rewardedEvent = null;
            Destroy(ad);
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            RequestAndLoadRewardedAd();
            ad.Destroy();
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void Destroy(RewardedAd ad)
    {
        ad?.Destroy();
        ad = null;
    }

    #endregion

    # region REWAREDED INTERSTITIAL ADS

    public bool CanShowRewardedInterstitialAd()
    {
        return rewardedInterstitialAd is not null && rewardedInterstitialAd.CanShowAd();
    }

    public void ShowRewardedInterstitialAd(UnityEvent eventHandler)
    {
        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show(reward =>
            {
                Debug.Log("Rewarded Interstitial ad granted a reward: " + reward.Amount);
                eventHandler.Invoke();
            });
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }

    private void RequestAndLoadRewardedInterstitialAd()
    {
        Debug.Log("Requesting Rewarded Interstitial ad.");

#if UNITY_ANDROID
        string adUnitId = androidRewardedInterstitialAd;
#elif UNITY_IOS
        string adUnitId = iOSRewardedInterstitialAd;
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        RewardedInterstitialAd.Load(adUnitId, CreateAdRequest(),
            (RewardedInterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("Rewarded Interstitial ad failed to load with error: " +
                              loadError.GetMessage());
                    return;
                }

                if (ad == null)
                {
                    Debug.Log("Rewarded Interstitial ad failed to load.");
                    return;
                }

                Debug.Log("Rewarded Interstitial ad loaded.");
                rewardedInterstitialAd = ad;

                RegisterEventHandlers(ad);
            });
    }

    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded Interstitial ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded Interstitial ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            RequestAndLoadRewardedInterstitialAd();
            Debug.Log("Rewarded Interstitial ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Destroy(ad);
            Debug.Log("Rewarded Interstitial ad full screen content closed.");
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            RequestAndLoadRewardedAd();
            ad.Destroy();
            Debug.LogError("Rewarded Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void Destroy(RewardedInterstitialAd ad)
    {
        ad?.Destroy();
        ad = null;
    }

    # endregion

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
}
