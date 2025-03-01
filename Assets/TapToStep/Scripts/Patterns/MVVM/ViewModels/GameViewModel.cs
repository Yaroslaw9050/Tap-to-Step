using System;
using Core.Service.GlobalEvents;
using Patterns.Commands;
using Patterns.Models;
using UnityEngine;

namespace Patterns.ViewModels
{
    public sealed class GameViewModel : ViewModel
    {
        private GlobalEventsHolder _globalEventsHolder;
        public ICommand ToMenuCommand { get; }
        public ICommand GetRewardCommand { get; }
        public ICommand<ulong> BitUpdatedCommand { get; }
        public ICommand<double> DistanceUpdatedCommand { get; }

        public event Action OnMenuButtonClicked;
        public event Action OnGetRewardsButtonClicked;
        public event Action<double> OnDistanceUpdated;
        public event Action<ulong> OnBitsUpdated; 
        
        public GameViewModel(GlobalEventsHolder globalEventsHolder, IViewModelStorageService viewModelStorageService)
            : base(viewModelStorageService)
        {
            _globalEventsHolder = globalEventsHolder;
            ToMenuCommand = new Command(()=> OnMenuButtonClicked?.Invoke());
            GetRewardCommand = new Command(() =>  OnGetRewardsButtonClicked?.Invoke());
            
            DistanceUpdatedCommand = new Command<double>(UpdateDistanceHandler);
            BitUpdatedCommand = new Command<ulong>(UpdateBitsHandler);
        }
        
        private void UpdateDistanceHandler(double distance)
        {
            OnDistanceUpdated?.Invoke(distance);
        }

        private void UpdateBitsHandler(ulong bits)
        {
            OnBitsUpdated?.Invoke(bits);
        }
    }
}