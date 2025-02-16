using CompositionRoot.SO.Player.Logic;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private LoadingView _loadingView;
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
        
        public async UniTask InitAsync(PlayerEntryPoint entryPoint)
        {
            _isFirstTap = false;
            _playerEntryPoint = entryPoint;
            _loadingView.ShowView(0f);
            
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
            _gameEventHandler.OnPlayerResumed += OnPlayerResumed;
            
            _gameView.OnToMenuButtonPressed += ToMenuButtonPressed;
            _mainMenuView.OnBackButtonClicked += MainMenuBackButtonClicked;
            _mainMenuView.OnToLeaderboardButtonPressed += ToLeaderboardFromMenu;
            _leaderBoardView.OnBackButtonPressed += LeaderBoardBackButtonPressed;

            await UniTask.Delay(500);
            _loadingView.HideView(0.5f);
        }

        public void Destruct()
        {
            _gameView.Destruct();
            _deadView.Destruct();
            _mainMenuView.Destruct();
            
            _gameEventHandler.OnPlayerStartMoving -= PlayerStartMoving;
            _gameEventHandler.OnPlayerDied -= OnPlayerDied;
            _gameEventHandler.OnPlayerResumed -= OnPlayerResumed;
            
            _gameView.OnToMenuButtonPressed -= ToMenuButtonPressed;
            _mainMenuView.OnBackButtonClicked -= MainMenuBackButtonClicked;
            _leaderBoardView.OnBackButtonPressed -= LeaderBoardBackButtonPressed;
            _mainMenuView.OnToLeaderboardButtonPressed -= ToLeaderboardFromMenu;
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
            _mainMenuView.HideView();
            _leaderBoardView.HideView();
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

        private void OnPlayerResumed()
        {
            _deadView.HideView();
            _gameView.ShowView();
            _gameEventHandler.InvokeOnMenuViewStatus(false);
        }
    }
}