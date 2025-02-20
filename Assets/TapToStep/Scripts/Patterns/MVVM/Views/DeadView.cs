using Patterns.ViewModels;
using TMPro;
using UI.Views.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Patterns.Views
{
    public class DeadView: BaseView
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
            _restartButton.onClick.AddListener(_deadViewModel.RestartButtonClicked.Execute);
            _continueByAdButton.onClick.AddListener(_deadViewModel.ContinueByAdButtonClicked.Execute);
            
            _deadViewModel.OnViewActivityStatusChanged += OnViewStatusChanged;
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
    }
}