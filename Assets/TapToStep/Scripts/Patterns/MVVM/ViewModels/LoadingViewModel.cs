namespace Patterns.ViewModels
{
    public sealed class LoadingViewModel: ViewModel
    {
        public LoadingViewModel(IViewModelStorageService viewModelStorageService)
        {
            viewModelStorageService.TryRegisterNewViewModel(this);
        }
    }
}