namespace Patterns.ViewModels
{
    public sealed class LoadingViewModel: ViewModel
    {
        public LoadingViewModel(IViewModelStorageService viewModelStorageService): base(viewModelStorageService)
        {
        }
    }
}