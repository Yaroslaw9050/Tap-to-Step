using System;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using Runtime.Audio;
using Runtime.Service.LocationGenerator;
using TapToStep.Scripts.Core.Service.AdMob;
using UI.Views.Upgrades;
using UnityEngine;
using Zenject;

namespace TapToStep.Scripts.Runtime.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        private AudioController _audioController;
        private PlayerBuilder _playerBuilder;
        private LocationBuilder _locationBuilder;
        private GameViewController _viewController;
        private LeaderboardService _leaderboardService;
        private IMobileAdsService _mobileAdsService;
        private MusicToMaterialEmmision _musicToMaterialEmision;
        
        [Inject]
        public void Constructor(PlayerBuilder playerBuilder,
            LocationBuilder locationBuilder, GameViewController viewController,
            AudioController audioController, LeaderboardService leaderboardService,
            IMobileAdsService mobileAdsService,
            MusicToMaterialEmmision musicToMaterialEmision)
        {
            _playerBuilder = playerBuilder;
            _locationBuilder = locationBuilder;
            _viewController = viewController;
            _audioController = audioController;
            _leaderboardService = leaderboardService;
            _mobileAdsService = mobileAdsService;
            _musicToMaterialEmision = musicToMaterialEmision;
        }
        
        private async void Start()
        {
            SetupGraphicSetting();

            _mobileAdsService.Init();
            _audioController.Init();
            await _locationBuilder.GenerateNewLocationAsync();
            _playerBuilder.CreatePlayer(Vector3.zero, _locationBuilder.StaticBackgroundTransform);
            await _viewController.InitAsync(_playerBuilder.PlayerEntryPoint);
            await _leaderboardService.InitAsync(_playerBuilder.PlayerEntryPoint.PlayerStatistic);
            _mobileAdsService.LoadBannerAd();
            _musicToMaterialEmision.Init(_audioController.MusicSource);
        }

        private void OnDestroy()
        {
            _playerBuilder.DestroyPlayer();
            _viewController.Destruct();
        }

        private void SetupGraphicSetting()
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}