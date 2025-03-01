using System;
using Core.Extension;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Views.Upgrades
{
    public abstract class BaseView: MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _thisViewCanvasGroup;
        
        protected IViewModelStorageService IViewModelStorageService;
        protected CompositeDisposable _disposable;

        [Inject]
        public void Constructor(IViewModelStorageService iViewModelStorageService)
        {
            IViewModelStorageService = iViewModelStorageService;
        }
        
        private void OnEnable()
        {
            _disposable = new CompositeDisposable();
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnSubscribeFromEvents();
            _disposable.Dispose();
            _thisViewCanvasGroup.DOKill();
        }

        private void OnDestroy()
        {
            _thisViewCanvasGroup = null;
        }

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