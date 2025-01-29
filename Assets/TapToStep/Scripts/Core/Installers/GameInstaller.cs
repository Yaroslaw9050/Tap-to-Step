using Runtime.EntryPoints.EventHandlers;
using Runtime.Service.LocationGenerator;
using TapToStep.Scripts.Runtime.EntryPoints;
using UI.Views;
using UnityEngine;
using Zenject;

namespace TapToStep.Scripts.Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameEntryPoint _gameEntryPoint;
        [SerializeField] private LocationBuilder _locationBuilder;
        [SerializeField] private PlayerBuilder _playerBuilder;
        [SerializeField] private GameViewController _gameViewController;
        
        private GameEventHandler _gameEventHandler;
        
        public override void InstallBindings()
        {
            _gameEventHandler = new GameEventHandler();
            
            BindEntryPoint();
            BindLocationBuilder();
            BindPlayerBuilder();
            BindGameViewController();
            BindGameEventHandler();
        }

        private void BindEntryPoint()
        {
            Container.Bind<GameEntryPoint>().FromInstance(_gameEntryPoint).AsSingle().NonLazy();
        }

        private void BindLocationBuilder()
        {
            Container.Bind<LocationBuilder>().FromInstance(_locationBuilder).AsSingle().NonLazy();
        }

        private void BindPlayerBuilder()
        {
            Container.Bind<PlayerBuilder>().FromInstance(_playerBuilder).AsSingle().NonLazy();
        }

        private void BindGameViewController()
        {
            Container.Bind<GameViewController>().FromInstance(_gameViewController).AsSingle().NonLazy();
        }

        private void BindGameEventHandler()
        {
            Container.Bind<GameEventHandler>().FromInstance(_gameEventHandler).AsSingle().NonLazy();
        }
    }
}