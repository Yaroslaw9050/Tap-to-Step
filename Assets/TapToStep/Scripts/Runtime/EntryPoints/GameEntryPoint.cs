using System;
using System.Threading;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using Patterns.Models;
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
        private ViewController _viewController;
        private LeaderboardService _leaderboardService;
        private IMobileAdsService _mobileAdsService;
        private MusicToMaterialEmmision _musicToMaterialEmision;
        
        private PlayerModel _playerModel;
        private CancellationTokenSource _cts;
        
        [Inject]
        public void Constructor(PlayerBuilder playerBuilder,
            LocationBuilder locationBuilder, ViewController viewController,
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
            _cts = new CancellationTokenSource();
        }
        
        private async void Start()
        {
            SetupGraphicSetting();
            _viewController.SetLoadingFadeTo(true);
            
            await _locationBuilder.GenerateNewLocationAsync(_cts.Token);
            _playerBuilder.CreatePlayer(Vector3.zero, _locationBuilder.StaticBackgroundTransform);
            
            _viewController.SetLoadingFadeTo(false);
            _viewController.StartGameLoop();
            //
            // _mobileAdsService.Init();
            // _audioController.Init();
           
            // await _viewController.InitAsync(_playerBuilder.PlayerEntryPoint);
            // await _leaderboardService.InitAsync();
            // _mobileAdsService.LoadBannerAd();
            // _musicToMaterialEmision.Init(_audioController.MusicSource);
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
            
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