using CompositionRoot.SO.Player.Logic;
using Cysharp.Threading.Tasks;
using Patterns.Models;
using Patterns.ViewModels;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using UI.Views.LeaderBoard;
using UnityEngine;
using Zenject;

namespace UI.Views.Upgrades
{
    public class ViewController : MonoBehaviour
    {
        private TutorialViewModel _tutorialViewModel;
        private GameViewModel _gameViewModel;
        private DeadViewModel _deadViewModel;
        private IViewModelStorageService _viewModelStorage;

        private GameEventHandler _gameEventHandler;
        private PlayerEntryPoint _playerEntryPoint;
        private PlayerModel _playerModel;
        
        private bool _isFirstTap;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler, PlayerModel playerModel, 
            IViewModelStorageService viewModelStorage)  
        {
            _gameEventHandler = gameEventHandler;
            _viewModelStorage = viewModelStorage;
            _playerModel = playerModel;
        }

        public void SetLoadingFadeTo(bool isFade)
        {
            if (_viewModelStorage.TryGetViewModel<LoadingViewModel>(out var view))
            {
                if (isFade) view.OpenView();
                else view.CloseView();
            }
        }

        public void StartGameLoop()
        {
            _tutorialViewModel = _viewModelStorage.GetViewMode<TutorialViewModel>();
            _gameViewModel = _viewModelStorage.GetViewMode<GameViewModel>();
            _deadViewModel = _viewModelStorage.GetViewMode<DeadViewModel>();
            
            _isFirstTap = true;
            SubscribeToEvents();


            _tutorialViewModel.OpenView();
            _gameViewModel.OpenView();
            // _isFirstTap = false;
            // _playerEntryPoint = entryPoint;
            // _loadingView.ShowView(0f);
            //
            // _gameView.Init(_playerEntryPoint);
            // _mainMenuView.Init(_playerEntryPoint);
            // _deadView.Init(_playerEntryPoint);
            // _leaderBoardView.Init();
            //
            // _gameView.HideView();
            // _deadView.HideView();
            // _mainMenuView.HideView();
            // _leaderBoardView.HideView(0f);
            // _tutorialView.ShowView(0f);
            //
            // _gameEventHandler.OnPlayerStartMoving += PlayerStartMoving;
            // _gameEventHandler.OnPlayerDied += OnPlayerDied;
            // _gameEventHandler.OnPlayerResumed += OnPlayerResumed;
            //
            // _gameView.OnToMenuButtonPressed += ToMenuButtonPressed;
            // _mainMenuView.OnBackButtonClicked += MainMenuBackButtonClicked;
            // _mainMenuView.OnToLeaderboardButtonPressed += ToLeaderboardFromMenu;
            // _leaderBoardView.OnBackButtonPressed += LeaderBoardBackButtonPressed;
            //
            // await UniTask.Delay(500);
            // _loadingView.HideView(0.5f);
        }

        public void Destruct()
        {
            _viewModelStorage.ClearAllViewModels();
            UnsubscribeFromEvents();
            // _gameView.Destruct();
            // _deadView.Destruct();
            // _mainMenuView.Destruct();
            //
            // _gameEventHandler.OnPlayerStartMoving -= PlayerStartMoving;
            // _gameEventHandler.OnPlayerDied -= OnPlayerDied;
            // _gameEventHandler.OnPlayerResumed -= OnPlayerResumed;
            //
            // _gameView.OnToMenuButtonPressed -= ToMenuButtonPressed;
            // _mainMenuView.OnBackButtonClicked -= MainMenuBackButtonClicked;
            // _leaderBoardView.OnBackButtonPressed -= LeaderBoardBackButtonPressed;
            // _mainMenuView.OnToLeaderboardButtonPressed -= ToLeaderboardFromMenu;
        }

        // private void PlayerStartMoving()
        // {
        //     if(_isFirstTap) return;
        //     
        //     _isFirstTap = true;
        //     _tutorialView.HideView();
        //     _gameView.ShowView();
        // }
        //
        // private void OnPlayerDied()
        // {
        //     _mainMenuView.HideView();
        //     _leaderBoardView.HideView();
        //     _gameView.HideView();
        //     
        //     _deadView.ShowView();
        // }
        //
        // private void ToMenuButtonPressed()
        // {
        //     _gameView.HideView();
        //     _mainMenuView.ShowView();
        //     _gameEventHandler.InvokeOnMenuViewStatus(true);
        // }
        //
        // private void MainMenuBackButtonClicked()
        // {
        //     _mainMenuView.HideView();
        //     _gameView.ShowView();
        //     _gameEventHandler.InvokeOnMenuViewStatus(false);
        // }
        //
        // private void LeaderBoardBackButtonPressed()
        // {
        //     _mainMenuView.ShowView(0f);
        //     _leaderBoardView.HideView(0f);
        // }
        //
        // private void ToLeaderboardFromMenu()
        // {
        //     _mainMenuView.HideView();
        //     _leaderBoardView.ShowView(0f);
        // }
        //
        // private void OnPlayerResumed()
        // {
        //     _deadView.HideView();
        //     _gameView.ShowView();
        //     _gameEventHandler.InvokeOnMenuViewStatus(false);
        // }


        private void SubscribeToEvents()
        {
            _gameEventHandler.OnPlayerStartMoving += PlayerStartMovingHandler;
            _gameEventHandler.OnCollectablesChanged += OnCollectablesChangedHandler;
            _gameEventHandler.OnPlayerDied += OnPlayerDiedHandler;
            
            _gameViewModel.OnMenuButtonClicked += OnMenuClickedHandler;
            _gameViewModel.OnGetRewardsButtonClicked += OnGetRewardButtonClichedHandler;
        }

        private void UnsubscribeFromEvents()
        {
            _gameEventHandler.OnPlayerStartMoving -= PlayerStartMovingHandler;
            _gameEventHandler.OnCollectablesChanged -= OnCollectablesChangedHandler;
            _gameEventHandler.OnPlayerDied -= OnPlayerDiedHandler;
            
            _gameViewModel.OnMenuButtonClicked -= OnMenuClickedHandler;
            _gameViewModel.OnGetRewardsButtonClicked -= OnGetRewardButtonClichedHandler;
        }

        private void PlayerStartMovingHandler()
        {
            _gameViewModel.DistanceUpdatedCommand.Execute(_playerModel.CurrentDistance);
            
            if(_isFirstTap == false) return;
            _isFirstTap = false;
            _tutorialViewModel.CloseView();
        }

        private void OnCollectablesChangedHandler(int _)
        {
            _gameViewModel.BitUpdatedCommand.Execute(_playerModel.Bits);
        }

        private void OnPlayerDiedHandler()
        {
            _viewModelStorage.CloseAllViewModels();
            _deadViewModel.OpenView();
        }

        private void OnMenuClickedHandler()
        {
            
        }

        private void OnGetRewardButtonClichedHandler()
        {
           
        }
    }
}