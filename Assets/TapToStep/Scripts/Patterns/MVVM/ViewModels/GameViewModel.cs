using System;
using Patterns.Commands;
using Patterns.Models;
using Runtime.EntryPoints.EventHandlers;

namespace Patterns.ViewModels
{
    public sealed class GameViewModel : ViewModel
    {
        private GameEventHandler _gameEventHandler;
        private readonly PlayerModel r_playerModel;
        public ICommand ToMenuCommand { get; }
        public ICommand GetRewardCommand { get; }
        public ICommand<ulong> BitUpdatedCommand { get; }
        public ICommand<double> DistanceUpdatedCommand { get; }

        public event Action OnMenuButtonClicked;
        public event Action OnGetRewardsButtonClicked;
        public event Action<double> OnDistanceUpdated;
        public event Action<ulong> OnBitsUpdated; 
        
        public GameViewModel(PlayerModel playerModel, GameEventHandler gameEventHandler,
            IViewModelStorageService viewModelStorageService)
        {
            viewModelStorageService.TryRegisterNewViewModel(this);
            
            r_playerModel = playerModel;
            _gameEventHandler = gameEventHandler;

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