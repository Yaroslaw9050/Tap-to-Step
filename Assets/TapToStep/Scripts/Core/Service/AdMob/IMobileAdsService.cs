using System.Threading;
using Core.Service.AdMob.Enums;
using Cysharp.Threading.Tasks;

namespace Core.Service.AdMob
{
    public interface IMobileAdsService
    {
        public void LoadAndShowBanner(BannerAdType adType);
        public void HideAndUnloadBanner(BannerAdType adType);
        public UniTask<(LoadStatus, double)> LoadRewardAdAsync(RewardAdType adType, CancellationToken token);
        public UniTask<double> ShowRewardAdAsync(RewardAdType adType);
        public UniTask<LoadStatus> LoadInterstitialAdAsync(InterstitialAdType adType, CancellationToken token);
        public void ShowInterstitialAd(InterstitialAdType adType);
    }
}