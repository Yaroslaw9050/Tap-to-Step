using CompositionRoot.Constants;
using Core.Extension.UI;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using TMPro;
using UI.ViewModels;
using UI.Views.LeaderBoard;
using UI.Views.Upgrades;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class LeaderBoardView: BaseView
    {
        [Header("Sub-Views:")]
        [SerializeField] private UserInfoSubView _userInfoSubView;
        
        [Header("Builders:")]
        [SerializeField] private LeaderBoardBuilder _boardBuilder;

        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _bitsText;
        
        [Header("Buttons:")]
        [SerializeField] private Button _backButton;
        
        private LeaderBoardViewModel _viewModel;
        private LeaderboardService _leaderboardService;
        
        [Inject]
        public void Constructor(LeaderBoardViewModel leaderBoardViewModel, LeaderboardService leaderboardService)
        {
            _viewModel = leaderBoardViewModel;
            _leaderboardService = leaderboardService;
        }
        
        protected override void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => _viewModel.CloseLeaderboardCommand.Execute());
            _viewModel.OnViewActivityStatusChanged += OnViewActivityStatusChanged;

            _viewModel.Bits.Subscribe(ReactBitsUpdated).AddTo(_disposable);
        }

        protected override void UnSubscribeFromEvents()
        {
            _backButton.onClick.RemoveAllListeners();
            _viewModel.OnViewActivityStatusChanged -= OnViewActivityStatusChanged;
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            
        }

        private void OnViewActivityStatusChanged(bool isActive)
        {
            if (isActive)
            {
                ShowView(ViewAnimationAssets.FAST);
            }
            else HideView(ViewAnimationAssets.FAST);
        }
        
        private void ReactBitsUpdated(ulong newValue)
        {
            _bitsText.SetText(ValueConvertor.ToBits(newValue));
        }
    }
}