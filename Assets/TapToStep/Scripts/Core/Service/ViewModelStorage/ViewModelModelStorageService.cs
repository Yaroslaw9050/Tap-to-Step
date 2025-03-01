using System;
using System.Collections.Generic;
using Patterns.MVVM.ViewModels;
using Patterns.ViewModels;
using UnityEngine;

namespace TapToStep.Scripts.Core.Service.ViewStorage
{
    public class ViewModelModelStorageService: IViewModelStorageService
    {
        private readonly Dictionary<Type, ViewModel> r_viewModelsDictionary = new();
        
        public bool TryRegisterNewViewModel<T>(T regView) where T : ViewModel
        {
            if(r_viewModelsDictionary.TryAdd(regView.GetType(), regView))
            {
                regView.CloseView();
                return true;
            }
            
            Debug.LogWarning($"{nameof(ViewModelModelStorageService)} | You trying to add a view ({typeof(T)}), but this view added before.");
            return false;
        }

        public bool TryRemoveViewModel<T>(T rmvView) where T : ViewModel
        {
            if (r_viewModelsDictionary.TryGetValue(rmvView.GetType(), out var regView))
            {
                r_viewModelsDictionary.Remove(regView.GetType());
                return true;
            }
            
            Debug.LogWarning($"{nameof(ViewModelModelStorageService)} | You trying to remove a view ({typeof(T)}) that is not registered.");
            return false;
        }

        public bool TryGetViewModel<T>(out T selectedView) where T : ViewModel
        {
            if (r_viewModelsDictionary.TryGetValue(typeof(T), out var regView))
            {
                selectedView = regView as T;
                return true;
            }
            
            selectedView = null;
            Debug.LogError($"{nameof(ViewModelModelStorageService)} | You trying to get a view ({typeof(T)}) that is not registered!");
            return false;
        }

        public T GetViewMode<T>() where T : ViewModel
        {
            if (r_viewModelsDictionary.TryGetValue(typeof(T), out var regView))
            {
                return regView as T;
            }

            return null;
        }

        public void ClearAllViewModels()
        {
            CloseAllViewModels();
            DisposeAllViewModels();
            r_viewModelsDictionary.Clear();
        }

        public void CloseAllViewModels()
        {
            foreach (var key in r_viewModelsDictionary.Keys)
            {
                r_viewModelsDictionary[key].CloseView();
            }
        }

        private void DisposeAllViewModels()
        {
            foreach (var key in r_viewModelsDictionary.Keys)
            {
                r_viewModelsDictionary[key].Dispose();
            }
        }
    }
}