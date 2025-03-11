using System;
using System.Threading;
using Core.Service.AdMob.Banner;
using Core.Service.AdMob.Enums;
using Core.Service.AdMob.Reward;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Core.Service.AdMob
{
    public class AdMobService: IMobileAdsService
    {
#if UNITY_ANDROID
        private const string REWARD_CONTINUE_ADS = "ca-app-pub-7582758822795295/8275391517";
        private const string REWARD_RARE_AFTER_DEAD_ADS = "ca-app-pub-7582758822795295/9202332516";
        
#elif UNITY_IOS
        private const string REWARD_CONTINUE_ADS = "ca-app-pub-7582758822795295/4991624763";
        private const string REWARD_RARE_AFTER_DEAD_ADS = "ca-app-pub-7582758822795295/8153200068";
#endif
        
        
        private InterstitialAd _continueAfterDead;
        private InterstitialAd _rareAfterDead;
        private RewardedAd _rewardedBitsAd;

        private readonly BannerAdController r_bannerController;
        private readonly RewardAdController r_rewardController;
        
        public event Action<InterstitialAd> OnShowInterstitialAd;
        public event Action OnContinueAdRecorded;

        public AdMobService()
        {
            r_bannerController = new BannerAdController();
            r_rewardController = new RewardAdController();
            
            MobileAds.Initialize(status =>
            {
                Debug.Log($"Admob service has been initialized.| {status}" );
            });
        }

        public void LoadAndShowBanner(BannerAdType adType)
        {
            r_bannerController.LoadAndShowBanner(adType);
        }

        public void HideAndUnloadBanner(BannerAdType adType)
        {
            r_bannerController.HideAndUnloadBanner(adType);
        }

        public async UniTask<(LoadStatus, double)> LoadRewardAdAsync(RewardAdType adType, CancellationToken token)
        {
             return await r_rewardController.LoadRewardAdAsync(adType, token);
        }

        public async UniTask<double> ShowRewardAdAsync(RewardAdType adType)
        {
            var result= await r_rewardController.ShowRewardAdAsync(adType);
            return result ?? 0.0;
        }

        public void ShowInterstitialAd(InterstitialAd ad)
        {
            if (ad != null && ad.CanShowAd())
            {
                ad.Show();
            }
        }
        
        public void LoadAndShowDeadAd()
        {
            if (_rareAfterDead != null)
            {
                _rareAfterDead.Destroy();
                _rareAfterDead = null;
            }

            var adRequest = new AdRequest();
            InterstitialAd.Load(REWARD_RARE_AFTER_DEAD_ADS, adRequest, AfterDeadAdLoaded);
        }

        public void LoadContinueAd()
        {
            if (_continueAfterDead != null)
            {
                _continueAfterDead.OnAdImpressionRecorded -= ContinueAdsRecorded;
                _continueAfterDead.Destroy();
                _continueAfterDead = null;
            }

            var adRequest = new AdRequest();
            InterstitialAd.Load(REWARD_CONTINUE_ADS, adRequest, ContinueAdLoaded);
        }
        
        private void AfterDeadAdLoaded(InterstitialAd adRequest, LoadAdError error)
        {
            if (error != null || adRequest == null)
            {
                return;
            }

            _rareAfterDead = adRequest;
            _rareAfterDead.Show();
        }

        private void ContinueAdLoaded(InterstitialAd adRequest, LoadAdError error)
        {
            if (error != null || adRequest == null)
            {
                return;
            }

            _continueAfterDead = adRequest;
            _continueAfterDead.OnAdImpressionRecorded += ContinueAdsRecorded;
            OnShowInterstitialAd?.Invoke(_continueAfterDead);
        }

        private void ContinueAdsRecorded()
        {
            OnContinueAdRecorded?.Invoke();
        }
    }
}