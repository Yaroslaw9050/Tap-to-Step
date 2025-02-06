using System;
using Core.Service.Leaderboard;
using Runtime.Audio;
using Runtime.Service.LocationGenerator;
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
        
        
        [Inject]
        public void Constructor(PlayerBuilder playerBuilder,
            LocationBuilder locationBuilder, GameViewController viewController,
            AudioController audioController, LeaderboardService leaderboardService)
        {
            _playerBuilder = playerBuilder;
            _locationBuilder = locationBuilder;
            _viewController = viewController;
            _audioController = audioController;
            _leaderboardService = leaderboardService;
        }
        
        private async void Start()
        {
            SetupGraphicSetting();
            
            _audioController.Init();
            await _locationBuilder.GenerateNewLocationAsync();
            _playerBuilder.CreatePlayer(Vector3.zero, _locationBuilder.StaticBackgroundTransform);
            _viewController.Init(_playerBuilder.PlayerEntryPoint);
            await _leaderboardService.InitAsync();
        }

        private void OnDestroy()
        {
            _playerBuilder.DestroyPlayer();
            _viewController.Destruct();
        }

        private void SetupGraphicSetting()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = 2500;
        }
    }
}