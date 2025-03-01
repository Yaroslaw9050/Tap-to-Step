using Patterns.ViewModels;

namespace Patterns.MVVM.ViewModels
{
    public sealed class MainMenuViewModel : ViewModel
    {
        public MainMenuViewModel(IViewModelStorageService viewModelStorageService): base(viewModelStorageService)
        {
        }
    }
}