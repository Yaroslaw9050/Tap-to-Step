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

        private GlobalEventHandler _globalEventHandler;
        
        private async void Start()
        {
            _globalEventHandler = new GlobalEventHandler();
            
            await _locationBuilder.GenerateNewLocationAsync();
            _playerBuilder.CreatePlayer(Vector3.zero, _globalEventHandler);

            _viewController.Init(_globalEventHandler, _playerBuilder.PlayerSettingSo);
        }
    }
}