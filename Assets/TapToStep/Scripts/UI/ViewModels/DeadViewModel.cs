using System;
using Core.Service.AdMob;
using Core.Service.LocalUser;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.ViewModels
{
    public sealed class DeadViewModel: ViewModel
    {
        private readonly IMobileAdsService r_mobileAdsService;
        private readonly LocalPlayerService r_localPlayerService;
        
        public readonly ReactiveCommand<bool> ChangeAdButtonStatus = new();
        public readonly ReactiveCommand RestartCommand = new();
        public readonly ReactiveCommand ContinueByAdCommand = new();
        
        public readonly ReactiveProperty<double> Distance = new(0);

        private int _diedCounter;
        
        public DeadViewModel(IMobileAdsService mobileAdsService, LocalPlayerService localPlayerService,
            IViewModelStorageService viewModelStorageService): base
            (viewModelStorageService)
        {
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
            CalculateDiedCount();
            //r_mobileAdsService.OnContinueAdRecorded += () => Debug.Log("Continue ad recorded!");
        }

        private Action<Unit> ContinueByAdCommandExecuted()
        {
            return _ =>
            {
                //r_mobileAdsService.LoadContinueAd();
            };
        }

        private Action<Unit> RestartCommandExecuted()
        {
            return _ =>
            {
                r_localPlayerService.SetCurrentDistance(0);
                var scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            };
        }

        private void CalculateDiedCount()
        {
            _diedCounter++;
            ChangeAdButtonStatus.Execute(_diedCounter < 2);
        }
        
    }
}