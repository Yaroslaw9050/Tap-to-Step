using Core.Service.GlobalEvents;
using UI.ViewModels;
using UnityEngine.SceneManagement;

namespace UI.Views.Controller
{
    public sealed class ViewController
    {
        private LoadingViewModel _loadingViewModel;
        private TutorialViewModel _tutorialViewModel;
        private GameViewModel _gameViewModel;
        private DeadViewModel _deadViewModel;
        private MainMenuViewModel _mainMenuViewModel;

        private readonly GlobalEventsHolder r_globalEventsHolder;
        private readonly IViewModelStorageService r_viewModelStorage;
        
        private bool _isFirstTap;

        public ViewController(GlobalEventsHolder globalEventsHolder, 
            IViewModelStorageService viewModelStorage)
        {
            r_globalEventsHolder = globalEventsHolder;
            r_viewModelStorage = viewModelStorage;
        }

        public void Initialize()
        {
            _isFirstTap = true;
            
            _loadingViewModel = r_viewModelStorage.GetViewMode<LoadingViewModel>();
            _tutorialViewModel = r_viewModelStorage.GetViewMode<TutorialViewModel>();
            _gameViewModel = r_viewModelStorage.GetViewMode<GameViewModel>();
            _deadViewModel = r_viewModelStorage.GetViewMode<DeadViewModel>();
            _mainMenuViewModel = r_viewModelStorage.GetViewMode<MainMenuViewModel>();
            
            SubscribeToEvents();
        }

        public void Destruct()
        {
            r_viewModelStorage.ClearAllViewModels();
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
            r_globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(true);
        }
        
        private void SubscribeToEvents()
        {
            r_globalEventsHolder.PlayerEvents.OnStartMoving += PlayerStartMovingHandler;
            r_globalEventsHolder.PlayerEvents.OnDied += OnPlayerDiedHandler;
        }

        private void UnsubscribeFromEvents()
        {
            r_globalEventsHolder.PlayerEvents.OnStartMoving -= PlayerStartMovingHandler;
            r_globalEventsHolder.PlayerEvents.OnDied -= OnPlayerDiedHandler;
        }

        public void ShowGameView()
        {
            r_globalEventsHolder.UIEvents.InvokeOnMainMenuIsOpen(false);
            r_viewModelStorage.CloseAllViewModels();
            _gameViewModel.OpenView();
            r_globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(true);
        }

        public void ShowMenuView()
        {
            r_globalEventsHolder.UIEvents.InvokeOnMainMenuIsOpen(true);
            r_viewModelStorage.CloseAllViewModels();
            _mainMenuViewModel.OpenView();
            r_globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(false);
        }

        private void PlayerStartMovingHandler()
        {
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

        private void OnPlayerDiedHandler()
        {
            //TODO: If user enter to ded view and exit - reset current distance
            
            r_viewModelStorage.CloseAllViewModels();
            r_globalEventsHolder.PlayerEvents.InvokeScreenInputStatusChanged(false);
            _deadViewModel.OpenView();
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