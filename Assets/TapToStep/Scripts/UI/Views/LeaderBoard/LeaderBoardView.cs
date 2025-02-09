using System;
using Runtime.EntryPoints.EventHandlers;
using Runtime.Player.Perks;
using UI.Views.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.LeaderBoard
{
    public class LeaderBoardView : BaseView
    {
        [SerializeField] private LeaderBoardBuilder _boardBuilder;
        [SerializeField] private Button _backButton;

        private GameEventHandler _gameEventHandler;

        public event Action OnBackButtonPressed;
        
        [Inject]
        public void Constructor(GameEventHandler gameEventHandler)
        {
            _gameEventHandler = gameEventHandler;
        }
        
        public void Init()
        {
            _backButton.onClick.AddListener(BackButtonClicked);
        }
        
        public override void ShowView(float duration = 0.5f)
        {
            base.ShowView(duration);
            _thisViewCanvasGroup.interactable = false;
            _boardBuilder.CreateBoard(OnBoardCreated);
        }

        public override void HideView(float duration = 0.5f)
        {
            base.HideView(duration);
            _boardBuilder.DestroyBoard();
        }

        private void OnBoardCreated()
        {
            _thisViewCanvasGroup.interactable = true;
        }

        private void BackButtonClicked()
        {
            _gameEventHandler.InvokeOnUiElementClicked();
            OnBackButtonPressed?.Invoke();
        }
    }
}