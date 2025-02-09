using Runtime.Audio;
using Runtime.Player.Perks;
using Runtime.Service.LocationGenerator;
using TapToStep.Scripts.Core.Service.AdMob;
using TapToStep.Scripts.Runtime.EntryPoints;
using UI.Views.Upgrades;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPerkSystem _playerPerkSystem;
        [SerializeField] private GameEntryPoint _gameEntryPoint;
        [SerializeField] private LocationBuilder _locationBuilder;
        [SerializeField] private PlayerBuilder _playerBuilder;
        [SerializeField] private GameViewController _gameViewController;
        [SerializeField] private MusicToMaterialEmmision _musicToMaterialEmision;
        
        
        public override void InstallBindings()
        {
            BindEntryPoint();
            BindLocationBuilder();
            BindPlayerBuilder();
            BindGameViewController();
            BindPlayerUpdateSystem();
            BindMobAds();
            BindMusicToMaterial();
        }

        private void BindMusicToMaterial()
        {
            Container.Bind<MusicToMaterialEmmision>().FromInstance(_musicToMaterialEmision).AsSingle().NonLazy();
        }

        private void BindMobAds()
        {
            Container.BindInterfacesAndSelfTo<AdMobService>().AsSingle().NonLazy();
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

        private void BindPlayerUpdateSystem()
        {
            Container.Bind<PlayerPerkSystem>().FromInstance(_playerPerkSystem).AsSingle().NonLazy();
        }
    }
}