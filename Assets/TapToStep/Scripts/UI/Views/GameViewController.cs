using CompositionRoot.SO.Player.Logic;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using UI.Views.LeaderBoard;
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
        private bool _isFirstTap;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
        }
        
        public void Init(PlayerEntryPoint entryPoint)
        {
            _isFirstTap = false;
            _playerEntryPoint = entryPoint;
            
            _gameView.Init(_playerEntryPoint);
            _mainMenuView.Init(_playerEntryPoint);
            _deadView.Init(_playerEntryPoint);
            _leaderBoardView.Init();
            
            _gameView.HideView();
            _deadView.HideView();
            _mainMenuView.HideView();
            _leaderBoardView.HideView(0f);
            _tutorialView.ShowView(0f);

            _gameEventHandler.OnPlayerStartMoving += PlayerStartMoving;
            _gameEventHandler.OnPlayerDied += OnPlayerDied;
            _gameView.OnToMenuButtonPressed += ToMenuButtonPressed;
            _mainMenuView.OnBackButtonClicked += MainMenuBackButtonClicked;
            _mainMenuView.OnToLeaderboardButtonPressed += ToLeaderboardFromMenu;
            _leaderBoardView.OnBackButtonPressed += LeaderBoardBackButtonPressed;
        }

        public void Destruct()
        {
            _gameView.Destruct();
            _deadView.Destruct();
            
            _gameEventHandler.OnPlayerStartMoving -= PlayerStartMoving;
            _gameEventHandler.OnPlayerDied -= OnPlayerDied;
            _gameView.OnToMenuButtonPressed -= ToMenuButtonPressed;
            _mainMenuView.OnBackButtonClicked -= MainMenuBackButtonClicked;
            _leaderBoardView.OnBackButtonPressed -= LeaderBoardBackButtonPressed;
        }

        private void PlayerStartMoving()
        {
            if(_isFirstTap) return;
            
            _isFirstTap = true;
            _tutorialView.HideView();
            _gameView.ShowView();
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

        private void LeaderBoardBackButtonPressed()
        {
            _mainMenuView.ShowView(0f);
            _leaderBoardView.HideView(0f);
        }

        private void ToLeaderboardFromMenu()
        {
            _mainMenuView.HideView();
            _leaderBoardView.ShowView(0f);
        }
    }
}