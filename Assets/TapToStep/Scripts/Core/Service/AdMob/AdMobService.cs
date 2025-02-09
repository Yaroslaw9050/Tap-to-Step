using GoogleMobileAds.Api;
using UnityEngine;

namespace TapToStep.Scripts.Core.Service.AdMob
{
    public class AdMobService: IMobileAdsService
    {
        private bool _isInitialized;

#if UNITY_ANDROID
        private const string BANNER_LOOP_ADS = "ca-app-pub-7582758822795295/7559106027";
#elif UNITY_IOS
        private const string BANNER_LOOP_ADS = "ca-app-pub-7582758822795295/2913284002";
#endif

        private BannerView _loopBanner;
        
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
            });
        }

        public void LoadAd()
        {
            if (_loopBanner == null)
            {
                CreateBannerView();
            }
            
            var adRequest = new AdRequest();
            _loopBanner?.LoadAd(adRequest);
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