using Patterns.Models;
using Patterns.ViewModels;
using TapToStep.Scripts.Core.Service.ViewStorage;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class ViewModelInstaller: MonoInstaller
    {

        public override void InstallBindings()
        {
            BindViewStorageService();
            
            Container.Bind<LoadingViewModel>().AsSingle();
            Container.Bind<TutorialViewModel>().AsSingle();
            Container.Bind<GameViewModel>().AsSingle();
            Container.Bind<DeadViewModel>().AsSingle();

            LoadAllViewModelToStorage();
        }

        private void LoadAllViewModelToStorage()
        {
            
        }
        
        private void BindViewStorageService()
        {
            Container.BindInterfacesAndSelfTo<ViewModelModelStorageService>().AsSingle().NonLazy();
        }
    }
}