using System;
using Core.Extension.UI;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using Runtime.Audio;
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
        [SerializeField] private Button _toTelegramButton;
        [SerializeField] private GradientAutoRotation _gradientAutoRotation;

        private PlayerPerkSystem _playerPerkSystem;
        private GlobalEventsHolder _globalEventsHolder;
        private PlayerEntryPoint _playerEntryPoint;
        private LeaderboardService _leaderboardService;

        private const string TELEGRAM_URL = "https://t.me/+LBbm-wqA7uk0MTIy";
        public event Action OnBackButtonClicked;
        public event Action OnToLeaderboardButtonPressed;
        
        [Inject]
        public void Constructor(GlobalEventsHolder globalEventsHolder, PlayerPerkSystem playerPerkSystem,
            AudioController audioController, LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
            _globalEventsHolder = globalEventsHolder;
            _playerPerkSystem = playerPerkSystem;
        }
        
        public void Init(PlayerEntryPoint playerEntryPoint)
        {
            _playerEntryPoint = playerEntryPoint;
            _upgradeHolderSubView.Init(_globalEventsHolder, _playerPerkSystem, _playerEntryPoint, _leaderboardService);
            
            _backButton.onClick.AddListener(BackButtonCLicked);
            _toLeaderboardButton.onClick.AddListener(ToLeaderboardButtonClicked);
            _toTelegramButton.onClick.AddListener(()=> Application.OpenURL(TELEGRAM_URL));
        }

        public void Destruct()
        {
            _gradientAutoRotation.Destruct();
            _backButton.onClick.RemoveListener(BackButtonCLicked);
            _toLeaderboardButton.onClick.RemoveListener(ToLeaderboardButtonClicked);
            _toTelegramButton.onClick.RemoveAllListeners();
        }

        protected override void SubscribeToEvents()
        {
            
        }

        protected override void UnSubscribeFromEvents()
        {
            
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _gradientAutoRotation.PlayAnimation();
            _upgradeHolderSubView.DisplayActualValues();
            _toLeaderboardButton.interactable = _leaderboardService.SystemReady;
            
            TrySaveUserDistanceAsync().Forget();
        }

        public override void HideView(float duration = 0f)
        {
            base.HideView(duration);
            _gradientAutoRotation.StopAnimation();
        }

        private void BackButtonCLicked()
        {
            //_globalEventsHolder.InvokeOnUiElementClicked();
            OnBackButtonClicked?.Invoke();
        }

        private void ToLeaderboardButtonClicked()
        {
            OnToLeaderboardButtonPressed?.Invoke();
            //_globalEventsHolder.InvokeOnUiElementClicked();
        }

        private async UniTaskVoid TrySaveUserDistanceAsync()
        {
            await _leaderboardService.UpdateUserDistanceAsync(_playerEntryPoint.PlayerStatistic.Distance);
        }
    }
}