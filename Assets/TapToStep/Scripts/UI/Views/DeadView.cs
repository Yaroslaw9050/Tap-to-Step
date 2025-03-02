using Core.Extension.UI;
using TMPro;
using UI.ViewModels;
using UI.Views.Upgrades;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public sealed class DeadView: BaseView
    {
        [Header("Buttons:")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueByAdButton;

        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI _currentDistanceText;
        
        private DeadViewModel _deadViewModel;
        
        [Inject]
        public void Constructor(DeadViewModel deadViewModel)
        {
            _deadViewModel = deadViewModel;
        }
        
        protected override void SubscribeToEvents()
        {
            _restartButton.onClick.AddListener(() => _deadViewModel.RestartCommand.Execute());
            _continueByAdButton.onClick.AddListener(() => _deadViewModel.ContinueByAdCommand.Execute());
            
            _deadViewModel.OnViewActivityStatusChanged += OnViewStatusChanged;
            _deadViewModel.Distance.Subscribe(ReactCurrentDistanceUpdated).AddTo(_disposable);
        }

        protected override void UnSubscribeFromEvents()
        {
            _restartButton.onClick.RemoveAllListeners();
            _continueByAdButton.onClick.RemoveAllListeners();
            
            _deadViewModel.OnViewActivityStatusChanged -= OnViewStatusChanged;
        }

        private void OnViewStatusChanged(bool isActive)
        {
            if(isActive) ShowView();
            else HideView();
        }

        private void ReactCurrentDistanceUpdated(double currentDistance)
        {
            _currentDistanceText.SetText($"Distance: {ValueConvertor.ToDistance(currentDistance)}");
        }
    }
}