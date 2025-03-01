using CompositionRoot.Constants;
using Core.Extension.UI;
using MPUIKIT;
using Patterns.ViewModels;
using TMPro;
using UI.Views.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Patterns.Views
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
        
        private GameViewModel _gameViewModel;
        
        [Inject]
        public void Constructor(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
        }
        
        protected override void SubscribeToEvents()
        {
            _toMenuButton.onClick.AddListener(_gameViewModel.ToMenuCommand.Execute);
            _getRewardButton.onClick.AddListener(_gameViewModel.GetRewardCommand.Execute);
            _gameViewModel.OnViewActivityStatusChanged += OnViewStatusChangedHandler;
            _gameViewModel.OnDistanceUpdated += DistanceUpdateHandler;
            
            _gameViewModel.OnBitsUpdated += OnBitsUpdated;
        }

        protected override void UnSubscribeFromEvents()
        {
            _toMenuButton.onClick.RemoveAllListeners();
            _getRewardButton.onClick.RemoveAllListeners();
            
            _gameViewModel.OnViewActivityStatusChanged -= OnViewStatusChangedHandler;
            _gameViewModel.OnDistanceUpdated -= DistanceUpdateHandler;
        }

        private void OnViewStatusChangedHandler(bool isActive)
        {
            if (isActive) ShowView(ViewAnimationAssets.BASE);
            else HideView(ViewAnimationAssets.BASE);
        }

        private void OnBitsUpdated(ulong newValue)
        {
            _bitsText.SetText(ValueConvertor.ToBits(newValue));
        }

        private void DistanceUpdateHandler(double distance)
        {
            _distanceText.SetText($"Distance\n{ValueConvertor.ToDistance(distance)}");
        }
    }
}