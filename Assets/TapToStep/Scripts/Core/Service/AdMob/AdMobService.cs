using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace TapToStep.Scripts.Core.Service.AdMob
{
    public class AdMobService: IMobileAdsService
    {
        private bool _isInitialized;

#if UNITY_ANDROID
        private const string BANNER_LOOP_ADS = "ca-app-pub-7582758822795295/7559106027";
        private const string REWARD_DEAD_ADS = "ca-app-pub-7582758822795295/8275391517";
        private const string REWARD_RARE_AFTER_DEAD_ADS = "ca-app-pub-7582758822795295/9202332516";
#elif UNITY_IOS
        private const string BANNER_LOOP_ADS = "ca-app-pub-7582758822795295/2913284002";
        private const string REWARD_CONTINUE_ADS = "ca-app-pub-7582758822795295/4991624763";
        private const string REWARD_RARE_AFTER_DEAD_ADS = "ca-app-pub-7582758822795295/8153200068";
#endif
        private BannerView _loopBanner;
        private InterstitialAd _deadContinueInterstitial;
        private InterstitialAd _afterDeadInterstitial;
        
        public event Action<InterstitialAd> OnShowInterstitialAd;
        public event Action OnContinueAdRecorded;

        public void Init()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("Admob service is already initialized.");
                return;
            }

            MobileAds.Initialize(status =>
            {
                Debug.Log($"Admob service has been initialized.| {status}" );
                _isInitialized = true;
            });
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
            if (_afterDeadInterstitial != null)
            {
                _afterDeadInterstitial.Destroy();
                _afterDeadInterstitial = null;
            }

            var adRequest = new AdRequest();
            InterstitialAd.Load(REWARD_RARE_AFTER_DEAD_ADS, adRequest, AfterDeadAdLoaded);
        }

        public void LoadContinueAd()
        {
            if (_deadContinueInterstitial != null)
            {
                _deadContinueInterstitial.OnAdImpressionRecorded -= ContinueAdsRecorded;
                _deadContinueInterstitial.Destroy();
                _deadContinueInterstitial = null;
            }

            var adRequest = new AdRequest();
            InterstitialAd.Load(REWARD_CONTINUE_ADS, adRequest, ContinueAdLoaded);
        }
        
        
        public void LoadBannerAd()
        {
            if (_loopBanner == null)
            {
                CreateBannerView();
            }
            
            var adRequest = new AdRequest();
            _loopBanner?.LoadAd(adRequest);
        }

        private void AfterDeadAdLoaded(InterstitialAd adRequest, LoadAdError error)
        {
            if (error != null || adRequest == null)
            {
                return;
            }

            _afterDeadInterstitial = adRequest;
            _afterDeadInterstitial.Show();
        }
        
        private void ContinueAdLoaded(InterstitialAd adRequest, LoadAdError error)
        {
            if (error != null || adRequest == null)
            {
                return;
            }

            _deadContinueInterstitial = adRequest;
            _deadContinueInterstitial.OnAdImpressionRecorded += ContinueAdsRecorded;
            OnShowInterstitialAd?.Invoke(_deadContinueInterstitial);
        }
        
        private void ContinueAdsRecorded()
        {
            OnContinueAdRecorded?.Invoke();
        }

        private void CreateBannerView()
        {
            if (_loopBanner != null)
            {
                DestroyBanner(_loopBanner);
            }
            _loopBanner = new BannerView(BANNER_LOOP_ADS, AdSize.IABBanner, AdPosition.Bottom);
        }

        private void DestroyBanner(BannerView selectedBanner)
        {
            if (selectedBanner != null)
            {
                selectedBanner.Destroy();
                selectedBanner = null;
            }
        }
    }
}