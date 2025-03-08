using System;
using Core.Service.LocalUser;
using Cysharp.Threading.Tasks;
using UI.Models;
using UI.Views.Controller;
using UniRx;
using UnityEngine;

namespace UI.ViewModels
{
    public sealed class LeaderBoardViewModel: ViewModel
    {
        private string _tempNewUserName;
        
        private readonly LocalPlayerService r_localPlayerService;
        private readonly ViewController r_viewController;
        public ReactiveCommand CloseLeaderboardCommand { get; } = new();
        public ReactiveCommand SaveUserNameCommand { get; } = new();
        public ReactiveCommand<string> UserNameValueUpdated { get; } = new();
        public ReactiveCommand<bool> BlockInteraction { get; } = new();
        public ReactiveProperty<ulong> Bits { get; } = new(0);

        private LeaderBoardViewModel(LocalPlayerService localPlayerService, ViewController viewController, IViewModelStorageService 
            viewModelStorageService) : base
            (viewModelStorageService)
        {
            _tempNewUserName = string.Empty;
            r_viewController = viewController;
            r_localPlayerService = localPlayerService;
            SubscribeReactive();
        }

        private void SubscribeReactive()
        {
            CloseLeaderboardCommand.Subscribe(OoCloseLeaderboardCommandExecuted()).AddTo(r_disposables);
            UserNameValueUpdated.Subscribe(OnUserNameValueUpdatedCommandExecuted).AddTo(r_disposables);
            SaveUserNameCommand.Subscribe(OnSaveUserNameCommandExecuted()).AddTo(r_disposables);
            
            r_localPlayerService.PlayerModel.Bits
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

        private void OnUserNameValueUpdatedCommandExecuted(string newName)
        {
            _tempNewUserName = newName;
        }

        private Action<Unit> OnSaveUserNameCommandExecuted()
        {
            return _ => ChangeUserNameAsync().Forget();
        }

        private async UniTask ChangeUserNameAsync()
        {
            BlockInteraction.Execute(true);
            var result = await r_localPlayerService.TryChangeUserNameAsync(_tempNewUserName, PriceModel.CHANGE_NAME_PRICE);
            if (result == false)
            {
                var temp = r_localPlayerService.PlayerModel.UserName.Value;
                r_localPlayerService.PlayerModel.UserName.Value = string.Empty;
                r_localPlayerService.PlayerModel.UserName.Value = temp;
            }
            BlockInteraction.Execute(false);
        }
    }
}