namespace Patterns.ViewModels
{
    public sealed class TutorialViewModel : ViewModel
    {
        public TutorialViewModel(IViewModelStorageService viewModelStorageService)
        {
            viewModelStorageService.TryRegisterNewViewModel(this);
        }
        
    }
}