using Core.Extension;
using UnityEngine;
using Zenject;

namespace UI.Views.Upgrades
{
    public abstract class BaseView: MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _thisViewCanvasGroup;
        
        protected IViewModelStorageService IViewModelStorageService;

        [Inject]
        public void Constructor(IViewModelStorageService iViewModelStorageService)
        {
            IViewModelStorageService = iViewModelStorageService;
        }
        
        private void OnEnable() => SubscribeToEvents();

        private void OnDisable() => UnSubscribeFromEvents();

        protected abstract void SubscribeToEvents();
        protected abstract void UnSubscribeFromEvents();
        
        public virtual void ShowView(float duration = 0.5f)
        {
            _thisViewCanvasGroup?.SetActive(true, duration);
        }

        public virtual void HideView(float duration = 0f)
        {
            _thisViewCanvasGroup?.SetActive(false, duration);
        }
    }
}