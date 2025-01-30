using Runtime.EntryPoints.EventHandlers;
using Runtime.Service.LocationGenerator;
using TapToStep.Scripts.Runtime.EntryPoints;
using UI.Views;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameEntryPoint _gameEntryPoint;
        [SerializeField] private LocationBuilder _locationBuilder;
        [SerializeField] private PlayerBuilder _playerBuilder;
        [SerializeField] private GameViewController _gameViewController;
        
        
        public override void InstallBindings()
        {
            BindEntryPoint();
            BindLocationBuilder();
            BindPlayerBuilder();
            BindGameViewController();

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
    }
}