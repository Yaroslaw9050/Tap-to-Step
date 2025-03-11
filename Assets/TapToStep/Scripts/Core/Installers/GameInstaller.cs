using Core.Service.AdMob;
using Core.Service.LocalUser;
using Runtime.Audio;
using Runtime.EntryPoints;
using Runtime.Player.Upgrade;
using Runtime.Service.LocationGenerator;
using UI.Views;
using UI.Views.Controller;
using UI.Views.Upgrades;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LocalPlayerService _localPlayerService;
        [SerializeField] private PlayerPerkSystem _playerPerkSystem;
        [SerializeField] private GameEntryPoint _gameEntryPoint;
        [SerializeField] private LocationBuilder _locationBuilder;
        [SerializeField] private PlayerBuilder _playerBuilder;
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
            BindPlayerLocalService();
        }
        
        private void BindPlayerLocalService()
        {
            Container.Bind<LocalPlayerService>().FromInstance(_localPlayerService).AsSingle().NonLazy();
        }

        private void BindMusicToMaterial()
        {
            Container.Bind<MusicToMaterialEmmision>().FromInstance(_musicToMaterialEmision).AsSingle().NonLazy();
        }

        private void BindMobAds()
        {
            Container.Bind<IMobileAdsService>().To<AdMobService>().AsSingle().NonLazy();
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
            Container.Bind<ViewController>().AsSingle().NonLazy();
        }

        private void BindPlayerUpdateSystem()
        {
            Container.Bind<PlayerPerkSystem>().FromInstance(_playerPerkSystem).AsSingle().NonLazy();
        }
    }
}