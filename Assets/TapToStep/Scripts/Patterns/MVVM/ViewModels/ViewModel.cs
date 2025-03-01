using System;

namespace Patterns.ViewModels
{
    public class ViewModel
    {
        public event Action<bool> OnViewActivityStatusChanged;

        protected ViewModel(IViewModelStorageService viewModelStorageService)
        {
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
    }
}