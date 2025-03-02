using System;
using Core.Service.LocalUser;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.ViewModels
{
    public sealed class DeadViewModel: ViewModel
    {
        private readonly LocalPlayerService r_localPlayerService;
        
        public readonly ReactiveCommand RestartCommand = new();
        public readonly ReactiveCommand ContinueByAdCommand = new();
        
        public readonly ReactiveProperty<double> Distance = new(0);
        
        public DeadViewModel(LocalPlayerService localPlayerService,
            IViewModelStorageService viewModelStorageService): base
            (viewModelStorageService)
        {
            r_localPlayerService = localPlayerService;
            RestartCommand.Subscribe(RestartCommandExecuted()).AddTo(r_disposables);
            ContinueByAdCommand.Subscribe(ContinueByAdCommandExecuted()).AddTo(r_disposables);

            localPlayerService.PlayerModel.CurrentDistance
                .Subscribe(newValue => Distance.Value = newValue)
                .AddTo(r_disposables);
        }

        private Action<Unit> ContinueByAdCommandExecuted()
        {
            return _ =>
            {
                Debug.Log("Watching reward ad!");
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
        
    }
}