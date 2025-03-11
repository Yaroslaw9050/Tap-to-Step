using System.Collections.Generic;
using Core.Service.AdMob.Enums;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Core.Service.AdMob.Banner
{
    public class BannerAdController
    {
        private readonly Dictionary<BannerAdType, BannerView> r_bannerAds = new();

#if UNITY_ANDROID
        private const string BANNER_LOOP_ADS = "ca-app-pub-7582758822795295/7559106027";
        
#elif UNITY_IOS
        private const string BANNER_LOOP_ADS = "ca-app-pub-7582758822795295/2913284002";
#endif

        public void LoadAndShowBanner(BannerAdType adType)
        {
            if (r_bannerAds.ContainsKey(adType))
            {
                Debug.Log($"Banner {adType} is already loaded.");
                return;
            }

            var adRequest = new AdRequest();
            var bannerView = CreateBanner(adType);
            
            if(bannerView == null) return;
            
            r_bannerAds[adType] = bannerView;
            bannerView.LoadAd(adRequest);
            bannerView.Show();
        }
        
        public void HideAndUnloadBanner(BannerAdType adType)
        {
            if (r_bannerAds.TryGetValue(adType, out var bannerView))
            {
                DestroyBanner(bannerView);
                r_bannerAds.Remove(adType);
            }
        }
        
        private BannerView CreateBanner(BannerAdType adType)
        {
            var (adUnitId, adSize, adPosition) = GetBannerAdSetting(adType);
            
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError($"Invalid banner type: {adType}");
                return null;
            }
            return new BannerView(adUnitId, adSize, adPosition);
        }

        private void DestroyBanner(BannerView selectedBanner)
        {
            selectedBanner?.Hide();
            selectedBanner?.Destroy();
        }

        private (string, AdSize, AdPosition) GetBannerAdSetting(BannerAdType adType)
        {
            switch (adType)
            {
                case BannerAdType.GameLoopBanner:
                    return (BANNER_LOOP_ADS, AdSize.Banner, AdPosition.Bottom);
                default:
                    return (null, AdSize.Banner, AdPosition.Bottom);
            }
        }
    }
}