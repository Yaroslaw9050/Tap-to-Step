using System;
using UnityEngine;

namespace Patterns.ViewModels
{
    public class ViewModel
    {
        public event Action<bool> OnViewActivityStatusChanged;
        
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