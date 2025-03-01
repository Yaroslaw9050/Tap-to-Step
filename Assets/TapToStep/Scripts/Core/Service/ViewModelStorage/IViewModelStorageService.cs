using Patterns.MVVM.ViewModels;
using Patterns.ViewModels;

public interface IViewModelStorageService
{
    public bool TryRegisterNewViewModel<T>(T regView) where T: ViewModel;
    public bool TryRemoveViewModel<T>(T rmvView) where T : ViewModel;

    public void ClearAllViewModels();
    public bool TryGetViewModel<T>(out T selectedView) where T: ViewModel;

    public T GetViewMode<T>() where T : ViewModel;

    public void CloseAllViewModels();
}