using System;
using Core.Service.LocalUser;
using UI.Views.Controller;
using UniRx;

namespace UI.ViewModels
{
    public class LeaderBoardViewModel: ViewModel
    {
        private readonly ViewController r_viewController;
        public ReactiveCommand CloseLeaderboardCommand { get; } = new();
        public ReactiveProperty<ulong> Bits { get; } = new(0);

        protected LeaderBoardViewModel(LocalPlayerService localPlayerService, ViewController viewController, IViewModelStorageService 
                viewModelStorageService) : base
            (viewModelStorageService)
        {
            r_viewController = viewController;
            SubscribeReactive(localPlayerService);
        }

        private void SubscribeReactive(LocalPlayerService localPlayerService)
        {
            CloseLeaderboardCommand.Subscribe(OoCloseLeaderboardCommandExecuted()).AddTo(r_disposables);
            
            localPlayerService.PlayerModel.Bits
                .Subscribe(BitValueChanged())
                .AddTo(r_disposables);
        }

        private Action<Unit> OoCloseLeaderboardCommandExecuted()
        {
            return _ => r_viewController.ShowMenuView();
        }
        
        private Action<ulong> BitValueChanged()
        {
            return newValue => Bits.Value = newValue;
        }
    }
}