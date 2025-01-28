using System;
using Runtime.Audio;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Service.LocationGenerator;
using UI.Views;
using UnityEngine;

namespace TapToStep.Scripts.Runtime.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerBuilder _playerBuilder;
        [SerializeField] private LocationBuilder _locationBuilder;
        [SerializeField] private GameViewController _viewController;
        [SerializeField] private AudioController _audioController;

        private GlobalEventHandler _globalEventHandler;
        
        private async void Start()
        {
            SetupGraphicSetting();
            _globalEventHandler = new GlobalEventHandler();
            
            _audioController.Init(_globalEventHandler);
            await _locationBuilder.GenerateNewLocationAsync();
            _playerBuilder.CreatePlayer(Vector3.zero, _globalEventHandler);
            _viewController.Init(_globalEventHandler, _playerBuilder.PlayerSettingSo);
        }

        private void OnDestroy()
        {
            _audioController.Destruct();
        }

        private void SetupGraphicSetting()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = 2500;
        }
    }
}