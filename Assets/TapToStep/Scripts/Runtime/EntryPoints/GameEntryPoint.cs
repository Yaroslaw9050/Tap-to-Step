using System;
using Runtime.Audio;
using Runtime.Service.LocationGenerator;
using UI.Views;
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
        
        
        [Inject]
        public void Constructor(PlayerBuilder playerBuilder,
            LocationBuilder locationBuilder, GameViewController viewController, AudioController audioController)
        {
            _playerBuilder = playerBuilder;
            _locationBuilder = locationBuilder;
            _viewController = viewController;
            _audioController = audioController;
        }
        
        private async void Start()
        {
            SetupGraphicSetting();
            
            _audioController.Init();
            await _locationBuilder.GenerateNewLocationAsync();
            _playerBuilder.CreatePlayer(Vector3.zero);
            _viewController.Init();
        }

        private void OnDestroy()
        {
            _viewController.Destruct();
        }

        private void SetupGraphicSetting()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = 2500;
        }
    }
}