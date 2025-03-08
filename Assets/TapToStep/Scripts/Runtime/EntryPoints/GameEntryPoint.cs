using System.Threading;
using CompositionRoot.Constants;
using CompositionRoot.Enums;
using Core.Service.Authorization;
using Core.Service.Leaderboard;
using Core.Service.LocalUser;
using Core.Service.RemoteDataStorage;
using Cysharp.Threading.Tasks;
using Runtime.Audio;
using Runtime.Service.LocationGenerator;
using TapToStep.Scripts.Core.Service.AdMob;
using UI.Models;
using UI.Views.Controller;
using UnityEngine;
using Zenject;

namespace Runtime.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        private AudioController _audioController;
        private PlayerBuilder _playerBuilder;
        private LocationBuilder _locationBuilder;
        private ViewController _viewController;
        private IMobileAdsService _mobileAdsService;
        private MusicToMaterialEmmision _musicToMaterialEmision;
        private IAuthorizationService _authorizationService;
        
        private PlayerModel _playerModel;
        private CancellationTokenSource _cts;
        private LocalPlayerService _localPlayerService;
        private IRemoteDataStorageService _remoteDataStorageService;
        private ILeaderboardService _leaderboardService;

        [Inject]
        public void Constructor(PlayerBuilder playerBuilder,
            LocationBuilder locationBuilder, ViewController viewController,
            AudioController audioController,
            IMobileAdsService mobileAdsService,
            MusicToMaterialEmmision musicToMaterialEmision,
            IAuthorizationService authorizationService,
            LocalPlayerService localPlayerService,
            IRemoteDataStorageService remoteDataStorageService,
            ILeaderboardService leaderboardService)
        {
            _cts = new CancellationTokenSource();
            
            _playerBuilder = playerBuilder;
            _locationBuilder = locationBuilder;
            _viewController = viewController;
            _audioController = audioController;
            _mobileAdsService = mobileAdsService;
            _musicToMaterialEmision = musicToMaterialEmision;
            _authorizationService = authorizationService;
            _localPlayerService = localPlayerService;
            _remoteDataStorageService = remoteDataStorageService;
            _leaderboardService = leaderboardService;
        }   
        
        private async void Start()
        {
            SetupGraphicSetting();
            _audioController.Initialise();
            _viewController.Initialize();
            _viewController.DisplayPreparingViews();
            _mobileAdsService.Initialise();
            
            await UserAuthorizationAsync();
            await InitialiseLeaderboardAsync();
            
            await _locationBuilder.GenerateNewLocationAsync(_cts.Token);
            _playerBuilder.CreatePlayer(Vector3.zero, _locationBuilder.StaticBackgroundTransform);
            _musicToMaterialEmision.Initialise(_audioController.MusicSource);
            _viewController.DisplayGameLoopViews();
            
            _mobileAdsService.LoadBannerAd();
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

        private async UniTask InitialiseLeaderboardAsync()
        {
            var userID = _localPlayerService.PlayerModel.UserId.Value;
            var userName = _localPlayerService.PlayerModel.UserName.Value;
            
            _leaderboardService.Initialise();
            await _leaderboardService.CheckAllUserFieldsAsync(userID); 
            await _leaderboardService.SaveUserDataAsync(userID, DatabaseKeyAssets.USER_NAME_KEY ,userName);
            
            var value = await _leaderboardService.LoadUserDataAsync(userID, DatabaseKeyAssets.BEST_DISTANCE_KEY);
            _localPlayerService.SetBestDistance(double.Parse(value));
        }

        private async UniTask UserAuthorizationAsync()
        {
            _authorizationService.Initialise();
            _remoteDataStorageService.Initialise();
            var userId = _localPlayerService.PlayerModel.UserId.Value;
            if (string.IsNullOrEmpty(userId))
            {
                userId = await _authorizationService.SignInAsync();
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
            }
            
            await _localPlayerService.LoadAllPerksAsync();
            await _localPlayerService.LoadBaseUserDataAsync();
        }
    }
}