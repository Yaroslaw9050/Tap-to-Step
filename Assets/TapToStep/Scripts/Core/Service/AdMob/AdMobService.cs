using System;
using System.Threading;
using Core.Service.AdMob.Banner;
using Core.Service.AdMob.Enums;
using Core.Service.AdMob.Interstitial;
using Core.Service.AdMob.Reward;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Core.Service.AdMob
{
    public class AdMobService: IMobileAdsService
    {
        private InterstitialAd _continueAfterDead;
        private InterstitialAd _rareAfterDead;
        private RewardedAd _rewardedBitsAd;

        private readonly BannerAdController r_bannerController;
        private readonly RewardAdController r_rewardController;
        private readonly InterstitialAdController r_interstitialController;

        public AdMobService()
        {
            r_bannerController = new BannerAdController();
            r_rewardController = new RewardAdController();
            r_interstitialController = new InterstitialAdController();
            
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

        public async UniTask<LoadStatus> LoadInterstitialAdAsync(InterstitialAdType adType, CancellationToken token)
        {
            return await r_interstitialController.LoadInterstitialAdAsync(adType, token);
        }

        public void ShowInterstitialAd(InterstitialAdType adType)
        {
            r_interstitialController.ShowInterstitialAd(adType);
        }
    }
}