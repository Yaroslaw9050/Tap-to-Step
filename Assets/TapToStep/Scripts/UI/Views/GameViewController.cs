using CompositionRoot.SO.Player.Logic;
using Runtime.EntryPoints.EventHandlers;
using UnityEngine;
using Zenject;

namespace UI.Views
{
    public class GameViewController : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private GameView _gameView;
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private DeadView _deadView;
        [SerializeField] private TutorialView _tutorialView;
        [SerializeField] private LeaderBoardView _leaderBoardView;

        private GameEventHandler _gameEventHandler;
        private PlayerSettingSO _playerSettingSo;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler, PlayerBuilder playerBuilder)
        {
            _gameEventHandler = gameEventHandler;
            _playerSettingSo = playerBuilder.PlayerSettingSo;
        }
        
        public void Init()
        {
            _gameView.Init(_gameEventHandler, _playerSettingSo);
            _deadView.Init(_gameEventHandler);
            
            _tutorialView.ShowView();
            _gameView.ShowView();
            _deadView.HideView();
            
            _gameEventHandler.OnPlayerStartMoving += PlayerStartMoving;
            _gameEventHandler.OnPlayerDied += OnPlayerDied;
        }

        public void Destruct()
        {
            _gameView.Destruct();
            _deadView.Destruct();
            
            _gameEventHandler.OnPlayerStartMoving -= PlayerStartMoving;
            _gameEventHandler.OnPlayerDied -= OnPlayerDied;
        }

        private void PlayerStartMoving()
        {
            _tutorialView.HideView();
        }

        private void OnPlayerDied()
        {
            _gameView.HideView();
            _deadView.ShowView();
        }
    }
}