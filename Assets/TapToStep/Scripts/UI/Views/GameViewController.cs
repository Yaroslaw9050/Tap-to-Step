using CompositionRoot.SO.Player.Logic;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using UnityEngine;
using Zenject;

namespace UI.Views.Upgrades
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
        private PlayerEntryPoint _playerEntryPoint;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
        }
        
        public void Init(PlayerEntryPoint entryPoint)
        {
            _playerEntryPoint = entryPoint;
            
            _gameView.Init(_playerEntryPoint);
            _mainMenuView.Int(_playerEntryPoint);
            _deadView.Init();

            _tutorialView.ShowView(0f);
            _gameView.ShowView();
            _deadView.HideView(0f);
            _mainMenuView.HideView(0f);
            
            _gameEventHandler.OnPlayerStartMoving += PlayerStartMoving;
            _gameEventHandler.OnPlayerDied += OnPlayerDied;
            _gameView.OnToMenuButtonPressed += ToMenuButtonPressed;
            _mainMenuView.OnBackButtonClicked += MainMenuBackButtonClicked;
        }

        public void Destruct()
        {
            _gameView.Destruct();
            _deadView.Destruct();
            
            _gameEventHandler.OnPlayerStartMoving -= PlayerStartMoving;
            _gameEventHandler.OnPlayerDied -= OnPlayerDied;
            _gameView.OnToMenuButtonPressed -= ToMenuButtonPressed;
            _mainMenuView.OnBackButtonClicked -= MainMenuBackButtonClicked;
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

        private void ToMenuButtonPressed()
        {
            _gameView.HideView();
            _mainMenuView.ShowView();
            _gameEventHandler.InvokeOnMenuViewStatus(true);
        }

        private void MainMenuBackButtonClicked()
        {
            _mainMenuView.HideView();
            _gameView.ShowView();
            _gameEventHandler.InvokeOnMenuViewStatus(false);
        }
    }
}