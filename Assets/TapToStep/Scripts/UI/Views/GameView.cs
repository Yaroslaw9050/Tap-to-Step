using CompositionRoot.Constants;
using Core.Extension.UI;
using DG.Tweening;
using MPUIKIT;
using TMPro;
using UI.ViewModels;
using UI.Views.Upgrades;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class GameView : BaseView
    {
        [Header("Buttons:")]
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private Button _getRewardButton;

        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _bitsText;
        
        [Header("Images:")]
        [SerializeField] private MPImage _characterEnergyImage;
        
        private GameViewModel _viewModel;
        private Tween _energyAnimationTween;
        
        [Inject]
        public void Constructor(GameViewModel gameViewModel)
        {
            _viewModel = gameViewModel;
        }

        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _characterEnergyImage.fillAmount = 1f;
        }

        protected override void SubscribeToEvents()
        {
            _toMenuButton.onClick.AddListener(() => _viewModel.OpenMainMenuCommand.Execute());
            _getRewardButton.onClick.AddListener(() => _viewModel.GetRewardCommand.Execute());
            
            _viewModel.OnViewActivityStatusChanged += OnViewStatusChangedHandler;
            
            _viewModel.PlayerStartMoveCommand.Subscribe(ReactPlayEnergyAnimation).AddTo(_disposable);
            _viewModel.Distance.Subscribe(ReactDistanceUpdateHandler).AddTo(_disposable);
            _viewModel.Bits.Subscribe(ReactBitsUpdated).AddTo(_disposable);
        }

        protected override void UnSubscribeFromEvents()
        {
            _toMenuButton.onClick.RemoveAllListeners();
            _getRewardButton.onClick.RemoveAllListeners();
            
            _viewModel.OnViewActivityStatusChanged -= OnViewStatusChangedHandler;
        }

        private void OnViewStatusChangedHandler(bool isActive)
        {
            if (isActive) ShowView(ViewAnimationAssets.BASE);
            else HideView(ViewAnimationAssets.FAST);
        }

        private void ReactBitsUpdated(ulong newValue)
        {
            _bitsText.SetText(ValueConvertor.ToBits(newValue));
        }

        private void ReactDistanceUpdateHandler(double distance)
        {
            _distanceText.SetText($"Distance\n{ValueConvertor.ToDistance(distance)}");
        }
        
        private void ReactPlayEnergyAnimation(float duration)
        {
            _energyAnimationTween?.Kill();
            _characterEnergyImage.fillAmount = 0f;
            _energyAnimationTween = _characterEnergyImage.DOFillAmount(1f, duration).SetEase(Ease.Linear);
        }
    }
}