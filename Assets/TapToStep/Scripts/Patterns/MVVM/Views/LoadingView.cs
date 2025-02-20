using CompositionRoot.Constants;
using Patterns.ViewModels;
using UI.Views.Upgrades;
using Zenject;

namespace Patterns.Views
{
    public class LoadingView : BaseView
    {
        private LoadingViewModel _loadingViewModel;
        
        [Inject]
        public void Constructor(LoadingViewModel loadingViewModel)
        {
            _loadingViewModel = loadingViewModel;
        }

        protected override void SubscribeToEvents()
        {
            _loadingViewModel.OnViewActivityStatusChanged += OnLoadingStatusChanged;
        }

        protected override void UnSubscribeFromEvents()
        {
            _loadingViewModel.OnViewActivityStatusChanged -= OnLoadingStatusChanged;
        }

        private void OnLoadingStatusChanged(bool isActive)
        {
            if (isActive) ShowView(ViewAnimationAssets.INSTANTLY);
            else HideView(ViewAnimationAssets.FAST);
        }
    }
}