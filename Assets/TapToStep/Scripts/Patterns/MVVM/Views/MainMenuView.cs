using CompositionRoot.Constants;
using Core.Extension.UI;
using Core.Service.GlobalEvents;
using Core.Service.Leaderboard;
using Core.Service.LocalUser;
using Patterns.MVVM.ViewModels;
using Runtime.Player.Upgrade;
using TMPro;
using UI.Views.Upgrades;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Patterns.Views
{
    public sealed class MainMenuView: BaseView
    {
        [Header("Buttons:")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _toLeaderboardButton;
        [SerializeField] private Button _toTelegramButton;

        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _bitsText;
        
        [Header("SubViews:")]
        [SerializeField] private UpgradeHolderSubView _upgradeHolderSubView;
        
        private MainMenuViewModel _viewModel;
        
        [Inject]
        public void Constructor(MainMenuViewModel mainMenuViewModel,
            GlobalEventsHolder eventsHolder, LocalPlayerService localPlayerService,
            PlayerPerkSystem perkSystem, LeaderboardService leaderboardService)
        {
            _viewModel = mainMenuViewModel;
            _upgradeHolderSubView.Init(eventsHolder, perkSystem, localPlayerService, leaderboardService);
        }
        
        protected override void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => _viewModel.CloseMainMenuCommand.Execute());
            _toLeaderboardButton.onClick.AddListener(() => _viewModel.OpenLeaderBoardCommand.Execute());
            _toTelegramButton.onClick.AddListener(() => _viewModel.ToTelegramCommunityCommand.Execute());
            
            _viewModel.OnViewActivityStatusChanged += OnViewActivityStatusChanged;

            _viewModel.Bits.Subscribe(ReactBitsUpdated).AddTo(_disposable);
            
        }

        protected override void UnSubscribeFromEvents()
        {
            _backButton.onClick.RemoveAllListeners();
            _toLeaderboardButton.onClick.RemoveAllListeners();
            _toTelegramButton.onClick.RemoveAllListeners();
            
            _viewModel.OnViewActivityStatusChanged -= OnViewActivityStatusChanged;
        }

        private void OnViewActivityStatusChanged(bool isActive)
        {
            if (isActive)
            {
                ShowView(ViewAnimationAssets.BASE);
                _upgradeHolderSubView.DisplayActualValues();
            }
            else HideView(ViewAnimationAssets.FAST);
        }
        
        private void ReactBitsUpdated(ulong newValue)
        {
            _bitsText.SetText(ValueConvertor.ToBits(newValue));
        }
    }
}