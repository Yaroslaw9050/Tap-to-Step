using System;
using System.Threading;
using Core.Service.AdMob;
using Core.Service.AdMob.Enums;
using Core.Service.GlobalEvents;
using Core.Service.LocalUser;
using Cysharp.Threading.Tasks;
using UI.Views.Controller;
using UniRx;

namespace UI.ViewModels
{
    public sealed class GameViewModel : ViewModel
    {
        private CancellationTokenSource _rewardAdCancellationTokenSource;
        
        private readonly LocalPlayerService r_localPlayerService;
        private readonly GlobalEventsHolder r_globalEventsHolder;
        private readonly IMobileAdsService r_mobileAdsService;
        public ReactiveCommand OpenMainMenuCommand { get; } = new();
        public ReactiveCommand GetRewardCommand { get; } = new();
        public ReactiveCommand<bool> RewardAdStatusChanged { get; } = new();
        
        public ReactiveProperty<double> RewardAdAmount { get; } = new(0.0);
        public ReactiveProperty<double> Distance { get; } = new(0.0);
        public ReactiveProperty<ulong> Bits { get; } = new(0);

        public ReactiveCommand<float> PlayerStartMoveCommand { get; } = new();
        
        public const int REWARD_AD_SHOW_DELAY = 40_000;
        
        
        public GameViewModel(LocalPlayerService localPlayerService, GlobalEventsHolder globalEventsHolder,
            IViewModelStorageService viewModelStorageService, ViewController viewController, IMobileAdsService mobileAdsService)
            : base(viewModelStorageService)
        {
            r_localPlayerService = localPlayerService;
            r_globalEventsHolder = globalEventsHolder;
            r_mobileAdsService = mobileAdsService;

            SubscribeReactive(localPlayerService, viewController);
        }

        private void SubscribeReactive(LocalPlayerService localPlayerService, ViewController viewController)
        {
            OpenMainMenuCommand.Subscribe(OnMenuCommandExecuted(viewController)).AddTo(r_disposables);
            GetRewardCommand.Subscribe(OnGetRewardCommandExecuted()).AddTo(r_disposables);

            localPlayerService.PlayerModel.Bits
                .Subscribe(BitValueChanged())
                .AddTo(r_disposables);
            
            localPlayerService.PlayerModel.CurrentDistance
                .Subscribe(DistanceValueChanged())
                .AddTo(r_disposables);
            
            r_globalEventsHolder.PlayerEvents.OnStartMoving += OnPlayerStartMoving;
        }

        public override void OpenView()
        {
            base.OpenView();
            LoadRewardAdAsync().Forget();
        }

        public override void CloseView()
        {
            base.CloseView();
            _rewardAdCancellationTokenSource?.Cancel();
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
            return _ =>
            {
                ShowRewardAdAsync().Forget();
            };
        }

        private Action<Unit> OnMenuCommandExecuted(ViewController viewController)
        {
            return _ => viewController.ShowMenuView();
        }

        private void OnPlayerStartMoving()
        {
            PlayerStartMoveCommand.Execute(r_localPlayerService.GetStepTime());
        }

        private async UniTaskVoid LoadRewardAdAsync()
        {
            _rewardAdCancellationTokenSource = new CancellationTokenSource();
            
            RewardAdStatusChanged.Execute(false);
            await UniTask.Delay(REWARD_AD_SHOW_DELAY, cancellationToken: _rewardAdCancellationTokenSource.Token);
            
            var (status, amount) = await r_mobileAdsService.LoadRewardAdAsync(RewardAdType.GameLoopGetBits, _rewardAdCancellationTokenSource.Token);
            if (status == LoadStatus.Success)
            {
                RewardAdAmount.Value = amount;
                RewardAdStatusChanged.Execute(true);
            }
        }

        private async UniTaskVoid ShowRewardAdAsync()
        {
            var reward = await r_mobileAdsService.ShowRewardAdAsync(RewardAdType.GameLoopGetBits);
            r_localPlayerService.AddBits((ushort)reward);
            LoadRewardAdAsync().Forget();
        }
    }
}