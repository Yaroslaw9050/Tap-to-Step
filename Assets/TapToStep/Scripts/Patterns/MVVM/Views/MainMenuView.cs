using CompositionRoot.Constants;
using Core.Extension;
using Patterns.MVVM.ViewModels;
using UI.Views.Upgrades;
using Zenject;

namespace Patterns.Views
{
    public sealed class MainMenuView: BaseView
    {
        private MainMenuViewModel _viewModel;
        
        [Inject]
        public void Constructor(MainMenuViewModel mainMenuViewModel)
        {
            _viewModel = mainMenuViewModel;
        }
        
        protected override void SubscribeToEvents()
        {
            _viewModel.OnViewActivityStatusChanged += OnViewActivityStatusChanged;
        }

        protected override void UnSubscribeFromEvents()
        {
            _viewModel.OnViewActivityStatusChanged -= OnViewActivityStatusChanged;
        }

        private void OnViewActivityStatusChanged(bool isActive)
        {
            if(isActive) ShowView(ViewAnimationAssets.SLOW);
            else HideView(ViewAnimationAssets.BASE);
        }
    }
}