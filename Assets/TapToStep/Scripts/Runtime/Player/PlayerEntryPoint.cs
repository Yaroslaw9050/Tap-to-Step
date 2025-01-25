using CompositionRoot.SO.Player.Logic;
using Runtime.Builders.Location;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerSettingSO _playerSettingSo;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Movement _movement;
        [SerializeField] private ScreenCaster _screenCaster;
        //[SerializeField] private InteractionTriggerHolder _interactionTrigger;
        //[SerializeField] private PlayerViewsHolder _viewsHolder;
        private EventHandler _eventHandler;

        private ILocationGenerator _locationGenerator;
        //private ISceneLoader _sceneLoader;
        
        public ScreenCaster ScreenCaster => _screenCaster;
        public EventHandler EventHandler => _eventHandler;
        public PlayerSettingSO PlayerSettingSo => _playerSettingSo;

        public ILocationGenerator LocationGenerator => _locationGenerator;
        //public InteractionTriggerHolder InteractionTrigger => _interactionTrigger;
        //public ISceneLoader SceneLoader => _sceneLoader;

        public void Init()
        {
            _eventHandler = new EventHandler();
            
            _cameraController.Init(this);
            _movement.Init(this);
            _screenCaster.Init(this);
            _screenCaster.ActivateControl();
            //_sceneLoader = sceneLoader;
            //_interactionTrigger.Initialize(locationGenerator, _eventHandler);
            //_viewsHolder.Initialise(viewService, _cameraRotation);
        }
    }
}