using System;
using CompositionRoot.Constants;
using Core.Service.LocalUser;
using UI.Views.Controller;
using UniRx;
using UnityEngine;

namespace UI.ViewModels
{
    public sealed class MainMenuViewModel : ViewModel
    {
        private readonly ViewController r_viewController;
        public ReactiveCommand CloseMainMenuCommand { get; } = new();
        public ReactiveCommand OpenLeaderBoardCommand { get; } = new();
        public ReactiveCommand ToTelegramCommunityCommand { get; } = new();

        public ReactiveProperty<ulong> Bits { get; } = new(0);

        public MainMenuViewModel(ViewController viewController, LocalPlayerService localPlayerService, IViewModelStorageService 
                viewModelStorageService): base
            (viewModelStorageService)
        {
            r_viewController = viewController;
            SubscribeReactive(localPlayerService);
        }

        private void SubscribeReactive(LocalPlayerService localPlayerService)
        {
            CloseMainMenuCommand.Subscribe(OnBackToMenuCommandExecuted()).AddTo(r_disposables);
            OpenLeaderBoardCommand.Subscribe(OnLeaderBoardCommandExecuted()).AddTo(r_disposables);
            ToTelegramCommunityCommand.Subscribe(OnTelegramCommandExecuted()).AddTo(r_disposables);

            localPlayerService.PlayerModel.Bits
                .Subscribe(BitValueChanged())
                .AddTo(r_disposables);
        }

        private Action<Unit> OnTelegramCommandExecuted()
        {
            return _ => Application.OpenURL(NetworkAssets.TELEGRAM_URL);
        }

        private Action<Unit> OnBackToMenuCommandExecuted()
        {
            return _ => r_viewController.ShowGameView();
        }

        private Action<Unit> OnLeaderBoardCommandExecuted()
        {
            return _ => Debug.Log("Open leaderboard");
        }

        private Action<ulong> BitValueChanged()
        {
            return newValue => Bits.Value = newValue;
        }
    }
}