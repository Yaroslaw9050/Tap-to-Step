using Core.Service.Authorization;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Core.Service.LocalUser;
using Core.Service.RemoteDataStorage;
using Runtime.Audio;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class BootInstaller: MonoInstaller
    {
        [SerializeField] private AudioController _audioController;
        
        public override void InstallBindings()
        {
            BindAudioController();
            BindGameEventHandler();
            BindAuthorizationService();
            BindRemoteDataStorageService();
            BindLeaderboardService();
        }

        private void BindLeaderboardService()
        {
            Container.BindInterfacesAndSelfTo<FirebaseLeaderBoardService>().AsSingle().NonLazy();
        }
        
        private void BindRemoteDataStorageService()
        {
            Container.BindInterfacesAndSelfTo<RemoteFirebaseDataStorageService>().AsSingle().NonLazy();
        }

        private void BindAuthorizationService()
        {
            Container.BindInterfacesAndSelfTo<FirebaseAuthorization>().AsSingle().NonLazy();
        }

        private void BindAudioController()
        {
            Container.Bind<AudioController>().FromInstance(_audioController).AsSingle().NonLazy();
        }

        private void BindGameEventHandler()
        {
            Container.Bind<GlobalEventsHolder>().AsSingle().NonLazy();
        }
    }
}