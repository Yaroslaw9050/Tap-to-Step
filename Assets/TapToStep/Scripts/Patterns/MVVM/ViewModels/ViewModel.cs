using System;
using UniRx;

namespace Patterns.MVVM.ViewModels
{
    public class ViewModel
    {
        protected readonly CompositeDisposable r_disposables; 
        
        public event Action<bool> OnViewActivityStatusChanged;

        protected ViewModel(IViewModelStorageService viewModelStorageService)
        {
            r_disposables = new CompositeDisposable();
            viewModelStorageService.TryRegisterNewViewModel(this);
        }
        
        public virtual void OpenView()
        {
            OnViewActivityStatusChanged?.Invoke(true);
        }
        
        public virtual void CloseView()
        {
            OnViewActivityStatusChanged?.Invoke(false);
        }

        public virtual void Dispose()
        {
            r_disposables.Dispose();
        }
    }
}