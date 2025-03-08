using System.Collections.Generic;
using CompositionRoot.Constants;
using Core.Extension.UI;
using Core.Service.Leaderboard;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [Header("Builders:")]
        [SerializeField] private LeaderBoardBuilder _boardBuilder;

        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _bitsText;
        
        [Header("Buttons:")]
        [SerializeField] private Button _backButton;
        
        private LeaderBoardViewModel _viewModel;
        
        [Inject]
        public void Constructor(LeaderBoardViewModel leaderBoardViewModel)
        {
            _viewModel = leaderBoardViewModel;
        }

        protected override void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => _viewModel.CloseLeaderboardCommand.Execute());
            _viewModel.OnViewActivityStatusChanged += OnViewActivityStatusChanged;

            _viewModel.Top100UsersUpdated.Subscribe(DisplayTop100Users).AddTo(_disposable);
            _viewModel.Bits.Subscribe(ReactBitsUpdated).AddTo(_disposable);
            _viewModel.BlockInteraction.Subscribe(ReactBlockInteraction).AddTo(_disposable);
        }

        protected override void UnSubscribeFromEvents()
        {
            _backButton.onClick.RemoveAllListeners();
            _viewModel.OnViewActivityStatusChanged -= OnViewActivityStatusChanged;
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
            _bitsText.DOColor(Color.magenta, 0.5f).OnComplete(() => _bitsText.DOColor(Color.white, 1f));
        }

        private void ReactBlockInteraction(bool isBlocked)
        {
            _thisViewCanvasGroup.interactable = !isBlocked;
        }

        private void DisplayTop100Users(List<LeaderboardUser> users)
        {
            if (users.Count == 0)
            {
                _boardBuilder.DestroyBoard();
                return;
            }
            
            _boardBuilder.CreateBoardAsync(users).Forget();
        }
    }
}