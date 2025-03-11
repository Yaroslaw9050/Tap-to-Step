using System;
using System.Collections.Generic;
using System.Threading;
using Core.Service.AdMob.Enums;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Core.Service.AdMob.Reward
{
    public class RewardAdController
    {
        private readonly Dictionary<RewardAdType, RewardedAd> r_rewardAds = new();
        
#if UNITY_ANDROID
        private const string REWARD_EARN_BITS_ADS = "ca-app-pub-7582758822795295/1750685079";
        
#elif UNITY_IOS
        private const string REWARD_EARN_BITS_ADS = "ca-app-pub-7582758822795295/2420730920";
#endif
        
        
        public async UniTask<(LoadStatus, double)> LoadRewardAdAsync(RewardAdType adType, CancellationToken 
                cancellationToken)
        {
            if (r_rewardAds.ContainsKey(adType))
            {
                Debug.Log($"Reward {adType} is already loaded.");
                return (LoadStatus.Success, r_rewardAds.GetValueOrDefault(adType).GetRewardItem().Amount);
            }

            var adRequest = new AdRequest();
            var adUnitId = GetRewardAdUid(adType);
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError($"Invalid reward ad type: {adType}");
                return (LoadStatus.Error, 0.0);
            }

            var tcs = new UniTaskCompletionSource<(LoadStatus, double)>();

            RewardedAd.Load(adUnitId, adRequest, (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogWarning($"Failed to load reward ad {adType}: {error?.GetMessage()}");
                    tcs.TrySetResult((LoadStatus.Error, 0.0));
                    return;
                }

                r_rewardAds[adType] = ad;
                tcs.TrySetResult((LoadStatus.Success, ad.GetRewardItem().Amount));
                ad.OnAdFullScreenContentClosed += () =>
                {
                    r_rewardAds.Remove(adType);
                    LoadRewardAdAsync(adType, cancellationToken).Forget();
                };
            });

            return await tcs.Task.AttachExternalCancellation(cancellationToken);
        }

        
        public async UniTask<double?> ShowRewardAdAsync(RewardAdType adType, CancellationToken cancellationToken = default)
        {
            if (!r_rewardAds.TryGetValue(adType, out var ad) || !ad.CanShowAd())
            {
                Debug.LogWarning($"Reward ad {adType} is not ready to show.");
                return null;
            }

            var tcs = new UniTaskCompletionSource<double>();

            ad.Show(reward =>
            {
                Debug.Log($"User earned reward from {adType}: {reward.Amount}");
                tcs.TrySetResult(reward.Amount);
            });

            return await tcs.Task.AttachExternalCancellation(cancellationToken);
        }
        
        
        private string GetRewardAdUid(RewardAdType adType)
        {
            return adType switch
            {
                RewardAdType.GameLoopGetBits => REWARD_EARN_BITS_ADS,
                _ => null
            };
        }
    }
}