using Core.Service.GlobalEvents;
using Core.Service.LocalUser;
using Runtime.Builders.Location;
using Runtime.Player.CompositionRoot;
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

        public GlobalEventsHolder GlobalEventsHolder => _globalEventsHolder;
        public PlayerStatistic PlayerStatistic => _playerStatistic;

        public void Init(GlobalEventsHolder globalEventsHolder, LocalPlayerService localPlayerService)
        {
            _globalEventsHolder = globalEventsHolder;
            _playerStatistic.LoadAllDataToVariables();

            _screenCaster = new ScreenCaster(this, _globalEventsHolder);
            _cameraController.Initialise(this, _globalEventsHolder, localPlayerService);
            _interactionTrigger.Initialise(_globalEventsHolder, localPlayerService);
            _movement.Initialise(this, localPlayerService, _screenCaster);

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