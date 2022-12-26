using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoSingletonGlobal<AdsManager>
{
    public string AppId = "ca-app-pub-8606178092427240~8710596129";
    public string BannerId = "ca-app-pub-8606178092427240/7707424076";
    public string InterId = "ca-app-pub-8606178092427240/7110821413";
    public string RewardId = "ca-app-pub-8606178092427240/1830574574";

    public enum AdStatus
    {
        LoadFail,
        Loading,
        Loaded
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => {
            Debug.Log($"GoogleMobileAds-{initStatus.ToString()}");
        });

        RequestBanner();
        RequestInterstitial();
        RequestReward();
    }

    private AdStatus BannerStatus = AdStatus.Loading;
    private BannerView bannerView;

    private void RequestBanner()
    {
        BannerStatus = AdStatus.Loading;

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(BannerId, AdSize.Banner, AdPosition.Top);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += BannerView_OnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += BannerView_OnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += BannerView_OnAdOpening;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += BannerView_OnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    private void BannerView_OnAdClosed(object sender, System.EventArgs e)
    {

    }

    private void BannerView_OnAdOpening(object sender, System.EventArgs e)
    {

    }

    private void BannerView_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        BannerStatus = AdStatus.LoadFail;
    }

    private void BannerView_OnAdLoaded(object sender, System.EventArgs e)
    {
        BannerStatus = AdStatus.Loaded;
    }

    public void ShowBanner()
    {
        bannerView.Show();
    }  
    
    public void HideBanner()
    {
        bannerView.Hide();
    }

    private AdStatus InterStatus = AdStatus.Loading;
    private InterstitialAd IntersView;

    private void RequestInterstitial()
    {
        InterStatus = AdStatus.Loading;

        // Initialize an InterstitialAd.
        this.IntersView = new InterstitialAd(InterId);

        // Called when an ad request has successfully loaded.
        this.IntersView.OnAdLoaded += IntersView_OnAdLoaded;
        // Called when an ad request failed to load.
        this.IntersView.OnAdFailedToLoad += IntersView_OnAdFailedToLoad;
        // Called when an ad is shown.
        this.IntersView.OnAdOpening += IntersView_OnAdOpening;
        // Called when the ad is closed.
        this.IntersView.OnAdClosed += IntersView_OnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        this.IntersView.LoadAd(request);
    }

    private void IntersView_OnAdClosed(object sender, System.EventArgs e)
    {
        Inter_Callback?.Invoke();
    }

    private void IntersView_OnAdOpening(object sender, System.EventArgs e)
    {
        
    }

    private void IntersView_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        InterStatus = AdStatus.LoadFail;
    }

    private void IntersView_OnAdLoaded(object sender, System.EventArgs e)
    {
        InterStatus = AdStatus.Loaded;
    }

    private System.Action Inter_Callback;
    public void ShowInter(System.Action Callback)
    {
        if (this.IntersView.IsLoaded())
        {
            Inter_Callback = Callback;
            this.IntersView.Show();
        }
        else
        {
            Callback?.Invoke();
        }
    }

    private AdStatus RewardStatus;
    private RewardedAd RewardView;
    private void RequestReward()
    {
        RewardStatus = AdStatus.Loading;

        // Initialize an Reward.
        this.RewardView = new RewardedAd(RewardId);

        // Called when an ad request has successfully loaded.
        this.RewardView.OnAdLoaded += RewardView_OnAdLoaded;
        // Called when an ad request failed to load.
        this.RewardView.OnAdFailedToLoad += RewardView_OnAdFailedToLoad;
        // Called when an ad is shown.
        this.RewardView.OnAdOpening += RewardView_OnAdOpening;
        // Called when an ad request failed to show.
        this.RewardView.OnAdFailedToShow += RewardView_OnAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.RewardView.OnUserEarnedReward += RewardView_OnUserEarnedReward;
        // Called when the ad is closed.
        this.RewardView.OnAdClosed += RewardView_OnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the rewarded ad with the request.
        this.RewardView.LoadAd(request);
    }

    private void RewardView_OnAdClosed(object sender, System.EventArgs e)
    {
        Reward_Callback_Success?.Invoke();
    }

    private void RewardView_OnUserEarnedReward(object sender, Reward e)
    {
        
    }

    private void RewardView_OnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Reward_Callback_Missing?.Invoke();
    }

    private void RewardView_OnAdOpening(object sender, System.EventArgs e)
    {
        
    }

    private void RewardView_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        RewardStatus = AdStatus.LoadFail;
    }

    private void RewardView_OnAdLoaded(object sender, System.EventArgs e)
    {
        RewardStatus = AdStatus.Loaded;
    }

    private System.Action Reward_Callback_Success;
    private System.Action Reward_Callback_Missing;
    public void ShowReward(System.Action Callback_Success, System.Action Callback_Missing)
    {
        if (this.RewardView.IsLoaded())
        {
            Reward_Callback_Success = Callback_Success;
            Reward_Callback_Missing = Callback_Missing;
            this.RewardView.Show();
        }
        else
        {
            Callback_Missing?.Invoke();
        }
    }

    float Timer = 0;
    private void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= 3f)
        {
            if (BannerStatus == AdStatus.LoadFail) RequestBanner();
            if (InterStatus == AdStatus.LoadFail) RequestInterstitial();
            if (RewardStatus == AdStatus.LoadFail) RequestReward();
        }
    }
}
