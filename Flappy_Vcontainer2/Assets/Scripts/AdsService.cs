using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsService : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsService Instance { get; private set; }

    [Header("Game IDs")]
    [SerializeField] private string androidGameId = "YOUR_ANDROID_GAME_ID";
    [SerializeField] private string iosGameId = "YOUR_IOS_GAME_ID";

    [Header("Placement IDs")]
    [SerializeField] private string bannerPlacementId = "Banner";
    [SerializeField] private string rewardedPlacementId = "Rewarded";

    [Header("Settings")]
    [SerializeField] private bool testMode = true;

    private string gameId;
    private Action rewardedClosedCallback;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAds();
    }

    private void InitializeAds()
    {
#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameId = androidGameId;
#else
        gameId = androidGameId;
#endif
        Advertisement.Initialize(gameId, testMode, this);
    }

    #region Init Callbacks
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialized");
        LoadBanner();
        LoadRewarded();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Ads Init Failed: {error} - {message}");
    }
    #endregion

    #region Banner
    public void LoadBanner()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerPlacementId, new BannerLoadOptions
        {
            loadCallback = () => Debug.Log("Banner Loaded"),
            errorCallback = (msg) => Debug.LogError("Banner Load Error: " + msg)
        });
    }

    public void ShowBanner()
    {
        Advertisement.Banner.Show(bannerPlacementId);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }
    #endregion

    #region Rewarded
    public void LoadRewarded()
    {
        Advertisement.Load(rewardedPlacementId, this);
    }

    public void ShowRewarded(Action onClosed = null)
    {
        rewardedClosedCallback = onClosed;
        Advertisement.Show(rewardedPlacementId, this);
    }
    #endregion

    #region Load/Show Callbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Ad Load Failed {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Ad Show Failed {placementId}: {error} - {message}");
        rewardedClosedCallback?.Invoke();
        rewardedClosedCallback = null;
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Ad Show Start: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Ad Clicked: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad Complete: {placementId} - {showCompletionState}");

        if (placementId == rewardedPlacementId)
        {
            // если нужно - награда
            // if (showCompletionState == UnityAdsShowCompletionState.COMPLETED) { ... }

            rewardedClosedCallback?.Invoke();
            rewardedClosedCallback = null;

            LoadRewarded();
        }
    }
    #endregion
}