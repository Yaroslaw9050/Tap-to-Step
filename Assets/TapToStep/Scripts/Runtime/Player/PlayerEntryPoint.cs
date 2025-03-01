using CompositionRoot.SO.Player.Logic;
using Core.Service.GlobalEvents;
using Patterns.Models;
using Runtime.Builders.Location;
using Runtime.Player.CompositionRoot;
using Runtime.Player.Perks;
using TapToStep.Scripts.Core.Service.LocalUser;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerEntryPoint : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Movement _movement;
        [SerializeField] private InteractionTriggerHolder _interactionTrigger;

        [Header("Player Statistic")]
        [SerializeField] private PlayerStatistic _playerStatistic;
        
        private ScreenCaster _screenCaster;
        private GlobalEventsHolder _globalEventsHolder;
        private ILocationGenerator _locationGenerator;
        private PlayerSettingSO _playerSetting;

        public GlobalEventsHolder GlobalEventsHolder => _globalEventsHolder;
        public PlayerStatistic PlayerStatistic => _playerStatistic;
        public PlayerSettingSO PlayerSettingSo => _playerSetting;

        public void Init(GlobalEventsHolder globalEventsHolder, PlayerSettingSO playerSetting,
            PlayerPerkSystem playerPerkSystem, LocalPlayerService localPlayerService)
        {
            _playerSetting = playerSetting;
            _globalEventsHolder = globalEventsHolder;
            _playerStatistic.LoadAllDataToVariables();

            _screenCaster = new ScreenCaster(this, _globalEventsHolder);
            _cameraController.Initialise(this, _globalEventsHolder);
            _interactionTrigger.Initialise(_globalEventsHolder, localPlayerService);
            _movement.Initialise(this, playerPerkSystem, localPlayerService, _screenCaster);

            SubscribeToEvents();
        }

        public void Destruct()
        {
            _playerStatistic.SaveAllData();
            _screenCaster.Destruct();
            _movement.Destruct();
            _cameraController.Destruct();
            
            UnSubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            //_gameEventHandler.OnMenuViewStatusChanged += OnMenuView;
            _globalEventsHolder.PlayerEvents.OnReborn += OnPlayerResumed;
            _globalEventsHolder.PlayerEvents.OnDied += OnPlayerDied;
        }

        private void UnSubscribeFromEvents()
        {
            //_gameEventHandler.OnMenuViewStatusChanged -= OnMenuView;
            _globalEventsHolder.PlayerEvents.OnReborn -= OnPlayerResumed;
            _globalEventsHolder.PlayerEvents.OnDied -= OnPlayerDied;
        }

        private void OnMenuView(bool isOpen)
        {
            _playerStatistic.SaveAllData();
        }

        private void OnPlayerDied()
        {
            _movement.OnPlayerDied();
            _cameraController.MoveToDeadPosition();
        }

        private void OnPlayerResumed()
        {
            _movement.OnPlayerResumed();
            _cameraController.MoveToPlayerResumePosition();
        }
    }
}