using System;
using Patterns.Commands;
using UnityEngine.SceneManagement;

namespace Patterns.ViewModels
{
    public sealed class DeadViewModel: ViewModel
    {
        public ICommand RestartButtonClicked;
        public ICommand ContinueByAdButtonClicked;
        
        public ICommand<double> DisplayCurrentDistance;

        public event Action OnRestartButtonClicked;
        public event Action OnContinueByAdButtonClicked;
        public event Action<double> OnCurrentDistanceUpdated; 
        
        
        public DeadViewModel(IViewModelStorageService viewModelStorageService): base(viewModelStorageService)
        {
            RestartButtonClicked = new Command(OnRestartButtonClickedHandler);
            ContinueByAdButtonClicked = new Command(OnContinueByAdButtonClickedHandler);

            DisplayCurrentDistance = new Command<double>(OnCurrentDistanceUpdatedHandler);
        }

        private void OnCurrentDistanceUpdatedHandler(double currentDistance)
        {
            OnCurrentDistanceUpdated?.Invoke(currentDistance);
        }

        private void OnRestartButtonClickedHandler()
        {
            OnRestartButtonClicked?.Invoke();
        }

        private void OnContinueByAdButtonClickedHandler()
        {
            OnContinueByAdButtonClicked?.Invoke();
        }
    }
}