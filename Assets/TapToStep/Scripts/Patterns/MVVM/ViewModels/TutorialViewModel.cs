using Patterns.MVVM.ViewModels;

namespace Patterns.ViewModels
{
    public sealed class TutorialViewModel : ViewModel
    {
        public TutorialViewModel(IViewModelStorageService viewModelStorageService): base(viewModelStorageService)
        {
        }
    }
}