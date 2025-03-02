using TapToStep.Scripts.Core.Service.ViewStorage;
using UI.ViewModels;
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
            Container.Bind<MainMenuViewModel>().AsSingle();
        }
        
        private void BindViewStorageService()
        {
            Container.BindInterfacesAndSelfTo<ViewModelModelStorageService>().AsSingle().NonLazy();
        }
    }
}