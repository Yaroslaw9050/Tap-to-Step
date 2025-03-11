using System.Collections.Generic;
using System.Threading;
using Core.Service.AdMob.Enums;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Core.Service.AdMob.Interstitial
{
    public class InterstitialAdController
    {
        private readonly Dictionary<InterstitialAdType, InterstitialAd> r_interstitialAd = new();
        
#if UNITY_ANDROID
        private const string INTERSTITIAL_CONTINUE_ADS = "ca-app-pub-7582758822795295/8275391517";
        private const string INTERSTITIAL_AFTER_DEAD_ADS = "ca-app-pub-7582758822795295/9202332516";
        
#elif UNITY_IOS
        private const string INTERSTITIAL_CONTINUE_ADS = "ca-app-pub-7582758822795295/4991624763";
        private const string INTERSTITIAL_AFTER_DEAD_ADS = "ca-app-pub-7582758822795295/8153200068";
#endif
        
        
        public async UniTask<LoadStatus> LoadInterstitialAdAsync(InterstitialAdType adType, CancellationToken 
            cancellationToken)
        {
            if (r_interstitialAd.ContainsKey(adType))
            {
                Debug.Log($"Interstitial {adType} is already loaded.");
                return LoadStatus.Success;
            }

            var adRequest = new AdRequest();
            var adUnitId = GetInterstitialAdUid(adType);
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError($"Invalid interstitial ad type: {adType}");
                return LoadStatus.Error;
            }

            var tcs = new UniTaskCompletionSource<LoadStatus>();

            InterstitialAd.Load(adUnitId, adRequest, (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogWarning($"Failed to load interstitial ad {adType}: {error?.GetMessage()}");
                    tcs.TrySetResult(LoadStatus.Error);
                    return;
                }

                r_interstitialAd[adType] = ad;
                tcs.TrySetResult(LoadStatus.Success);
                ad.OnAdFullScreenContentClosed += () =>
                {
                    r_interstitialAd.Remove(adType);
                    LoadInterstitialAdAsync(adType, cancellationToken).Forget();
                };
            });
            return await tcs.Task.AttachExternalCancellation(cancellationToken);
        }
        
        public void ShowInterstitialAd(InterstitialAdType adType)
        {
            if (!r_interstitialAd.TryGetValue(adType, out var ad) || !ad.CanShowAd())
            {
                Debug.LogWarning($"Interstitial ad {adType} is not ready to show.");
                return;
            }
            ad.Show();
        }
        
        private string GetInterstitialAdUid(InterstitialAdType adType)
        {
            return adType switch
            {
                InterstitialAdType.DeadViewContinue => INTERSTITIAL_CONTINUE_ADS,
                InterstitialAdType.AfterDead => INTERSTITIAL_AFTER_DEAD_ADS,
                _ => null
            };
        }
    }
}