using Runtime.Audio;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class BootInstaller: MonoInstaller
    {
        [SerializeField] private AudioController _audioController;
        private GameEventHandler _gameEventHandler;
        
        public override void InstallBindings()
        {
            _gameEventHandler = new GameEventHandler();
            
            BindAudioController();
            BindGameEventHandler();
        }

        private void BindAudioController()
        {
            Container.Bind<AudioController>().FromInstance(_audioController).AsSingle().NonLazy();
        }
        
        private void BindGameEventHandler()
        {
            Container.Bind<GameEventHandler>().FromInstance(_gameEventHandler).AsSingle().NonLazy();
        }
    }
}