using System;
using System.Threading;
using Core.Service.AdMob;
using Core.Service.AdMob.Enums;
using Core.Service.GlobalEvents;
using Core.Service.LocalDataStorage;
using Core.Service.LocalUser;
using Cysharp.Threading.Tasks;
using UI.Views.Controller;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.ViewModels
{
    public sealed class DeadViewModel: ViewModel
    {
        private CancellationTokenSource _adsCts;
        private int _diedCounter;
        private int _continueCounter;

        private readonly ILocalDataStorageService r_localDataStorageService;
        private readonly GlobalEventsHolder r_globalEventsHolder;
        private readonly ViewController r_viewController;
        private readonly IMobileAdsService r_mobileAdsService;
        private readonly LocalPlayerService r_localPlayerService;
        
        public readonly ReactiveCommand<bool> ChangeAdButtonStatus = new();
        public readonly ReactiveCommand RestartCommand = new();
        public readonly ReactiveCommand ContinueByAdCommand = new();
        
        public readonly ReactiveProperty<double> Distance = new(0);

        private const int MAX_DEAD_COUNTER = 3;
        private const int MAX_CONTINUE_COUNTER = 2;
        
        private const string DEAD_COUNTER_KEY = "DeadCounter";
        private const string CONTINUE_COUNTER_KEY = "ContinueValue";
        
        public DeadViewModel(ILocalDataStorageService localDataStorageService, GlobalEventsHolder globalEventsHolder, 
            ViewController viewController, IMobileAdsService mobileAdsService, LocalPlayerService localPlayerService,
            IViewModelStorageService viewModelStorageService): base
            (viewModelStorageService)
        {
            r_localDataStorageService = localDataStorageService;
            r_globalEventsHolder = globalEventsHolder;
            r_viewController = viewController;
            r_mobileAdsService = mobileAdsService;
            r_localPlayerService = localPlayerService;
            RestartCommand.Subscribe(RestartCommandExecuted()).AddTo(r_disposables);
            ContinueByAdCommand.Subscribe(ContinueByAdCommandExecuted()).AddTo(r_disposables);

            localPlayerService.PlayerModel.CurrentDistance
                .Subscribe(newValue => Distance.Value = newValue)
                .AddTo(r_disposables);
        }

        public override void OpenView()
        {
            base.OpenView();
            PreparingViewAsync().Forget();
        }

        public override void CloseView()
        {
            base.CloseView();
            _adsCts?.Cancel();
        }

        private Action<Unit> ContinueByAdCommandExecuted()
        {
            return _ =>
            {
                _continueCounter++;
                r_localDataStorageService.SaveInt(CONTINUE_COUNTER_KEY, _continueCounter);
                ShowContinueAdAsync().Forget();
            };
        }

        private Action<Unit> RestartCommandExecuted()
        {
            return _ =>
            {
                _diedCounter++;
                r_localDataStorageService.SaveInt(DEAD_COUNTER_KEY, _diedCounter);
                r_localDataStorageService.SaveInt(CONTINUE_COUNTER_KEY, 0);
                r_localPlayerService.ResetCurrentDistance();
                var scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            };
        }

        private void LoadLocalData()
        {
            if (r_localDataStorageService.TryLoadInt(DEAD_COUNTER_KEY, out var deadValue) == false) return;
            if (r_localDataStorageService.TryLoadInt(CONTINUE_COUNTER_KEY, out var continueValue) == false) return;
            _diedCounter = deadValue;
            _continueCounter = continueValue;
            
            Debug.Log($"Dead: {_diedCounter} | Continue: {_continueCounter}");
        }

        private void CheckButtonStatus()
        {
            ChangeAdButtonStatus.Execute(_continueCounter < MAX_CONTINUE_COUNTER);
            if (_diedCounter >= MAX_DEAD_COUNTER)
            {
                _diedCounter = 0;
                r_localDataStorageService.SaveInt(DEAD_COUNTER_KEY, _diedCounter);
                ShowDeadAdAsync().Forget();
            }
        }

        private async UniTask PreparingViewAsync()
        {
            LoadLocalData();
            await PreparingAdsAsync();
            CheckButtonStatus();
        }

        private async UniTaskVoid ShowContinueAdAsync()
        {
            await r_mobileAdsService.ShowInterstitialAdAsync(InterstitialAdType.DeadViewContinue, _adsCts.Token);
            r_viewController.ShowGameView();
            r_globalEventsHolder.PlayerEvents.InvokeOnReborn();
        }

        private async UniTaskVoid ShowDeadAdAsync()
        {
            await r_mobileAdsService.ShowInterstitialAdAsync(InterstitialAdType.AfterDead, _adsCts.Token);
        }

        private async UniTask PreparingAdsAsync()
        {
            _adsCts = new CancellationTokenSource();
            var result = await r_mobileAdsService.LoadInterstitialAdAsync(InterstitialAdType.DeadViewContinue, _adsCts.Token);
            ChangeAdButtonStatus.Execute(result == LoadStatus.Success);
            await r_mobileAdsService.LoadInterstitialAdAsync(InterstitialAdType.AfterDead, _adsCts.Token);
        }
    }
}