using CompositionRoot.SO.Player.Logic;
using Runtime.Builders.Location;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player.CompositionRoot;
using Runtime.Player.Perks;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerEntryPoint : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Movement _movement;
        [SerializeField] private ScreenCaster _screenCaster;
        [SerializeField] private InteractionTriggerHolder _interactionTrigger;
        
        [Header("Player Statistic")]
        [SerializeField] private PlayerStatistic _playerStatistic;
        
        private GameEventHandler _gameEventHandler;
        private PlayerEventHandler _playerEventHandler;
        private ILocationGenerator _locationGenerator;
        private PlayerSettingSO _playerSetting;

        public PlayerStatistic PlayerStatistic => _playerStatistic;
        public PlayerEventHandler PlayerEventHandler => _playerEventHandler;
        public PlayerSettingSO PlayerSettingSo => _playerSetting;

        public void Init(GameEventHandler gameEventHandler, PlayerSettingSO playerSetting,
            PlayerPerkSystem playerPerkSystem)
        {
            _playerSetting = playerSetting;
            _gameEventHandler = gameEventHandler;
            _playerEventHandler = new PlayerEventHandler(this, gameEventHandler);
            _playerStatistic.LoadAllDataToVariables();
            
            _cameraController.Init(this, gameEventHandler);
            _movement.Init(this, playerPerkSystem);
            _screenCaster.Init(this, gameEventHandler);
            _interactionTrigger.Initialize(_playerEventHandler);
            
            _gameEventHandler.InvokeOnPlayerScreenCastStatus(true);
            _gameEventHandler.OnMenuViewStatusChanged += OnMenuView;
            _gameEventHandler.OnPlayerResumed += OnPlayerResumed;
            _playerEventHandler.OnPlayerDied += OnPlayerDied;
        }

        public void Destruct()
        {
            _playerStatistic.SaveAllData();
            _screenCaster.Destruct();
            _movement.Destruct();
            _cameraController.Destruct();
            
            _gameEventHandler.OnMenuViewStatusChanged -= OnMenuView;
            _gameEventHandler.OnPlayerResumed -= OnPlayerResumed;
            _playerEventHandler.OnPlayerDied -= OnPlayerDied;
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