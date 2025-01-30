using CompositionRoot.SO.Player.Logic;
using Runtime.Builders.Location;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerEntryPoint : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Movement _movement;
        [SerializeField] private ScreenCaster _screenCaster;
        [SerializeField] private InteractionTriggerHolder _interactionTrigger;
        
        private PlayerEventHandler _playerEventHandler;
        private ILocationGenerator _locationGenerator;
        private PlayerSettingSO _playerSetting;
        
        public ScreenCaster ScreenCaster => _screenCaster;
        public PlayerEventHandler PlayerEventHandler => _playerEventHandler;
        public PlayerSettingSO PlayerSettingSo => _playerSetting;
        public ILocationGenerator LocationGenerator => _locationGenerator;
        public InteractionTriggerHolder InteractionTrigger => _interactionTrigger;

        public void Init(GameEventHandler gameEventHandler, PlayerSettingSO playerSetting)
        {
            _playerSetting = playerSetting;
            _playerEventHandler = new PlayerEventHandler(this, gameEventHandler);
            
            _cameraController.Init(this);
            _movement.Init(this);
            _screenCaster.Init(this, gameEventHandler);
            _interactionTrigger.Initialize(_playerEventHandler);
        }

        public void Destruct()
        {
            _screenCaster.Destruct();
            _movement.Destruct();
            _cameraController.Destruct();
        }
    }
}