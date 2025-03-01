using Core.Service.GlobalEvents;
using Patterns.MVVM.ViewModels;
using Patterns.ViewModels;
using Runtime.Player;
using TapToStep.Scripts.Core.Service.LocalUser;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.Views
{
    public class ViewController : MonoBehaviour
    {
        private LoadingViewModel _loadingViewModel;
        private TutorialViewModel _tutorialViewModel;
        private GameViewModel _gameViewModel;
        private DeadViewModel _deadViewModel;
        private MainMenuViewModel _mainMenuViewModel;

        private GlobalEventsHolder _globalEventsHolder;
        private PlayerEntryPoint _playerEntryPoint;
        private LocalPlayerService _localPlayerService;
        private IViewModelStorageService _viewModelStorage;
        
        private bool _isFirstTap;
        
        [Inject]
        public void Constructor(GlobalEventsHolder globalEventsHolder, LocalPlayerService localPlayerService, 
            IViewModelStorageService viewModelStorage)  
        {
            _globalEventsHolder = globalEventsHolder;
            _viewModelStorage = viewModelStorage;
            _localPlayerService = localPlayerService;
        }

        public void Initialize()
        {
            _isFirstTap = true;
            
            _loadingViewModel = _viewModelStorage.GetViewMode<LoadingViewModel>();
            _tutorialViewModel = _viewModelStorage.GetViewMode<TutorialViewModel>();
            _gameViewModel = _viewModelStorage.GetViewMode<GameViewModel>();
            _deadViewModel = _viewModelStorage.GetViewMode<DeadViewModel>();
            _mainMenuViewModel = _viewModelStorage.GetViewMode<MainMenuViewModel>();
            
            SubscribeToEvents();
        }

        public void Destruct()
        {
            _viewModelStorage.ClearAllViewModels();
            UnsubscribeFromEvents();
        }

        public void DisplayPreparingViews()
        {
            _loadingViewModel.OpenView();
            _tutorialViewModel.OpenView();
        }

        public void DisplayGameLoopViews()
        {
            _loadingViewModel.CloseView();
            _globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(true);
        }
        
        private void SubscribeToEvents()
        {
            _globalEventsHolder.PlayerEvents.OnStartMoving += PlayerStartMovingHandler;
            _globalEventsHolder.OnCollectablesChanged += OnCollectablesChangedHolder;
            _globalEventsHolder.PlayerEvents.OnDied += OnPlayerDiedHandler;

            _gameViewModel.OnMenuButtonClicked += OnMenuClickedHandler;
            _gameViewModel.OnGetRewardsButtonClicked += OnGetRewardButtonClichedHandler;
            
            _deadViewModel.OnRestartButtonClicked += OnRestartButtonClicked;
        }

        private void UnsubscribeFromEvents()
        {
            _globalEventsHolder.PlayerEvents.OnStartMoving -= PlayerStartMovingHandler;
            _globalEventsHolder.OnCollectablesChanged -= OnCollectablesChangedHolder;
            _globalEventsHolder.PlayerEvents.OnDied -= OnPlayerDiedHandler;
            
            _gameViewModel.OnMenuButtonClicked -= OnMenuClickedHandler;
            _gameViewModel.OnGetRewardsButtonClicked -= OnGetRewardButtonClichedHandler;
            
            _deadViewModel.OnRestartButtonClicked -= OnRestartButtonClicked;
        }

        private void PlayerStartMovingHandler()
        {
            _gameViewModel.DistanceUpdatedCommand.Execute(_localPlayerService.PlayerModel.CurrentDistance);
            CheckIsFirstTap();
        }

        private void CheckIsFirstTap()
        {
            if (_isFirstTap)
            {
                _isFirstTap = false;
                _tutorialViewModel.CloseView();
                _gameViewModel.OpenView();
            }
        }

        private void OnCollectablesChangedHolder()
        {
            _gameViewModel.BitUpdatedCommand.Execute(_localPlayerService.PlayerModel.Bits);
        }

        private void OnPlayerDiedHandler()
        {
            //TODO: If user enter to ded view and exit - reset current distance
            
            _viewModelStorage.CloseAllViewModels();
            _globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(false);
            _deadViewModel.DisplayCurrentDistance.Execute(_localPlayerService.PlayerModel.CurrentDistance);
            _deadViewModel.OpenView();
        }

        private void OnMenuClickedHandler()
        {
            _mainMenuViewModel.OpenView();
            _globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(false);
        }

        private void OnGetRewardButtonClichedHandler()
        {
           
        }

        private void OnRestartButtonClicked()
        {
            //TODO: Reset current player distance
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}