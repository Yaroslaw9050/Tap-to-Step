using CompositionRoot.Constants;
using Patterns.ViewModels;
using UI.Views.Upgrades;
using Zenject;

namespace Patterns.Views
{
    public class TutorialView : BaseView
    {
        private TutorialViewModel _tutorialViewModel;
        
        [Inject]
        public void Constructor(TutorialViewModel tutorialViewModel)
        {
            _tutorialViewModel = tutorialViewModel;
        }

        protected override void SubscribeToEvents()
        {
            _tutorialViewModel.OnViewActivityStatusChanged += OnViewActivityStatusChanged;
        }

        protected override void UnSubscribeFromEvents()
        {
            _tutorialViewModel.OnViewActivityStatusChanged -= OnViewActivityStatusChanged;
        }

        private void OnViewActivityStatusChanged(bool isActive)
        {
            if(isActive) ShowView(ViewAnimationAssets.INSTANTLY);
            else HideView(ViewAnimationAssets.FAST);
        }
    }
}