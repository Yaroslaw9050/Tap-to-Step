using System;
using Core.Extension.UI;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using Runtime.Audio;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player;
using Runtime.Player.Perks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Upgrades
{
    public class MainMenuView : BaseView
    {
        [Header("Sub-Views")]
        [SerializeField] private UpgradeHolderSubView _upgradeHolderSubView;
        
        [Header("Other settings")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _toLeaderboardButton;
        [SerializeField] private GradientAutoRotation _gradientAutoRotation;

        private PlayerPerkSystem _playerPerkSystem;
        private GameEventHandler _gameEventHandler;
        private PlayerEntryPoint _playerEntryPoint;
        private LeaderboardService _leaderboardService;
        public event Action OnBackButtonClicked;
        public event Action OnToLeaderboardButtonPressed;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler, PlayerPerkSystem playerPerkSystem,
            AudioController audioController, LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
            _gameEventHandler = gameEventHandler;
            _playerPerkSystem = playerPerkSystem;
        }
        
        public void Init(PlayerEntryPoint playerEntryPoint)
        {
            _playerEntryPoint = playerEntryPoint;
            _upgradeHolderSubView.Init(_gameEventHandler, _playerPerkSystem, _playerEntryPoint);
            
            _backButton.onClick.AddListener(BackButtonCLicked);
            _toLeaderboardButton.onClick.AddListener(ToLeaderboardButtonClicked);
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _gradientAutoRotation.PlayAnimation();
            _upgradeHolderSubView.DisplayActualValues();
            
            TrySaveUserDistanceAsync().Forget();
        }

        public override void HideView(float duration = 0f)
        {
            base.HideView(duration);
            _gradientAutoRotation.StopAnimation();
        }

        private void BackButtonCLicked()
        {
            _gameEventHandler.InvokeOnUiElementClicked();
            OnBackButtonClicked?.Invoke();
        }

        private void ToLeaderboardButtonClicked()
        {
            OnToLeaderboardButtonPressed?.Invoke();
            _gameEventHandler.InvokeOnUiElementClicked();
        }

        private async UniTaskVoid TrySaveUserDistanceAsync()
        {
            await _leaderboardService.UpdateUserDistanceAsync(_playerEntryPoint.PlayerStatistic.Distance);
        }
    }
}