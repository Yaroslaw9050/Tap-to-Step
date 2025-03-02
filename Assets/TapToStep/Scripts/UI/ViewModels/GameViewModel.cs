using System;
using Core.Service.GlobalEvents;
using Core.Service.LocalUser;
using UI.Views.Controller;
using UniRx;

namespace UI.ViewModels
{
    public sealed class GameViewModel : ViewModel
    {
        private readonly LocalPlayerService r_localPlayerService;
        private readonly GlobalEventsHolder r_globalEventsHolder;
        public ReactiveCommand OpenMainMenuCommand { get; } = new();
        public ReactiveCommand GetRewardCommand { get; } = new();
        public ReactiveProperty<double> Distance { get; } = new(0.0);
        public ReactiveProperty<ulong> Bits { get; } = new(0);

        public ReactiveCommand<float> PlayerStartMoveCommand { get; } = new();
        
        public GameViewModel(LocalPlayerService localPlayerService, GlobalEventsHolder globalEventsHolder,
            IViewModelStorageService viewModelStorageService, ViewController viewController)
            : base(viewModelStorageService)
        {
            r_localPlayerService = localPlayerService;
            r_globalEventsHolder = globalEventsHolder;
            
            SubscribeReactive(localPlayerService, viewController);
        }

        private void SubscribeReactive(LocalPlayerService localPlayerService, ViewController viewController)
        {
            OpenMainMenuCommand.Subscribe(OnMenuCommandExecuted(viewController)).AddTo(r_disposables);
            GetRewardCommand.Subscribe(OnGetRewardCommandExecuted());

            localPlayerService.PlayerModel.Bits
                .Subscribe(BitValueChanged())
                .AddTo(r_disposables);
            
            localPlayerService.PlayerModel.CurrentDistance
                .Subscribe(DistanceValueChanged())
                .AddTo(r_disposables);
            
            r_globalEventsHolder.PlayerEvents.OnStartMoving += OnPlayerStartMoving;
        }

        public override void Dispose()
        {
            base.Dispose();
            r_globalEventsHolder.PlayerEvents.OnStartMoving -= OnPlayerStartMoving;
        }

        private Action<double> DistanceValueChanged()
        {
            return newValue => Distance.Value = newValue;
        }

        private Action<ulong> BitValueChanged()
        {
            return newValue => Bits.Value = newValue;
        }

        private Action<Unit> OnGetRewardCommandExecuted()
        {
            return _ => r_localPlayerService.AddBits(5000);
        }

        private Action<Unit> OnMenuCommandExecuted(ViewController viewController)
        {
            return _ => viewController.ShowMenuView();
        }

        private void OnPlayerStartMoving()
        {
            PlayerStartMoveCommand.Execute(r_localPlayerService.GetStepTime());
        }
    }
}