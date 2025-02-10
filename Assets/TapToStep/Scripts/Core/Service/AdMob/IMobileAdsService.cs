using System;
using GoogleMobileAds.Api;

namespace TapToStep.Scripts.Core.Service.AdMob
{
    public interface IMobileAdsService
    {
        public void Init();

        public void LoadBannerAd();

        public void LoadAndShowDeadAd();
        public void LoadContinueAd();
        public void ShowInterstitialAd(InterstitialAd ad);

        public void ShowRewardedBitsAd(Action<double> onComplete);
        public void LoadRewardBitsAd(Action onComplete);
        
        public event Action<InterstitialAd> OnShowInterstitialAd;
        public event Action OnContinueAdRecorded;
    }
}