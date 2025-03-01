using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Runtime.Audio;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class BootInstaller: MonoInstaller
    {
        [SerializeField] private AudioController _audioController;
        [SerializeField] private LeaderboardService _leaderboardService;
        
        public override void InstallBindings()
        {
            BindAudioController();
            BindGameEventHandler();
            BindLeaderboardService();
        }

        private void BindAudioController()
        {
            Container.Bind<AudioController>().FromInstance(_audioController).AsSingle().NonLazy();
        }

        private void BindGameEventHandler()
        {
            Container.Bind<GlobalEventsHolder>().AsSingle().NonLazy();
        }

        private void BindLeaderboardService()
        {
            Container.Bind<LeaderboardService>().FromInstance(_leaderboardService).AsSingle().NonLazy();
        }
    }
}