using System;
using System.Threading;
using System.Threading.Tasks;
using CompositionRoot.Enums;
using Core.Service.Authorization;
using Core.Service.Leaderboard;
using Core.Service.LocalUser;
using Core.Service.RemoteDataStorage;
using Cysharp.Threading.Tasks;
using Patterns.MVVM.Models;
using Runtime.Audio;
using Runtime.Service.LocationGenerator;
using TapToStep.Scripts.Core.Service.AdMob;
using UI.Views;
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
        private IAuthorizationService _authorizationService;
        
        private PlayerModel _playerModel;
        private CancellationTokenSource _cts;
        private LocalPlayerService _localPlayerService;
        private IRemoteDataStorageService _remoteDataStorageService;

        [Inject]
        public void Constructor(PlayerBuilder playerBuilder,
            LocationBuilder locationBuilder, ViewController viewController,
            AudioController audioController, LeaderboardService leaderboardService,
            IMobileAdsService mobileAdsService,
            MusicToMaterialEmmision musicToMaterialEmision,
            IAuthorizationService authorizationService,
            LocalPlayerService localPlayerService,
            IRemoteDataStorageService remoteDataStorageService)
        {
            _cts = new CancellationTokenSource();
            
            _playerBuilder = playerBuilder;
            _locationBuilder = locationBuilder;
            _viewController = viewController;
            _audioController = audioController;
            _leaderboardService = leaderboardService;
            _mobileAdsService = mobileAdsService;
            _musicToMaterialEmision = musicToMaterialEmision;
            _authorizationService = authorizationService;
            _localPlayerService = localPlayerService;
            _remoteDataStorageService = remoteDataStorageService;
        }
        
        private async void Start()
        {
            SetupGraphicSetting();
            _viewController.Initialize();
            _viewController.DisplayPreparingViews();
            
            await UserAuthorizationAsync();
            
            await _locationBuilder.GenerateNewLocationAsync(_cts.Token);
            _playerBuilder.CreatePlayer(Vector3.zero, _locationBuilder.StaticBackgroundTransform);
            
            _viewController.DisplayGameLoopViews();
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

        private async UniTask UserAuthorizationAsync()
        {
            _authorizationService.Initialise();
            _remoteDataStorageService.Initialise();
            
            var userId = await _authorizationService.SignInAsync();
            if (string.IsNullOrEmpty(userId))
            {
                userId = await _authorizationService.SignUpAsync();
                _localPlayerService.PlayerModel.UserId.Value = userId;
                _remoteDataStorageService.CreateStartedFieldsForNewUser(userId);
                _remoteDataStorageService.SavePerkAsync(userId, _localPlayerService.GetPerk(PerkType.StepSpeed).ToPlayerPerkData());
                _remoteDataStorageService.SavePerkAsync(userId, _localPlayerService.GetPerk(PerkType.StepLenght).ToPlayerPerkData());
                _remoteDataStorageService.SavePerkAsync(userId, _localPlayerService.GetPerk(PerkType.TurnSpeed).ToPlayerPerkData());
                return;
            }
            
            _localPlayerService.PlayerModel.UserId.Value = userId;
            await _localPlayerService.LoadAllPerksAsync();
            await _localPlayerService.LoadBaseUserDataAsync();
        }
    }
}